using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class WaterOverlay : Updater
{
    public float[,] waterOverlay;

    public float[,] getOverlay()
    {
        return waterOverlay;
    }
    public void initOverlay()
    {
        waterOverlay = new float[GameObject.Find("MapGen").GetComponent<MapGen>().height, GameObject.Find("MapGen").GetComponent<MapGen>().width];
        for (int x = 0; x < GameObject.Find("MapGen").GetComponent<MapGen>().height; x++)
        {
            for (int y = 0; y < GameObject.Find("MapGen").GetComponent<MapGen>().width; y++)
            {
                waterOverlay[x, y] = 0;
            }
        }
    }

    public void addToOverlay()
    {
        for (int x = 0; x < GameObject.Find("MapGen").GetComponent<MapGen>().height; x++)
        {
            for (int y = 0; y < GameObject.Find("MapGen").GetComponent<MapGen>().width; y++)
            {
                waterOverlay[x, y] = 0;//add in person wateroverlay to this
            }
        }
    }

}