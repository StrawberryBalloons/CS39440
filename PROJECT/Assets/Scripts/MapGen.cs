using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    //E037M Leftoff

        public enum Draw
    {
        nMap, cMap
    };
    public Draw draw;
    public int width;
    public int height;
    public float noiseModifier;

    public int oct; //octaves
    public float persist; //persistance
    public float lac; //lacuna

    public bool autoUpdate;

    public Terrain[] biomes;

    public void GenMap()
    { 
        float[,] nMap = NoiseMap.GenMap(width, height, noiseModifier, oct, persist, lac);
        Color[] cMap = new Color[width * height];
        for(int y = 0; y <height; y++)
        {
            for(int x =0; x < width; x++)
            {
                float currentHeight = nMap[x, y];
                for (int i =0; i<biomes.Length;i++)
                {
                    if(currentHeight <= biomes[i].height)
                    {
                        cMap[y * width + x] = biomes[i].colour;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if(draw == Draw.nMap)
        {
            display.DrawTexture(textureGen.textureFromHeight(nMap));
        } else if(draw == Draw.cMap)
        {
            display.DrawTexture(textureGen.textureColourMap(cMap, width, height));
        }
    }

    [System.Serializable]
    public struct Terrain
    {
        public float height;
        public Color colour;
        public string name;
    }
}
