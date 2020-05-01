using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TreeLSystem : MonoBehaviour
{
    public int iterations;
    public float angle;
    public float width;
    public float variance;
    public float mutationChance;
    public float dominantParent;

    public float minLeafLength;
    public float maxLeafLength;
    public float minBranchLength;
    public float maxBranchLength;

    public GameObject patriarch;
    public GameObject branch;
    public GameObject leaf;
    public GameObject tree;
    public GameObject matriarch;

    private const string axiom = "X";

    private Dictionary<char, string> rules = new Dictionary<char, string>();
    private Stack<TreeTransform> transformStack = new Stack<TreeTransform>();

    private Vector3 intialPosition;
    Vector2 boundsMinMaxX;
    Vector2 boundsMinMaxY;
    private static TreeLSystem instance;
    public static TreeLSystem Instance { get { return instance; } }
    private string currentPath = " ";
    private float[] randomRotations;
    private Vector3 initialPosition;
    List<TreeElement> allLines;
    List<Coroutine> animationRoutines;
    private void Awake()
    {
        instance = this;
        randomRotations = new float[1000];
        for (int i =0; i<randomRotations.Length;i++)
        {
            randomRotations[i] = UnityEngine.Random.Range(-2f, 2f);
        }
        rules.Add('X', "[-FX][+FX][FX]");
        rules.Add('F', "FF");

        Generate();
    }
    public void setTreeProgenitor(GameObject parent)
    {
        patriarch = parent;
    }
    private int getMaxGrowthStage(GameObject patriarch, GameObject matriarch, float chance)
    {
        if (chance < mutationChance)
        {
            return patriarch.GetComponent<TreeMechanics>().getMaxGrowthStage() + UnityEngine.Random.Range(-1, 1);
        }
        else if (chance > mutationChance && chance < mutationChance + dominantParent)
        {
            return patriarch.GetComponent<TreeMechanics>().getMaxGrowthStage();
        }
        else
        {
            return matriarch.GetComponent<TreeMechanics>().getMaxGrowthStage();
        }
    }

    private float getRepoChance(GameObject patriarch, GameObject matriarch, float chance)
    {
        if (chance < mutationChance)
        {
            return patriarch.GetComponent<TreeMechanics>().getRepoChance() * UnityEngine.Random.Range(0.9f, 1.1f);
        }
        else if (chance > mutationChance && chance < mutationChance + dominantParent)
        {
            return patriarch.GetComponent<TreeMechanics>().getRepoChance();
        }
        else
        {
            return matriarch.GetComponent<TreeMechanics>().getRepoChance();
        }
    }

    private int getTreeWaterRadius(GameObject patriarch, GameObject matriarch, float chance)
    {
        if (chance < mutationChance)
        {
            return patriarch.GetComponent<TreeMechanics>().getTreeWaterRadius() + UnityEngine.Random.Range(-1, 1);
        }
        else if (chance > mutationChance && chance < mutationChance + dominantParent)
        {
            return patriarch.GetComponent<TreeMechanics>().getTreeWaterRadius();
        }
        else
        {
            return matriarch.GetComponent<TreeMechanics>().getTreeWaterRadius();
        }
    }

    private float getMateRadius(GameObject patriarch, GameObject matriarch, float chance)
    {
        if (chance < mutationChance)
        {
            return patriarch.GetComponent<TreeMechanics>().getMateRadius() * UnityEngine.Random.Range(0.9f, 1.1f);
        }
        else if (chance > mutationChance && chance < mutationChance + dominantParent)
        {
            return patriarch.GetComponent<TreeMechanics>().getMateRadius();
        }
        else
        {
            return matriarch.GetComponent<TreeMechanics>().getMateRadius();
        }
    }

    private int mutateChance()
    {
        return UnityEngine.Random.Range(0, 100);
    }
    private void Generate()
    {

        float chance = mutateChance();
        allLines = new List<TreeElement>();
        boundsMinMaxX = new Vector2(float.MaxValue, float.MinValue);
        boundsMinMaxY = new Vector2(float.MaxValue, float.MinValue);

        Destroy(tree);
        tree = Instantiate(patriarch, transform.position, Quaternion.identity);
        if (matriarch == null)
        {
            matriarch = patriarch;
        }
        /** MECHANICS **/
        tree.GetComponent<TreeMechanics>().setMateRadius(getMateRadius(patriarch, matriarch, chance));
        tree.GetComponent<TreeMechanics>().setTreeWaterRadius(getTreeWaterRadius(patriarch, matriarch, chance));
        tree.GetComponent<TreeMechanics>().setRepoChance(getRepoChance(patriarch, matriarch, chance));
        tree.GetComponent<TreeMechanics>().setMaxGrowthStage(getMaxGrowthStage(patriarch, matriarch, chance));


        Debug.Log("Instantiate tree at " + transform.position);

        currentPath = axiom;
        StringBuilder stringBuilder = new StringBuilder();

        //GROW A STRING PATH
        for (int i = 0; i< iterations; i++)
        {
            char[] currentPathChars = currentPath.ToCharArray();
            for (int n = 0; n < currentPathChars.Length; n++)
            {
                stringBuilder.Append(rules.ContainsKey(currentPathChars[n]) ? rules[currentPathChars[n]] : currentPathChars[n].ToString());
            }

            currentPath = stringBuilder.ToString();
            stringBuilder = new StringBuilder();
        }

        //DRAWING LINES
        for (int k = 0; k< currentPath.Length; k++)
        {
            switch (currentPath[k])
            {
                case 'F':
                    initialPosition = transform.position;
                    bool isLeaf = false;
                    GameObject currentElement;
                    if (currentPath[k + 1] % currentPath.Length == 'X' || currentPath[k+3] % currentPath.Length == 'F' && currentPath[k+4] % currentPath.Length == 'X')
                    {
                        currentElement = Instantiate(leaf);
                        isLeaf = true;
                    } else
                    {
                        currentElement = Instantiate(branch);
                    }

                    currentElement.transform.SetParent(tree.transform);
                    //access linerenderer from current tree element
                    TreeElement currentTreeElement = currentElement.GetComponent<TreeElement>();
                    //star and end point of line renderer
                    currentTreeElement.lineRenderer.SetPosition(0, initialPosition);

                    if (isLeaf)
                    {
                        transform.Translate(Vector3.up * 2f * UnityEngine.Random.Range(minLeafLength, maxLeafLength));
                    }
                    else
                    {
                        transform.Translate(Vector3.up * 2f * UnityEngine.Random.Range(minBranchLength, maxBranchLength));
                    }
                    currentTreeElement.lineRenderer.SetPosition(1, transform.position);
                    if (isLeaf)
                    {
                        currentTreeElement.lineRenderer.startWidth = width * 2f;
                        currentTreeElement.lineRenderer.endWidth = width / 4f;
                        currentTreeElement.isLeaf = true;
                    }
                    else
                    {
                        currentTreeElement.lineRenderer.startWidth = width;
                        currentTreeElement.lineRenderer.endWidth = width;
                    }
                    currentTreeElement.lineRenderer.sharedMaterial = currentTreeElement.material;
                    allLines.Add(currentTreeElement);
                    break;


                case 'X':
                    break;
                case '+':
                    transform.Rotate(Vector3.back * angle * (1 + variance / 100 * randomRotations[k % randomRotations.Length]));
                    transform.Rotate(Vector3.right * angle * (1 + variance / 100 * randomRotations[k % randomRotations.Length]));
                    break;
                case '-':
                    transform.Rotate(Vector3.forward * angle * (1 + variance / 100 * randomRotations[k % randomRotations.Length]));
                    transform.Rotate(Vector3.left * angle * (1 + variance / 100 * randomRotations[k % randomRotations.Length]));
                    break;
                case '*':
                    transform.Rotate(Vector3.up * 120f * (1f + variance / 100f * randomRotations[k % randomRotations.Length]));
                    transform.Rotate(Vector3.left * 120f * (1f + variance / 100f * randomRotations[k % randomRotations.Length]));
                    break;
                case '/':
                    transform.Rotate(Vector3.down * 120f * (1f + variance / 100f * randomRotations[k % randomRotations.Length]));
                    transform.Rotate(Vector3.right * 120f * (1f + variance / 100f * randomRotations[k % randomRotations.Length]));
                    break;

                case '[':
                    transformStack.Push(new TreeTransform(transform.position, transform.rotation));
                    break;
                case ']':
                    TreeTransform treeTransform = transformStack.Pop();

                    transform.position = treeTransform.Position;
                    transform.rotation = treeTransform.Rotation;
                    break;
            }
            boundsMinMaxX.x = Mathf.Min(transform.position.x, boundsMinMaxX.x);
            boundsMinMaxX.y = Mathf.Max(transform.position.x, boundsMinMaxX.y);
            boundsMinMaxY.x = Mathf.Min(transform.position.y, boundsMinMaxY.x);
            boundsMinMaxY.y = Mathf.Max(transform.position.y, boundsMinMaxY.y);
        }
    }
}
