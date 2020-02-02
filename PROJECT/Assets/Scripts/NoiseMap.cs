using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMap{
    public static float[,] GenMap(int width, int height, float modifier, int oct, float persist, float lac){
        float[,] map = new float[width, height];

        if (modifier <= 0)
        {
            modifier = 0.001f;
        }

        float maxHeight = float.MinValue;
        float minHeight = float.MaxValue;

        for (int y = 0; y < height; y++)
        {

            for (int x = 0; x < width; x++)
            {
                float amp = 1;
                float freq = 1;
                float nHeight = 0;

                for (int i = 0; i < oct; i++) {
                    float X = x / modifier * freq;
                    float Y = y / modifier * freq;
                    //the higher the requency the further away the sample points
                    //height values change faster with higher frequency
                    float perlin = Mathf.PerlinNoise(X, Y) * 2 - 1;// *2 - 1 so we can get negative values
                    nHeight += perlin * amp;

                    amp *= persist; //decreses due to persist <=1
                    freq *= lac; //increases due to lac >=1
                }
                if(nHeight > maxHeight){
                    maxHeight = nHeight;//sets range of the map values
                } else{ minHeight = nHeight;
                }
                map[x, y] = nHeight;
            }
        }

        for (int y = 0; y < height; y++){
            for (int x = 0; x < width; x++){
                map[x, y] = Mathf.InverseLerp(minHeight, maxHeight, map[x, y]); //inverselerp makes everything betwwen 0 and 1
            }
        }
                return map;
    }

}
