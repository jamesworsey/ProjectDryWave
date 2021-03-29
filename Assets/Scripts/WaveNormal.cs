using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveNormal : MonoBehaviour
{    
    [SerializeField] GameObject waveTile = null;
    public Mesh waveTileMesh;
    public Vector3[] waveTileVertices;

    public WaveOctave[] waveParameters;

    private int findIndex(int x, int y, int dim)
    {
        return x * (dim + 1) + y;
    }

    private void Start() {
        waveTileMesh = waveTile.GetComponent<MeshFilter>().mesh;
        waveTileVertices = waveTileMesh.vertices;
    }

    private void FixedUpdate() {
        waveTileVertices = waveTileMesh.vertices;

        int width = 61;

        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < width; z++)
            {
                float vertY = 0.0f;
                float waveNoise = Mathf.PerlinNoise((x * waveParameters[0].waveScale.x) / width, (z * waveParameters[0].waveScale.y) / width) * Mathf.PI * 2f;
                vertY += Mathf.Cos(waveNoise + waveParameters[0].waveSpeed.magnitude * Time.time) * waveParameters[0].waveHeight;

                try{
                    waveTileVertices[findIndex(x, z, width)] = new Vector3(x, vertY, z);
                }catch(IndexOutOfRangeException e)
                {
                    Debug.Log(e);
                }
            }
             waveTileMesh.vertices = waveTileVertices;
        }       
       
    }
    
    [Serializable]
    public struct WaveOctave
    {
        public Vector2 waveSpeed;
        public Vector2 waveScale;
        public float waveHeight;
        public bool alternate;
    }

}
