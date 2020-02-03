using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOff
{
    public static float[,] GenFallOffMap(int size)
    {
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

    static float Eval(float value)
    {
        float a = 3;
        float b = 8.0f;
        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
    }
}
    
