using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMechanics : MonoBehaviour
{
    public TerrainStorage terrainStorage;
    public NoiseStorage noiseStorage;
    private GameObject mObj;

    int width;
    int height;
    float[,] waterOverlay;
    float[,] terrainMap;
    float[,] adjustedMap;
    MapDisplay display;

    void Start()
    {
        display = FindObjectOfType<MapDisplay>();
        //get the paramaters to copy
        mObj = GameObject.Find("MapGen");

        height = mObj.GetComponent<MapGen>().height;
        width = mObj.GetComponent<MapGen>().width;

        //makes the water mesh the same shape as the terrain mesh initially
        terrainMap = NoiseMap.GenMap(width, height, noiseStorage.noiseModifier, noiseStorage.oct, noiseStorage.persist, noiseStorage.lac);
        display.DrawWaterMesh(MeshGen.GenMesh(terrainMap, terrainStorage.mHMultiplier, terrainStorage.mhCurve));
        waterOverlay = new float[width, height];
        adjustedMap = new float[width, height];

        //init overlay to no water(all 0's)
        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                adjustedMap[x, y] = terrainMap[x, y];
                waterOverlay[x, y] = 0;
            }
        }

        addWater(2, 2, 1);

    }

    // Update is called once per frame
    void Update()
    {

    }

    //x and y for now but may need to take location dynamically
    public void addWater(int x, int y, float input)
    {
        waterOverlay[x, y] = input;
        if (waterOverlay[x, y] - input == 0)
        {
            waterFlow(x, y);
        }
        else {
            waterDisperse(x, y);
        }
        applyWater();
    }

    private void waterDisperse(int x, int y)
    {
        throw new NotImplementedException();
    }

    //need to account for out of bounds in code
    private void waterFlow(int x, int y)
    {
        float sHeight, smallest;
        int sx, sy;
        sx = x;
        sy = y;
        smallest = 1;
        sHeight = terrainMap[x, y];//terrain height of water source(start height)
        sHeight += waterOverlay[x, y];//water plus height of terrain = adjusted water height
        for (int xx = -1; xx < 2; xx++)//checks the surrounding vertices
        {
            for (int yy = -1; yy < 2; yy++)
            {
                Debug.Log("xx = " + xx + " yy = " + yy);
                Debug.Log("x = " + x + " y = " + y);
                if (sHeight > terrainMap[x + xx, y + yy])//if height of water is greater than the surrounding terrain
                {
                    if (terrainMap[x + xx, y + yy] < smallest)//find the lowest point and take the value
                    {
                        smallest = terrainMap[x + xx, y + yy];
                        sx = x + xx;
                        sy = y + yy;
                    }
                }
            }
        }

        if (waterOverlay[sx, sy] == 0 && (sx != x && sy != y))//if the water flow continues
        {
            waterOverlay[sx, sy] = waterOverlay[x, y];//moves the water in a flow, there is no loss
            waterOverlay[x, y] = 0;
            waterFlow(sx, sy);//recursive 
        } else if(waterOverlay[sx, sy] > 0 && (sx != x && sy != y)) //if the water flows into a body of water
        {
            waterDisperse(sx, sy);
        } else if (sx == x && sy == y) //water has found a minimum, if sx and sy are the same as x and y then there is no lower area around them
        {
            //not sure if I need anything here
        }
    }

    private void applyWater()
    {
        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                adjustedMap[x, y] = terrainMap[x, y] + waterOverlay[x, y];
            }
        }
        display.DrawWaterMesh(MeshGen.GenMesh(adjustedMap, terrainStorage.mHMultiplier, terrainStorage.mhCurve));
    }
}
