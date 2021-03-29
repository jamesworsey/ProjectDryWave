using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator
{
    public int meshWidth, meshHeight;
    public Vector3[] verticies;
    public int[] triangles;
    public Vector2[] uvs;

    public int vertPerLine;

    int triangleIndex;

    public void GenerateMesh(int meshWidth, int meshHeight, int resolution, bool useOffsetCorrection = false)
    {
        this.meshWidth = meshWidth;
        this.meshHeight = meshHeight;
        float topLeftX = (meshWidth - 1) / -2f;
        float topLeftZ = (meshHeight - 1) / 2f;
        InitMeshData();

        vertPerLine = (meshWidth - 1) / resolution + 1;

        int index = 0;
        for(int y = 0; y < meshHeight; y++)
        {
            for(int x = 0; x < meshWidth; x++)
            {
                if(useOffsetCorrection) verticies[index] = new Vector3(topLeftX + x, 0, topLeftZ - y);
                else verticies[index] = new Vector3(x, 0, y);
                uvs[index] = new Vector2(x / (float)meshWidth, y / (float)meshHeight);

                if(x < meshWidth - 1 && y < meshHeight - 1)
                {
                    AddTriangle(index, index + vertPerLine + 1, index + vertPerLine);
                    AddTriangle(index + vertPerLine + 1, index, index + 1);
                }
                index++;
            }
        }
    }

    public Mesh getMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }

    private void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    private void InitMeshData()
    {
        verticies = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }
}