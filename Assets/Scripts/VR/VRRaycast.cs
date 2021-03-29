using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

// This class allows interaction with the scene through raycasting instead of using collision detection.
// The raycast is currently visualized with a green line in the scene
// The information on what object was hit is stored in the hit variable of initRaycast and its position in the point variable
public class VRRaycast : MonoBehaviour
{
    // Controller Inputs
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean raycastAction;

    private Vector3 point;
    public GameObject rayObject;

    // Set the position and rotation of the ray object to match the controller
    private void showRay(RaycastHit hit)
    {
        rayObject.transform.position = Vector3.Lerp(controllerPose.transform.position, point, 0.5f);
        rayObject.transform.LookAt(point);
        rayObject.transform.localScale = new Vector3(rayObject.transform.localScale.x, rayObject.transform.localScale.y, hit.distance);
    }


    // Initialize a ray originating at the controllers position
    private void initRaycast()
    {
        RaycastHit hit;
        if(Physics.Raycast(controllerPose.transform.position, transform.forward, out hit))
        {
            point = hit.point;
            showRay(hit);
            Debug.Log("Hit " + hit.transform.gameObject + " with " + handType + " controller at " + point);
        }
    }


    void Update()
    {
        if(raycastAction.GetState(handType))
        {
            rayObject.SetActive(true);
            initRaycast();
        }else
        {
            rayObject.SetActive(false);
        }
    }
}
