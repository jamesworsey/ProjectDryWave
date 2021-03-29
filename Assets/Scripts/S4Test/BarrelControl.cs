using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelControl : MonoBehaviour
{
    private Vector3 startPos;

    private CharacterController controller;
    [Header("Input Variables")]
    public float speed = 10f;
    public float gravity = 9.8f;
    public float rotationSpeed = 10f;
    float turnVelocity = 0.0f;
    public float turnAccelerationRate = 2.0f;
    public float turnDecelRate = 0.5f;
    Vector3 rotationVector = Vector3.zero;
    float currentTurnAngle = 0;
    float turnInputAxis = 0;

    [Header("Board Angle")]
    public float heading = 0f;
    public float headingBack = 0.0f;
    [SerializeField] Transform backTransform = null;
    [SerializeField] Transform forwardTransform = null;
    [SerializeField] Transform waveLip = null;
    Quaternion waveNormal;

    private bool hitEdgeTrigger = false;

    public Vector3 waveDragVector = Vector3.zero;
    private Vector3 waveLipVector = Vector3.zero;
    private Vector3 forwardVector = Vector3.zero;
    public float forwardSpeed = 3.0f;

    private bool isOnSlope = false;

    public float waveSpeed = 2.0f;
    public float lipSpeed = 0.5f;
    float horizontalScaleFactor = 0f;

    public float maxVelocity = 5.0f;
    private float currentVelocity = 0.0f;

    [Header("Slope Velocity Variables")]
    public float minSlopeAccelerationAngle = -45.0f;
    public float maxSlopeAccelerationAngle = 45.0f;
    public float slopeVelocityIncreaseRate = 0.1f;
    public float slopeVelocityDecreaseRate = 0.5f;

    [Header("Face Velocity Variables")]
    public float minFaceAccelerationAngle = 0;
    public float maxFaceAccelerationAngle = -45.0f;
    public float faceVelocityIncreaseRate = 0.1f;
    public float faceVelocityDecreaseRate = 0.5f;

    public GameObject Board = null;

    public AnimationCurve slopeCurve;
    public AnimationCurve turnDecel;
    public AnimationCurve climbRate;

    [SerializeField] private Transform standingpos;
    [SerializeField] private Transform controllerCamera;

    public void setSpeed(float speed)
    {
        currentVelocity = speed;
    }

    private void CalculateBoardHeading()
    {
        heading = calculateBoardAngle(transform);
        headingBack = calculateBoardAngle(backTransform);

    }

    private void FindWaveType()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            if (hit.transform.CompareTag("WaveSlope")){
                isOnSlope = true;
            }
            else
            {
                isOnSlope = false;
            }
            waveNormal = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
    }

    private float calculateBoardAngle(Transform pos)
    {
        return Mathf.Atan2(pos.right.z, pos.right.x) * Mathf.Rad2Deg;
    }

    private Vector3 calculateBoardVelocity(Vector3 moveVector)
    {
        float absTurnInputAxis = Mathf.Abs(turnInputAxis);
        waveLipVector = waveLip.position - transform.position;

        // If on Wave Slope
        if (isOnSlope)
        {
            if(heading > 0)
            {
               float forwardsInverseLerp = Mathf.InverseLerp(0, 90, heading);
               float curveEval = slopeCurve.Evaluate(forwardsInverseLerp);
               currentVelocity += (slopeVelocityIncreaseRate * curveEval) * Time.deltaTime;
            }
            else
            {
                currentVelocity -= slopeVelocityDecreaseRate * Time.deltaTime;
            }
        }

        // If on Wave Face
        else
        {
            currentVelocity -= faceVelocityDecreaseRate * Time.deltaTime;
        }

        // If board is pointing up
        if(heading < 0)
        {
            float backwardsInverseLerp = Mathf.InverseLerp(0, -90, heading);
            backwardsInverseLerp = climbRate.Evaluate(backwardsInverseLerp);
            horizontalScaleFactor += backwardsInverseLerp * Time.deltaTime;
        }
        else
        {
            horizontalScaleFactor -= 1 * Time.deltaTime;
        }


        if(absTurnInputAxis > 0)
        {
            currentVelocity -= 10 * Time.deltaTime;
        }

        currentVelocity = Mathf.Clamp(currentVelocity, -15, maxVelocity);
        horizontalScaleFactor = Mathf.Clamp(horizontalScaleFactor, 0, 1);

        Vector3 currentVector = moveVector;
        currentVector = (forwardVector * currentVelocity);
        currentVector += (waveLipVector * horizontalScaleFactor);
        moveVector += currentVector * Time.deltaTime;

        return moveVector;
    }

    public void relativeRigidbodyTurn(Vector3 direction)
    {
        controller.transform.Rotate(direction);
    }

    private void CalculateBoardTurn()
    {
        turnInputAxis = Input.GetAxis("Horizontal");

        float percentSpeed = Mathf.InverseLerp(-10, maxVelocity, currentVelocity);
        percentSpeed = turnDecel.Evaluate(percentSpeed);

        if (isOnSlope) percentSpeed *= 1.2f;

        Vector3 turnVector = Vector3.zero;
        turnVector.y = turnInputAxis * (turnAccelerationRate * percentSpeed);

        relativeRigidbodyTurn(turnVector * Time.deltaTime);
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        startPos = transform.position;
    }

    private float CalculateGravity()
    {
        float h = 0.0f;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
        {
            if (Mathf.Abs(transform.position.y - hit.transform.position.y) > 0.1f)
            {
                h -= gravity;
            }
        }
        return h;
    }

    private void Update()
    {
        Vector3 moveDir = Vector3.zero;
        forwardVector = forwardTransform.forward;

        CalculateBoardHeading();
        FindWaveType();
        CalculateBoardTurn();
        float h = CalculateGravity();

        moveDir = new Vector3(0, h, 0).normalized;
        moveDir = calculateBoardVelocity(moveDir);

        if (!hitEdgeTrigger)
            controller.Move(moveDir);
        else
        {
            transform.position = startPos;
            hitEdgeTrigger = false;
        }

        controllerCamera.position = Vector3.Lerp(controllerCamera.position, standingpos.position, 0.1f);
    }

 

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("EdgeTrigger"))
        {
            hitEdgeTrigger = true;
        }

        if (other.CompareTag("SlowDown")){
            setSpeed(-5);
        }
    }
}
