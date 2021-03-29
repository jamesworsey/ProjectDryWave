using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelControls : MonoBehaviour
{
    [SerializeField] ObjectBuoyancy objectBuoyancy = null;
    [SerializeField]bool isOnBarrel = false;

    private GameObject parentObject;
    private Rigidbody _rigidbody;
    private float _heading = 0.0f;


    private void Start()
    {
        parentObject = GameObject.FindGameObjectWithTag("GameController");
        _rigidbody = GetComponent<Rigidbody>();
    }


    public void onWaveCollision()
    {
        objectBuoyancy.enabled = false;
        _rigidbody.isKinematic = true;
        _heading = calculateBoardAngle(transform);

        if (_heading >= 0) isOnBarrel = true;
        else
            isOnBarrel = false;
    }


    private float calculateBoardAngle(Transform pos)
    {
        return Mathf.Atan2(pos.right.z, pos.right.x) * Mathf.Rad2Deg;
    }

}
