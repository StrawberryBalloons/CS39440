using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(WaterMechanics))]
public class WaterAdder : Editor
{
    public override void OnInspectorGUI()
    {
        WaterMechanics wMech = (WaterMechanics)target;
        int waterX = GameObject.Find("WaterMeshy").GetComponent<WaterElement>().waterX;
        int waterY = GameObject.Find("WaterMeshy").GetComponent<WaterElement>().waterY;
        float waterAmount = GameObject.Find("WaterMeshy").GetComponent<WaterElement>().waterAmount;
        if (GUILayout.Button("AddWater"))
        {
            for (int i = 0; i < waterAmount; i++)
            {
                wMech.addWater(waterX, waterY, 0.5f);
            }
        }
    }
}

