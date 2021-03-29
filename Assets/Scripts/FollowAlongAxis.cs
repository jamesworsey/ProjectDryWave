using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAlongAxis : MonoBehaviour
{
    public bool x, y, z;
    public Transform target;

    private void Update()
    {
        Vector3 pos = target.position;
        Vector3 targetVector = transform.position;

        targetVector.x = (x) ? pos.x : targetVector.x;
        targetVector.y = (y) ? pos.y : targetVector.y;
        targetVector.z = (z) ? pos.z : targetVector.z;

        transform.position = targetVector;
    }
}
