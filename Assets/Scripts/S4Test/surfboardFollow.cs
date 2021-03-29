using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class surfboardFollow : MonoBehaviour
{
    public Transform playerTr;

    public float minWaveHeight = 0;
    public float maxWaveHeight = 6;
    Quaternion slopeAngle = Quaternion.identity;
    Quaternion faceAngle = Quaternion.identity;

    Quaternion lastTarget = Quaternion.identity;

    private void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            slopeAngle = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
    }

    private void Update()
    {
       
        float h = transform.position.y;
        float lerpPercent = Mathf.InverseLerp(minWaveHeight, maxWaveHeight, h);
        transform.rotation = Quaternion.Lerp(faceAngle, slopeAngle, lerpPercent);
           
   
        Vector3 targetPosition = new Vector3(playerTr.position.x, playerTr.position.y - 0.5f, playerTr.position.z);
        transform.position = targetPosition;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, playerTr.eulerAngles.y, transform.eulerAngles.z);
    }
}
