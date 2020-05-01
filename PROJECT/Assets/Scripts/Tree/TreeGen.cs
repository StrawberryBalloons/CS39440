using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGen : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject treeLSystem;
    public GameObject treeGenitor;
    public GameObject branchGenitor;
    public GameObject leafGenitor;
    public int mutationChance;
    public float dominantParent; //which is more likely to take after, mother or father
    private int count = 0;
    private float getMinLeaffatherTree;

    public void begin()
    {
        if (count != 2)
        {
            //GameObject adamTree = GameObject.CreatePrimitive(PrimitiveType.Cube);
            treeSpawn();
            count++;
        }
    }

    private void treeSpawn()
    {
        GameObject tree = treeLSystem; //CreateTree();
        Instantiate(tree, findStartloc(), Quaternion.identity);
        tree.name = "TreeLSystem";
        tree.GetComponent<TreeLSystem>().mutationChance = mutationChance;
        tree.GetComponent<TreeLSystem>().dominantParent = dominantParent;
        tree.GetComponent<TreeLSystem>().matriarch = treeGenitor;
        tree.GetComponent<TreeLSystem>().branch = branchGenitor;
        tree.GetComponent<TreeLSystem>().leaf = leafGenitor;
        tree.GetComponent<TreeLSystem>().patriarch = treeGenitor;
    }


    public void spawnChildTree(Vector3 seedLoc, GameObject motherTree)
    {
        GameObject tree = treeLSystem; //CreateTree();
        Instantiate(tree, seedLoc, Quaternion.identity);
        tree.name = "ChildTreeLSystem";
        tree.GetComponent<TreeLSystem>().mutationChance = mutationChance;
        tree.GetComponent<TreeLSystem>().dominantParent = dominantParent;
        tree.GetComponent<TreeLSystem>().matriarch = motherTree;
        tree.GetComponent<TreeLSystem>().branch = branchGenitor;
        tree.GetComponent<TreeLSystem>().leaf = leafGenitor;
        tree.GetComponent<TreeLSystem>().patriarch = treeGenitor;

    }


    private static Vector3 findStartloc()
    {
        float[,] waterOverlay = GameObject.Find("WaterMeshy").GetComponent<WaterMechanics>().waterOverlay;
        Vector2Int placeTree = findLand(findWater(waterOverlay), waterOverlay);

        return new Vector3(placeTree.x * 10, getHeight(placeTree.x, placeTree.y), placeTree.y * 10); //multiplied by 10 to get scale
    }

    private static float getHeight(int x, int y)
    {
        float[,] terrain = GameObject.Find("WaterMeshy").GetComponent<WaterMechanics>().terrainMap;
        //Vector3 worldPt = transform.TransformPoint(meshVert);// changes the xyz of the mesh to xyz of the world space
        Debug.Log(x + " " + y);
        return terrain[x, y] - 73;
    }

    private static Vector2Int findLand(Vector2Int findWater, float[,] waterOverlay)
    {
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y<2; y++)
            {
                Debug.Log((x + findWater.x) + " " + (y + findWater.y));
                if (waterOverlay[x + findWater.x, y + findWater.y] == 0)
                {
                    if (pingLocation(x + findWater.x, y + findWater.y))
                    {
                        return new Vector2Int(x + findWater.x, y + findWater.y);
                    }
                }
            }
        }
        return new Vector2Int(0,0);
    }

    private static bool pingLocation(int x, int y)
    {
        Vector3 transform = new Vector3(x*10, -73, y*10);
        Collider[] colliders = Physics.OverlapSphere(transform, 10);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].tag == "tree")
            {
                Debug.Log("Pinged Tree");
                return false;
            }
        }
        Debug.Log("No tree pinged");
        return true;
    }


    private static Vector2Int findWater(float[,] waterOverlay)
    {
        for (int x = 0; x < 100; x++)
        {
            for (int y = 0; y< 100; y++)
            {
                if (waterOverlay[x, y] > 0)
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return new Vector2Int(0, 0);
    }


    // Update is called once per frame
    void Update()
    {
        Destroy(GameObject.Find("Cube(Clone)"));
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space button pressed ");
            begin();
        }
    }
}
