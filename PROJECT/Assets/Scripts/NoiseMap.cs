using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMap{
    public static float[,] GenMap(int width, int height, float modifier){
        float[,] map = new float[width, height];

        if (modifier <= 0)
        {
            modifier = 0.001f;
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float X = x / modifier;
                float Y = y / modifier;

                float perlin = Mathf.PerlinNoise(X, Y);
                map[x, y] = perlin;
            }
        }
        return map;
    }
}
