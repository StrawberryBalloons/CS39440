using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRender;
    public MeshFilter mFilter;
    private MeshCollider mCollider;
    private GameObject mObj;
    public MeshRenderer mRenderer;

    public void DrawTexture(Texture2D texture)
    {
 
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MData mData, Texture2D tex)
    {

        if (!GameObject.Find("Meshy"))
        {
            mObj = new GameObject("Meshy");
        }
        mFilter.sharedMesh = mData.cMesh();
        mRenderer.sharedMaterial.mainTexture = tex;
        DestroyImmediate(GameObject.Find("Meshy").GetComponent<MeshCollider>());
        //Destroy(GameObject.Find("Meshy").GetComponent<MeshCollider>());
        GameObject.Find("Meshy").AddComponent<MeshCollider>();
        
    }
}
