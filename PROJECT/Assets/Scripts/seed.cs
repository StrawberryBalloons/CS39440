using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{//given by seb

    public Vector2 throwForceMinMax = new Vector2(4.5f, 5.5f);
    public const float gravity = 10;

    Vector3 velocity;

    //TerrainGeneration.TerrainGenerator terrain;

    void Start()
    {
        //terrain = FindObjectOfType<TerrainGeneration.TerrainGenerator>();

    }

    public void Throw()
    {
        velocity = transform.forward * (UnityEngine.Random.Range(throwForceMinMax.x, throwForceMinMax.y));
    }

    void Update()
    {
        velocity -= Vector3.up * gravity * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;

        float terrainHeight = getHeight((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.z));
        if (transform.position.y < terrainHeight)
        {
            //Vector3 terrainUp = terrain.Raycast(new Vector2(transform.position.x, transform.position.z)).normal;
            float angle = UnityEngine.Random.value * Mathf.PI * 2;
            //var plantRot = Quaternion.LookRotation(new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)), terrainUp);
            //Instantiate(plantPrefab, new Vector3(transform.position.x, terrainHeight, transform.position.z), plantRot);
            Destroy(gameObject);
        }
    }

    private float getHeight(int x, int y)
    {
        float[,] terrain = GameObject.Find("WaterMeshy").GetComponent<WaterMechanics>().terrainMap;
        //Vector3 worldPt = transform.TransformPoint(meshVert);// changes the xyz of the mesh to xyz of the world space
        return terrain[x, y] - 81f;
    }
}