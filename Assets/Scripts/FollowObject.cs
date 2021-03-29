using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public GameObject Object;
    public Vector3 offset;
    public float interpolateTime = 0.1f;
    private Vector3 targetPosition;

    private Vector3 velocity = Vector3.zero;

    
    private void Update() {
        targetPosition = Object.transform.position + offset;
       transform.position =  Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, interpolateTime);
    }
}
