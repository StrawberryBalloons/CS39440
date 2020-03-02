using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    //E037M Leftoff


        
        public enum Draw
    {
        nMap, cMap, mesh, fallOff, tMap
    };
    public Draw draw;
    public int width;
    public int height;
    float[,] fall;
    public bool autoUpdate;
    public float[,] nMap;

    public TerrainStorage terrainStorage;
    public NoiseStorage noiseStorage;
    public TextureStorage textureStorage;
    public WaterStorage waterStorage;

    public Terrain[] biomes; //just colours really, only needed for the Colour map now
    public Terrain[] temp; //just colours really, only needed for the Colour map now

    public Material terrainMaterial;
    public Material waterMaterial;

    public int getHeight()
    {
        return this.height;
    }
    public int getWidth()
    {
        return this.width;
    }

    void OnValuesUpdates()
    {
        if (!Application.isPlaying)
        {
            GenMap();//refreshes the map
        }
    }

    private void Awake()
    {
        fall = FallOff.GenFallOffMap(100);//makes an island, may come in useful later
    }

    public void GenMap()
    {
        if (terrainStorage != null)
        {
            terrainStorage.OnValueUpdate -= OnValuesUpdates;
            terrainStorage.OnValueUpdate += OnValuesUpdates;
        }
        if (noiseStorage != null)
        {
            noiseStorage.OnValueUpdate -= OnValuesUpdates;
            noiseStorage.OnValueUpdate += OnValuesUpdates;
        }

        fall = FallOff.GenFallOffMap(100); 
        nMap = NoiseMap.GenMap(width, height, noiseStorage.noiseModifier, noiseStorage.oct, noiseStorage.persist, noiseStorage.lac);
        //gets the Noisemap from the example files
        Color[] cMap = new Color[width * height];
        Color[] tMap = new Color[width * height];
        //gets the colours by width and height
        for(int y = 0; y <height; y++)
        {
            for(int x =0; x < width; x++)
            {
                if (terrainStorage.useFallOff)
                { 
                    nMap[x, y] = Mathf.Clamp01(nMap[x, y] - fall[x, y]);
                }
                float currentHeight = nMap[x, y];
                //gets the value of the noise at location x, y
                for (int i =0; i<biomes.Length;i++)
                {
                    if(currentHeight <= biomes[i].height)
                    {
                        cMap[y * width + x] = biomes[i].colour;
                        //sets a colour based on height
                        break;
                    }
                }
                for (int i = 0; i < temp.Length; i++)
                {
                    if (currentHeight <= temp[i].height)
                    {
                        tMap[y * width + x] = temp[i].colour;
                        //sets a colour based on height
                        break;
                    }
                }
            }
        }

        //This just draws all the different maps
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if(draw == Draw.nMap)
        {
            display.DrawTexture(textureGen.textureFromHeight(nMap));
        } else if (draw == Draw.tMap)
        {
            display.DrawTexture(textureGen.textureColourMap(tMap, width, height));
        }
        else if(draw == Draw.cMap)
        {
            display.DrawTexture(textureGen.textureColourMap(cMap, width, height));
        } else if(draw == Draw.mesh)
        {
            display.DrawMesh(MeshGen.GenMesh(nMap, terrainStorage.mHMultiplier, terrainStorage.mhCurve), (textureGen.textureColourMap(cMap, width, height)));
        } 
        else if (draw == Draw.fallOff)
        {
            display.DrawTexture(textureGen.textureFromHeight(FallOff.GenFallOffMap(100)));
        }
        //updates the mesh heights and terrain material when the presets are changed
        textureStorage.UpdateMeshHeights(terrainMaterial, terrainStorage.minHeight, terrainStorage.maxHeight);
        textureStorage.ApplyToMaterial(terrainMaterial);
        waterStorage.ApplyToMaterial(waterMaterial);

    }


    //think this is obsolete now, may need to remove it
    [System.Serializable]
    public struct Terrain
    {
        public float height;
        public Color colour;
        public string name;
    }
}
