using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TreeTransform
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 Position { get { return position; } }
    public Quaternion Rotation { get { return rotation; } }

    public TreeTransform(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}
