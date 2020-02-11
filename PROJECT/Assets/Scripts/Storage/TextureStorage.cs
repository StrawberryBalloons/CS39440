using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TextureStorage : Updater
{
    public Color[] baseColours;
    [Range(0, 1)]
    public float[] baseStartHeights;
    [Range(0,1)]
    public float[] baseBlends;

    float savedMin;
    float savedMax;
    public void ApplyToMaterial(Material material)
    {
        material.SetInt("baseColourCount", baseColours.Length);
        material.SetColorArray("baseColours", baseColours);
        material.SetFloatArray("baseStartHeights", baseStartHeights);
        material.SetFloatArray("baseBlends", baseBlends);

        UpdateMeshHeights(material, savedMin, savedMax);
    }

    public void UpdateMeshHeights(Material material, float minHeight, float maxHeight)
    {

        savedMin = minHeight;
        savedMax = maxHeight;

        material.SetFloat("minHeight", minHeight);
        material.SetFloat("maxHeight", maxHeight);

    }

}
