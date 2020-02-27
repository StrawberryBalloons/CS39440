using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Updater : ScriptableObject
{
    //I should probably put all the things that need updating in here, not just some of them
    public event System.Action OnValueUpdate;
    public bool autoUpdate;

    protected virtual void OnValidate()
    {
        if (autoUpdate)
        {
            UnityEditor.EditorApplication.update += UpdateValues;
        }
    }
    public void UpdateValues()
    {
        UnityEditor.EditorApplication.update -= UpdateValues;
        if (OnValueUpdate != null)
        {
            OnValueUpdate();
        }
    }
}
