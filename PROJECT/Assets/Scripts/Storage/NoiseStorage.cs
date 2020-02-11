using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class NoiseStorage : Updater
{
    public float noiseModifier;

    public int oct; //octaves
    public float persist; //persistance
    public float lac; //lacuna
}
