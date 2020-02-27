using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOff
{
    //not sure if I will use this class, may become obsolete soon if I decide I don't want islands
    public static float[,] GenFallOffMap(int size)
    {
        //generates a height map with all the borders being black
        float[,] map = new float[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = i / (float)size * 2 - 1;
                float y = j / (float)size * 2 - 1;

                float val = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                map[i, j] = Eval(val); 
            }
        }

        return map;
    }

    //can't remember what this one does, I think it adjusts border size
    static float Eval(float value)
    {
        float a = 3;
        float b = 8.0f;
        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
    }
}
    
