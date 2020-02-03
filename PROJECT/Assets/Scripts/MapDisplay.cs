using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRender;
    public MeshFilter mFilter;
    public MeshRenderer mRenderer;

    public void DrawTexture(Texture2D texture)
    {
 
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MData mData, Texture2D tex)
    {
        mFilter.sharedMesh = mData.cMesh();
        mRenderer.sharedMaterial.mainTexture = tex;
    }
}
