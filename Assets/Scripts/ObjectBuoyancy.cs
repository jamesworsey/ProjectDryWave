using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//
// Basic Buoyancy calculation script. 
// As the height of the waves is currently calcuated in the vertex shader this script can't take
// wave height into account. vertex heigh of waves should be calculated separately from shader in future. 
//
public class ObjectBuoyancy : MonoBehaviour
{

    [SerializeField] CustomWaveMesh wave = null;

    private Vector3 currentPosition;
    public bool floating;

    private void Update()
    {
        if (floating)
        {
            currentPosition = transform.position;
            currentPosition.y = wave.GetHeightAtPoint(currentPosition);
            transform.position = currentPosition;
        }
    }

}

