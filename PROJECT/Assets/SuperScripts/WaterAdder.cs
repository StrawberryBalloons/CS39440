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
        if (GUILayout.Button("AddWater"))
        {
            wMech.addWater(50, 50, 0.01f);
        }
    }
}

