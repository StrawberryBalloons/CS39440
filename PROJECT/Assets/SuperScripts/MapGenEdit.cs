using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (MapGen))]
public class MapgenEdit : Editor
{

    public override void OnInspectorGUI()
    {
            MapGen mGen = (MapGen)target;
        if (DrawDefaultInspector())
        {
            if (mGen.autoUpdate)
            {
                mGen.GenMap();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            mGen.GenMap();
        }
    }
}
