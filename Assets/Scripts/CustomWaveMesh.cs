using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomWaveMesh : MonoBehaviour
{
    [SerializeField] private int tileWidth = 200;
    [SerializeField] private int tileHeight = 200;
    private float waveArea;
    private MeshGenerator meshGenerator;
    private MeshFilter meshFilter;
    private Mesh waveMesh;

    public WaveOctave[] WaveLayers;

    public Transform playerTransform;


    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        GenerateMesh();
    }

    private void GenerateMesh()
    {
        if (waveMesh == null)
        {
            meshGenerator = new MeshGenerator();
            meshGenerator.GenerateMesh(tileWidth, tileHeight, 1);
            waveArea = tileWidth * tileHeight;
            waveMesh = meshGenerator.getMesh();

            meshFilter.sharedMesh = waveMesh;
            meshFilter.name = gameObject.name;
        }
    }

    private void Update() {

        calculateWaveVertex();
        
    }
    

    private void calculateWaveVertex()
    {
        var verts = waveMesh.vertices;
        float topLeftX = (tileWidth - 1) / -2f;
        float topLeftZ = (tileHeight - 1) / 2f;

        int index = 0;
        for (int x = 0; x < tileWidth; x++)
        {
            for (int z = 0; z < tileHeight; z++)
            {
                float y = 0f;

                // Apply Nosie to verticies based on waveLayers
                var waveNoise = Mathf.PerlinNoise((x * WaveLayers[0].waveScale.x) / tileWidth, (z * WaveLayers[0].waveScale.y) / tileHeight) * Mathf.PI * 2f;
                y += Mathf.Cos(waveNoise + WaveLayers[0].waveSpeed.magnitude * Time.time) * WaveLayers[0].waveHeight;

                verts[index] = new Vector3(x, y,z);
                index++;
            }
        }

        waveMesh.vertices = verts;
    }


// convert 2D coordinate to index
  private int coordToIndex(float x, float y)
  {
      return (int)(x * (tileWidth + 1) + y);
  }


// Calcualte mesh height at 3D position
  public float GetHeightAtPoint(Vector3 position)
  {
    Vector3 scale = new Vector3(1/ transform.lossyScale.x, 0, 1/transform.lossyScale.z);
    Vector3 lPosition = Vector3.Scale((position - transform.position), scale);

    Vector3 p1 = new Vector3(Mathf.Floor(lPosition.x), 0, Mathf.Floor(lPosition.z));
    Vector3 p2 = new Vector3(Mathf.Floor(lPosition.x), 0, Mathf.Ceil(lPosition.z));
    Vector3 p3 = new Vector3(Mathf.Ceil(lPosition.x), 0, Mathf.Floor(lPosition.z));
    Vector3 p4 = new Vector3(Mathf.Ceil(lPosition.x), 0, Mathf.Ceil(lPosition.z));

    p1.x = Mathf.Clamp(p1.x, 0, waveArea);
    p1.z = Mathf.Clamp(p1.z, 0, waveArea);
    p2.x = Mathf.Clamp(p2.x, 0, waveArea);
    p2.z = Mathf.Clamp(p2.z, 0, waveArea);
    p3.x = Mathf.Clamp(p3.x, 0, waveArea);
    p3.z = Mathf.Clamp(p3.z, 0, waveArea);
    p4.x = Mathf.Clamp(p4.x, 0, waveArea);
    p4.z = Mathf.Clamp(p4.z, 0, waveArea);

    float max = Mathf.Max(Vector3.Distance(p1, lPosition), Vector3.Distance(p2, lPosition), Vector3.Distance(p3, lPosition), Vector3.Distance(p4, lPosition) + Mathf.Epsilon);

    float dist = (max - Vector3.Distance(p1, lPosition)) 
               + (max - Vector3.Distance(p2, lPosition)) 
               + (max - Vector3.Distance(p3, lPosition)) 
               + (max - Vector3.Distance(p4, lPosition) + Mathf.Epsilon);

    float height = waveMesh.vertices[coordToIndex(p1.x, p1.z)].y * (max - Vector3.Distance(p1, lPosition))
               + waveMesh.vertices[coordToIndex(p2.x, p1.z)].y * (max - Vector3.Distance(p2, lPosition))
               + waveMesh.vertices[coordToIndex(p3.x, p1.z)].y * (max - Vector3.Distance(p3, lPosition))
               + waveMesh.vertices[coordToIndex(p4.x, p1.z)].y * (max - Vector3.Distance(p4, lPosition));

   
    return (height * transform.lossyScale.y / dist) + transform.position.y;
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
