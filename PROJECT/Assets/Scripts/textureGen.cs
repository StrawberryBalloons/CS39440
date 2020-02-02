using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class textureGen
{
   public static Texture2D textureColourMap(Color[] cMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(cMap);
        texture.Apply();
        return texture;
    }

    public static Texture2D textureFromHeight(float[,] hMap)
    {
        int width = hMap.GetLength(0);
        int height = hMap.GetLength(1);

        Color[] colourMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, hMap[x, y]);
            }
        }
        return textureColourMap(colourMap, width, height);
    }

}
