using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public int width;
    public int height;
    public float noiseModifier;

    public bool autoUpdate;

    public void GenMap()
    {
        float[,] nMap = NoiseMap.GenMap(width, height, noiseModifier);

        MapDisplay display = FindObjectOfType<MapDisplay>();
        display.DrawNoiseMap(nMap);
    }
}
