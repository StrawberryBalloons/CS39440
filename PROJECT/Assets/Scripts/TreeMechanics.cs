using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeMechanics : MonoBehaviour
{
    private float time = 6.0f;
    public float increment = 6.0f;
    float mateRadius = 50;
    int treeWaterRequirements = 1;
    int growthStage = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }
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
                chuckSeed();
                if (growthStage < 3)
                {
                    growthStage++;
                }
            }
            else if (waterLogged() || waterStarved())
            {
                Destroy(); //maybe turn into dead wood?
            }
        }
    }

    private int convertToMeshX()
    {
        throw new NotImplementedException();
    }
    private int ConvertToMeshY()
    {

        throw new NotImplementedException();
    }

    private bool waterStarved()
    {
        Debug.Log("WaterStarved");
        float[,] waterOverlay = getOverlay();
        for (int x = -treeWaterRequirements; x < treeWaterRequirements + 1; x++)//checks the surrounding vertices
        { 
            for (int y = -treeWaterRequirements; y < treeWaterRequirements + 1; y++)
            {
                if (waterOverlay[x + getX(), y + getY()] > 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void Destroy()
    {
        Debug.Log("Destroy");
        Destroy(gameObject);
    }

    private bool waterLogged()
    {
        Debug.Log("WaterLogged");
        float[,] waterOverlay = getOverlay();
        if (waterOverlay[getX(), getY()] > 0)
        {
            return true;
        }
        return false;
    }

    private static float[,] getOverlay()
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
        if (spawnPos.y > getHeight(getX(), getY()))
        {
            Debug.Log("Seed Thrown");
            //Seed seed = Instantiate(seeds, transform.position, transform.rotation); //this is throwing a null error
            //seed.Throw();
            spawnPos = getNewSpawnPos();
            GameObject.Find("Meshy").GetComponent<TreeGen>().spawnChildTree(spawnPos);
        } else
        {
            Debug.Log("Seed not thrown");
        }
    }

    private Vector3 getNewSpawnPos()
    {
        int x, y;
        x = 0;
        y = 0;
        while (x == 0 && y == 0 )
        {
            x = UnityEngine.Random.Range(-growthStage, growthStage);
            y = UnityEngine.Random.Range(-growthStage, growthStage);
        }
        return new Vector3(transform.position.x + x, transform.position.y + y, getHeight(x, y));
    }

    private float getHeight(int x, int y)
    {
        float[,] terrain = GameObject.Find("WaterMeshy").GetComponent<WaterMechanics>().terrainMap;
        //Vector3 worldPt = transform.TransformPoint(meshVert);// changes the xyz of the mesh to xyz of the world space
        Debug.Log(terrain[x, y] - 81 +" "+  transform.position.y) ;
        return terrain[x, y] - 81;
    }

    private bool nearbyTree()
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
        Collider closest = colliders[0];
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].transform.position == transform.position)
            {

            }
            if (Vector3.Distance(transform.position, colliders[i].transform.position) <= Vector3.Distance(transform.position, closest.transform.position))
            {
                closest = colliders[i];
            }
        }

        return closest;
    }
}
