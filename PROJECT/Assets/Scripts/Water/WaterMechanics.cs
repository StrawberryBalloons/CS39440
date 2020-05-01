using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class WaterMechanics : MonoBehaviour
{
    public TerrainStorage terrainStorage;
    public NoiseStorage noiseStorage;
    public WaterOverlay waterMap;
    private GameObject mObj;
    public float[,] waterOverlay;
    public float[,] terrainMap;
    float[,] adjustedMap;

    int width;
    int height;

    MapDisplay display;

    public void deductWater(int x, int y, float treeConsumption)
    {
        waterOverlay[x, y]-= treeConsumption;
        if (waterOverlay[x, y] < 0){
            waterOverlay[x, y] = 0;
        }
    }

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
        initOverlay();//init overlay to no water(all 0's)
    }

    //need to account for out of bounds in code
    private void waterFlow(int prevX, int prevY, float input)
    {
        float currentHeight , smallest;
        int nextX, nextY;
        nextX = prevX;
        nextY = prevY;
        currentHeight = terrainMap[prevX, prevY] + waterOverlay[prevX, prevY];
        smallest = currentHeight;
        //Debug.Log("water flow");
        for (int x = -1; x < 2; x++)//checks the surrounding vertices
        {
            for (int y = -1; y < 2; y++)
            {
                int ox = prevX + x;
                int oy = prevY + y;
                checkBounds(ref ox, ref oy);
                //Debug.Log("x = " + prevX + " y = " + prevY);
                if (currentHeight > terrainMap[ox, oy] + waterOverlay[ox, oy])//if height of water is greater than the surrounding terrain (changed to terrainmap + waterOverlay) terrainMap[ox, oy] + waterOverlay to adjusted
                {
                    if (smallest > terrainMap[ox, oy] + waterOverlay[ox, oy])//take the height of the location with the steepest gradient (changed to terrainmap + waterOverlay)
                    {
                        if (ox > 0 && oy > 0 && ox < 100 && oy < 100)//water doesn't go past the borders
                        {
                            smallest = terrainMap[ox, oy] + waterOverlay[ox, oy]; // (changed to terrainmap + waterOverlay)
                            nextX = ox;
                            nextY = oy;
                        }
                    }
                }
            }
        }
        waterOptionSelect(prevX, prevY, nextX, nextY, input);
    }

    private static void checkBounds(ref int ox, ref int oy)
    {
        if (ox < 1)
        {
            ox = 1;
        }
        else if (oy > 99)
        {
            oy = 99;
        }
        else if (oy < 1)
        {
            oy = 1;
        }
        else if (ox > 99)
        {
            ox = 99;
        }
    }

    private void waterOptionSelect(int originX, int originY, int newX, int newY, float input)
    { 
        float adjustedOrigin = terrainMap[originX, originY] + waterOverlay[originX, originY];//height of originX originY
        float adjustedNew = terrainMap[newX, newY] + waterOverlay[newX, newY]; //height of newX newY
        if (waterOverlay[originX, originY] > 0.0f && waterOverlay[newX, newY] > 0.0f && waterOverlay[originX, originY] >= waterOverlay[newX, newY] && (newX != originX || newY != originY))
        { 
            //Debug.Log("overlay " + originX + " " + originY + " moving to " + newX + " " + newY);
            //Debug.Log("origin " + terrainMap[originX, originY] + " " + waterOverlay[originX, originY] + "  new " + terrainMap[newX, newY] + " " + waterOverlay[newX, newY]);
            //this should keep the water flowing instead of spiking
            waterFlow(newX, newY, input);//recursive 
        }
        else if (adjustedNew < adjustedOrigin && (newX != originX || newY != originY) && newX != 100 && newY != 100)//if the water height of the new is less than the original
        {
            waterFlow(newX, newY, input);//recursive 
        }
        else if (newX == originX && newY == originY) //water has found a minimum, if sx and sy are the same as x and y then there is no lower area around them
        {
            //Debug.Log("basin " + newX + " " + newY + " water " + waterOverlay[newX, newY]);
            basin(newX, newY, input);
        } else
        {
            Debug.Log("Error");
        }

    }

    private void basin(int x, int y, float water)
    {
        waterOverlay[x, y] += water;//moves the water in a flow, there is no loss (changed to += input)
        applyWater();
    }

    private void initOverlay()
    {
        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                waterOverlay[x, y] = 0;
                adjustedMap[x, y] = terrainMap[x, y];
            }
        }
    }
    public float[,] getWaterOverlay()
    {
        return waterOverlay;
    }


    //x and y for now but may need to take location dynamically
    public void addWater(int x, int y, float input)
    {
        waterFlow(x, y, input);
        applyWater();
    }

    private void applyWater()
    {
        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                if (waterOverlay[x, y] > 0)
                {
                    adjustedMap[x, y] += waterOverlay[x, y];
                }
            }
        }
        display.DrawWaterMesh(MeshGen.GenWMesh(terrainMap, terrainStorage.mHMultiplier, terrainStorage.mhCurve, waterOverlay));
    }
}
