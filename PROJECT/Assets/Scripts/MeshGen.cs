﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGen
{//E06LOD LEFT4

    public static MData GenMesh(float[,] hMap, float hMultiplier, AnimationCurve mhCurve)
    {// the reason why the height map is between 0 and 1 and not 0 to 255
        int width = hMap.GetLength(0);
        int height = hMap.GetLength(1);
        float tlX = (width - 1) / -2f;
        float tlZ = (height - 1) / 2f;
        //Debug.Log("MeshGen Width: " + width + " Height: " + height); //100 100
        //Debug.Log("tlX" + tlX + " tlZ " + tlZ); //-49.5 and 49.5
        MData mesh = new MData(width, height);
        int vertIndex = 0;

        //generates a mirror of the height map as a mesh where the white are high areas and the black are low areas
        //uses a curve to make the black areas flat for water, this may need to be changed for the water mechanics
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

                mesh.verticies[vertIndex] = new Vector3(tlX + x, mhCurve.Evaluate(hMap[x, y]) * hMultiplier, tlZ - y);//mhCurve.Evaluate(hMap[x,y]) * hMultiplier is the one that decides height
                mesh.uvs[vertIndex] = new Vector2(x / (float)width, y / (float)height);

                if (x < width - 1 && y < height - 1)
                {
                    mesh.AddTriangle(vertIndex, vertIndex + width + 1, vertIndex + width);
                    mesh.AddTriangle(vertIndex + width + 1, vertIndex, vertIndex + 1);
                }

                vertIndex++;
            }
        }
        return mesh;
    }
    public static MData GenWMesh(float[,] hMap, float hMultiplier, AnimationCurve mhCurve, float[,] waterOverlay)
    {// the reason why the height map is between 0 and 1 and not 0 to 255
        int width = hMap.GetLength(0);
        int height = hMap.GetLength(1);
        float tlX = (width - 1) / -2f;
        float tlZ = (height - 1) / 2f;
        //Debug.Log("MeshGen Width: " + width + " Height: " + height); //100 100
        //Debug.Log("tlX" + tlX + " tlZ " + tlZ); //-49.5 and 49.5
        MData mesh = new MData(width, height);
        int vertIndex = 0;

        //generates a mirror of the height map as a mesh where the white are high areas and the black are low areas
        //uses a curve to make the black areas flat for water, this may need to be changed for the water mechanics
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

                mesh.verticies[vertIndex] = new Vector3(tlX + x, (mhCurve.Evaluate(hMap[x, y]) * hMultiplier) + waterOverlay[x,y], tlZ - y);//mhCurve.Evaluate(hMap[x,y]) * hMultiplier is the one that decides height
                mesh.uvs[vertIndex] = new Vector2(x / (float)width, y / (float)height);

                if (x < width - 1 && y < height - 1)
                {
                    mesh.AddTriangle(vertIndex, vertIndex + width + 1, vertIndex + width);
                    mesh.AddTriangle(vertIndex + width + 1, vertIndex, vertIndex + 1);
                }

                vertIndex++;
            }
        }
        return mesh;
    }
}

public class MData
{
    public Vector3[] verticies;
    public int[] triangles;
    public Vector2[] uvs;

    int triIndex;
    public MData(int mWidth, int mHeight)
    {
        verticies = new Vector3[mWidth * mHeight];
        uvs = new Vector2[mWidth * mHeight];
        triangles = new int[(mWidth - 1) * (mHeight - 1) * 6];
    }
    public void AddTriangle(int a, int b, int c)
    {
        triangles[triIndex] = a;
        triangles[triIndex+1] = b;
        triangles[triIndex+2] = c;
        triIndex += 3;
    }

    public Mesh cMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.uv = uvs; //changed to uvs from uv because ub used by Mesh class
        mesh.RecalculateNormals();
        return mesh;
    }

}
