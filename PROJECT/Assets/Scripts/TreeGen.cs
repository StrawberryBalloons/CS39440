using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        GameObject adamTree = GameObject.CreatePrimitive(PrimitiveType.Cube);
        treeSpawn(adamTree);
        treeSpawn(adamTree);
    }

    private static void treeSpawn(GameObject tree)
    {
        Instantiate(tree);
        //tree.AddComponent(typeof(MeshFilter));
        //tree.AddComponent(typeof(MeshRenderer));
        tree.AddComponent<Rigidbody>();
        tree.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        tree.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
        tree.AddComponent<TreeMechanics>();
        tree.transform.localScale = new Vector3(5, 15, 5);
        tree.transform.localPosition = findNoWater();//1,1 is the top left most and is equivalent to -490, 490 so multiply by 10 and -500
        tree.transform.gameObject.tag = "tree";
    }

    private static Vector3 findNoWater()
    {
        int x, y, count;
        count = 0;
        x = 0;
        y = 0;
        bool notOnWater = false;
        float[,] waterOverlay = GameObject.Find("WaterMeshy").GetComponent<WaterMechanics>().waterOverlay;

        while (notOnWater == false){
            x = UnityEngine.Random.Range(1, 100);
            y = UnityEngine.Random.Range(1, 100);
            if (waterOverlay[x, y] == 0)
            {
                notOnWater = true;
            }
            if (count > 9000)
            {
                break;
            }
            count++;
        }
        return new Vector3(x * 10, 100, y * 10); //multiplied by 10 to get scale
    }

    public void spawnChildTree(Vector3 seedLoc)
    {
        GameObject tree = CreateTree();
        Instantiate(tree);
        tree.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        tree.AddComponent<TreeMechanics>();
        tree.transform.localScale = new Vector3(5, 15, 5);//need to change to location of orb
        tree.transform.localPosition = seedLoc;
        transform.gameObject.tag = "tree";
    }

    private GameObject CreateTree()
    {
        return new GameObject();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
