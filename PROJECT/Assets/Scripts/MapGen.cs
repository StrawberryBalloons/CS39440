using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    //E037M Leftoff

        public enum Draw
    {
        nMap, cMap, mesh, fallOff
    };
    public Draw draw;
    public int width;
    public int height;
    public float noiseModifier;

    public int oct; //octaves
    public float persist; //persistance
    public float lac; //lacuna
    public float mHMultiplier;
    float[,] fall;

    public AnimationCurve mhCurve;

    public bool autoUpdate;
    public bool useFallOff;

    public Terrain[] biomes;

    private void Awake()
    {
        fall = FallOff.GenFallOffMap(100);
    }

    public void GenMap()
    {
        fall = FallOff.GenFallOffMap(100); 
        float[,] nMap = NoiseMap.GenMap(width, height, noiseModifier, oct, persist, lac);
        Color[] cMap = new Color[width * height];
        for(int y = 0; y <height; y++)
        {
            for(int x =0; x < width; x++)
            {
                if (useFallOff)
                { 
                    nMap[x, y] = Mathf.Clamp01(nMap[x, y] - fall[x, y]);
                }
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
        } else if(draw == Draw.mesh)
        {
            display.DrawMesh(MeshGen.GenMesh(nMap, mHMultiplier, mhCurve ), (textureGen.textureColourMap(cMap, width, height)));
        } 
        else if (draw == Draw.fallOff)
        {
            display.DrawTexture(textureGen.textureFromHeight(FallOff.GenFallOffMap(100)));
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
