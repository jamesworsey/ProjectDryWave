using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerManager : MonoBehaviour
{
    private enum States
    {
        Paddle, Takeoff, Barrel, Falloff
    }
    private States currentState = States.Paddle;

    SurfboardControl boardControl;
    BarrelControl barrelControl;
    [SerializeField] GameObject controllerCamera = null;
    [SerializeField] GameObject surfboard;
    [SerializeField] Transform layingPosition = null;
    [SerializeField] Transform standingPosition = null;
    [SerializeField] Transform floorHeight = null;
    [SerializeField] GameObject normalPostProc;
    [SerializeField] GameObject grayPostProc;

    private bool isStanding = false;
    private BarrelWaveManager barrelEffect;

    private void Start()
    {
        boardControl = GetComponent<SurfboardControl>();
        barrelControl = GetComponent<BarrelControl>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(0);

        switch (currentState)
        {
            case States.Paddle:
                Paddle();
                break;
            case States.Takeoff:
                Takeoff();
                break;
            case States.Barrel:
                break;
            case States.Falloff:
                break;
            default:
                Debug.LogError("Invalid Movement State", this);
                break;
        }
    }


    private void Paddle()
    {
        Vector3 cameraPosition = controllerCamera.transform.position;
        Vector3 targetPosition;

        if (Input.GetKeyDown(KeyCode.F)) isStanding = !isStanding;

        if (isStanding)
        {
            targetPosition = standingPosition.position;
        }
        else
        {
            targetPosition = layingPosition.position;
        }

        controllerCamera.transform.position = Vector3.Lerp(cameraPosition, targetPosition, 0.1f);
    }


    private void Takeoff()
    {
        Paddle();
        int takeoffResult = boardControl.takeoff(isStanding);

        // success
        if (takeoffResult == 0)
        {
            boardControl.enabled = false;
            barrelControl.enabled = true;
            currentState = States.Barrel;
            barrelControl.setSpeed(barrelControl.maxVelocity);
        }
        else if (takeoffResult == 1)
        {
            Falloff();

        }else if(takeoffResult == 2)
        {
            boardControl.enabled = false;
            barrelControl.enabled = true;
            currentState = States.Barrel;
            barrelControl.setSpeed(barrelControl.maxVelocity/2);
            transform.position = new Vector3(transform.position.x, floorHeight.position.y, transform.position.z);
        }
    }


    public void Falloff()
    {
        currentState = States.Falloff;
        barrelControl.enabled = false;
        normalPostProc.SetActive(false);
        grayPostProc.SetActive(true);

        Rigidbody surfboardRB = surfboard.GetComponent<Rigidbody>();
        surfboardFollow sbfollow = surfboard.GetComponent<surfboardFollow>();

        surfboardRB.isKinematic = false;
        surfboardRB.useGravity = true;
        sbfollow.enabled = false;

        Vector3 expPosition = new Vector3(Random.Range(-1, 5), Random.Range(3, 5), Random.Range(-1, 5));
        Vector3 expRotation = new Vector3(Random.Range(1, 20), Random.Range(1, 20), Random.Range(1, 20));
        float expForce = Random.Range(100, 1000);

        Vector3 position = surfboard.transform.position - expPosition;

        surfboardRB.AddExplosionForce(expForce, position, 100);
        surfboardRB.AddRelativeTorque(expRotation);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EdgeTrigger"))
        {
            barrelEffect = other.GetComponentInParent<BarrelWaveManager>();
            if (currentState == States.Paddle)
            {
                currentState = States.Takeoff;
                other.GetComponentInParent<FollowObject>().enabled = false;
            }
        }

        if(other.CompareTag("FallTrigger"))
        {
            if(barrelEffect.getWaveTimer() > barrelEffect.getBreakTime())
            {
                Falloff();
            }
        }

        if(other.CompareTag("WaveLip"))
        {

        }
    }

}
