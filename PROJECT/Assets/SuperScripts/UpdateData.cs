using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Updater), true)]
public class UpdateData : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Updater data = (Updater)target;
        if (GUILayout.Button("update"))
        {
            data.UpdateValues();
        }
    }
}
