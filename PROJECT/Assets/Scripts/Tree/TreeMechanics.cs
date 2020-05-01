using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeMechanics : MonoBehaviour
{
    [HideInInspector]
    private float time = 6.0f;
    [HideInInspector]
    public float increment = 6.0f;

    public float mateRadius;
    public int treeWaterRadius;
    [HideInInspector]
    public int growthStage = 0;
    [HideInInspector]
    public GameObject parentTree;
    public float repoChance;
    public int maxGrowthStage;

    private void OnCollisionEnter(Collision other)
    {
        this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time > time)
        {
            time = Time.time + increment;
            // THIS CODE COULD BE EXTREMELY LAGGY think about putting it into tree gen and then calling the function with the code below
            if (nearbyTree())
            {
                if (reproductionChance() < repoChance) //simulates chances of reproducing every cycle
                {
                    chuckSeed();
                }
                if (growthStage < maxGrowthStage)//how old the tree is
                {
                    growthStage++;
                }
            }
            if (waterLogged() || waterStarved())
            {
                Destroy(); //replace this with making the leaves brown and changing repoChance to 0%
            }
        }
    }
    public void setMaxGrowthStage(int var)
    {
        maxGrowthStage = var;
    }

    public void setRepoChance(float var)
    {
        repoChance = var;
    }

    public void setTreeWaterRadius(int var)
    {
        treeWaterRadius = var;
    }

    public void setMateRadius(float var)
    {
        mateRadius = var;
    }
    public int reproductionChance()
    {
        return UnityEngine.Random.Range(0, 100);
    }
    public int getMaxGrowthStage()
    {
        return maxGrowthStage;
    }

    public float getRepoChance()
    {
        return repoChance;
    }

    public int getTreeWaterRadius()
    {
        return treeWaterRadius;
    }

    public float getMateRadius()
    {
        return mateRadius;
    }

    public bool waterStarved()
    {
        float[,] waterOverlay = getOverlay();
        for (int x = -treeWaterRadius; x < treeWaterRadius + 1; x++)//checks the surrounding vertices
        { 
            for (int y = -treeWaterRadius; y < treeWaterRadius + 1; y++)
            {
                if (waterOverlay[x + getX(), y + getY()] > 0)
                {
                    Debug.Log("Has enough to drink");
                    //deduct by growth stage /10
                    GameObject.Find("WaterMeshy").GetComponent<WaterMechanics>().deductWater(x + getX(), y + getY(), growthStage/10);
                    return false;
                }
            }
        }
        Debug.Log("waterStarved");
        return true;
    }

    private void Destroy()
    {
        Debug.Log("Destroy");
        Destroy(gameObject);
    }
    public void TestDestroy(GameObject obj)
    {
        Destroy(obj);
    }

    public bool waterLogged()
    {
        float[,] waterOverlay = getOverlay();
        Debug.Log(transform.position.x + " " + transform.position.z);
        if (waterOverlay[getX(), getY()] > 0)
        {
            Debug.Log("WaterLogged, is drowning");
            return true;
        }
        return false;
    }

    public static float[,] getOverlay()
    {
        return GameObject.Find("WaterMeshy").GetComponent<WaterMechanics>().waterOverlay;
    }

    private int getY()
    {
        return (int)Mathf.Round(transform.position.z)/10;
    }

    private int getX()
    {
        return (int)Mathf.Round(transform.position.x)/10;
    }
    private void chuckSeed()
    {
        Vector3 spawnPos = transform.position;
        //float terrainHeight = terrain.GetHeight(new Vector2(spawnPos.x, spawnPos.z));
        Debug.Log("spawnpos.y " + spawnPos.y + " get height " + getHeight(getX(), getY())); //The sample of the mesh for the height isn't as accurate and may throw some results off
        Debug.Log("growth stage: " + growthStage    );
        if (spawnPos.y > getHeight(getX(), getY()) && growthStage > 0)
        {
            Debug.Log("Seed Thrown");
            //Seed seed = Instantiate(seeds, transform.position, transform.rotation); //this is throwing a null error
            //seed.Throw();
            spawnPos = getNewSpawnPos(); //freeze
            GameObject.Find("Meshy").GetComponent<TreeGen>().spawnChildTree(spawnPos, parentTree);
        }
        else
        {
            Debug.Log(spawnPos.y + " " + getHeight(getX(), getY()));
            Debug.Log("Seed not thrown");
        }
    }

    private Vector3 getNewSpawnPos()
    {
        Debug.Log("Spawn for child designated");
        int x, y;
        x = 0;
        y = 0;
        while (x == 0 && y == 0 ) //&& should be fine because only 1 needs to be different
        {
            x = UnityEngine.Random.Range(-(growthStage * 10), growthStage * 10);
            y = UnityEngine.Random.Range(-(growthStage * 10), growthStage * 10);
        }
        return new Vector3(transform.position.x + (x), getHeight(getX() + (x), getY() + (y)), transform.position.z + (y));
    }

    private float getHeight(int x, int y)
    {
        float[,] terrain = GameObject.Find("WaterMeshy").GetComponent<WaterMechanics>().terrainMap;
        //Vector3 worldPt = transform.TransformPoint(meshVert);// changes the xyz of the mesh to xyz of the world space
        Debug.Log(x +" "+ y);
        return terrain[x, y] - 81;
    }

    public bool nearbyTree()
    {
        Debug.Log("nearby tree");
        if (getClosest() != null)
        {
            return true;
        }
        return false;
    }
    private Collider getClosest()
    {

        Debug.Log("get closest");
        Collider[] colliders = Physics.OverlapSphere(transform.position, mateRadius);
        Collider closest = null;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (closest == null && colliders[i].tag == "tree")
            {
                closest = colliders[i];
                Debug.Log("First tree found");
            }
            else if (closest != null && colliders[i].tag == "tree")
            {
                Debug.Log("Tree other than 1st found");
                parentTree = colliders[i].transform.gameObject;   
                if (Vector3.Distance(transform.position, colliders[i].gameObject.transform.position) <= Vector3.Distance(transform.position, closest.gameObject.transform.position)) //unnecessary as any tree will do, not just closest
                {
                    closest = colliders[i];
                    parentTree = closest.gameObject;
                    Debug.Log("a tree");
                }
            }
        }
        return closest;
    }
}
