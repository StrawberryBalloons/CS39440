using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class WaterStorage : Updater
{

    public Color[] waterColours;
    [Range(0, 1)]
    public float[] waterStartHeights;
    [Range(0, 1)]
    public float[] waterBlends;

    float savedMin;
    float savedMax;
    public void ApplyToMaterial(Material material)
    {
        material.SetInt("waterColourCount", waterColours.Length);
        material.SetColorArray("waterColours", waterColours);
        material.SetFloatArray("waterStartHeights", waterStartHeights);
        material.SetFloatArray("waterBlends", waterBlends);

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
