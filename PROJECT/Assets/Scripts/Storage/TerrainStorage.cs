using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class TerrainStorage: Updater
{
    public float mHMultiplier;
    public AnimationCurve mhCurve;
    public bool useFallOff;

    public float minHeight
    {
        get
        {
            return 2.5f * mHMultiplier * mhCurve.Evaluate(0);
        }
    }

    public float maxHeight
    {
        get
        {
            return 2.5f * mHMultiplier * mhCurve.Evaluate(1);
        }
    }
}
