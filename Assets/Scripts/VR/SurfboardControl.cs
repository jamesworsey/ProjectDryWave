using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SurfboardControl : MonoBehaviour
{
    private GameObject surfboard;
    private CharacterController controller;
    private float paddleVector;
    private bool isTakingoff = false;

    [SerializeField] private float rotationSpeed = 0.01f;
    [SerializeField] private AnimationCurve turnDecelerationCurve;
    [SerializeField] private float paddleForce = 1f;
    [SerializeField] private float paddleDecelerationRate = 0.1f;
    [SerializeField] private float maxBoardSpeed = 10f;
    [SerializeField] private Transform takeoffTarget;
    [SerializeField] private float takeoffEarlyTime;
    [SerializeField] private float takeoffLateTime;

    [SerializeField] private GameObject customWaveMesh;
    private CustomWaveMesh waveMesh;

    private void Start()
    {
        surfboard = GameObject.FindGameObjectWithTag("Board");
        controller = GetComponent<CharacterController>();
        waveMesh = customWaveMesh.GetComponent<CustomWaveMesh>();
    }


    private void turn(float direction)
    {
        Vector3 turnVector = Vector3.zero;

        float absDirection = Mathf.Abs(direction);
        float turnDecel = turnDecelerationCurve.Evaluate(absDirection);
        turnVector.y = (direction * turnDecel);

        controller.transform.Rotate(turnVector * rotationSpeed);
    }


    private void paddle(float direction)
    {
        Vector3 boardForward = -surfboard.transform.right;
        controller.Move(boardForward * direction * Time.deltaTime);
    }

    public int takeoff(bool standing)
    {
        isTakingoff = true;
        transform.position = Vector3.Lerp(transform.position, takeoffTarget.position, 0.01f);
        float distance = Vector3.Distance(transform.position, takeoffTarget.position);

        if (distance < takeoffLateTime)
        {
            return 1; // Failed to stand in time
        }

        if (standing && distance < takeoffEarlyTime)
        {
            return 0; // Successfully stood up
        }

        if(standing && distance > takeoffEarlyTime)
        {
            return 2; // Early Standup
        }

        return 3;
    }


    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");

        paddleVector -= paddleDecelerationRate * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            paddleVector += paddleForce;
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            paddleVector += paddleForce;
        }

        paddleVector = Mathf.Clamp(paddleVector, 0, maxBoardSpeed);

        if (!isTakingoff)
        {
            turn(horizontal);
            paddle(paddleVector);

            float height = waveMesh.GetHeightAtPoint(transform.position);
            transform.position = new Vector3(transform.position.x, (height + 1.5f), transform.position.z);
        }
    }
}
