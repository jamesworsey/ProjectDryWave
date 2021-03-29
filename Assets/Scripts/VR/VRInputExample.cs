using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR; // Steam SDK


// This class contains the basics for VR controller input and interaction.
// Allows for simple grab and release of physics objects.
// This class should be attached to each controller.
public class VRInputExample : MonoBehaviour
{

    // handtype contains a reference to the controller the input was detected on. (Left or right)
    public SteamVR_Input_Sources handType;

    // References to inputs set in the Unity editor
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Behaviour_Pose controllerPose;

    private GameObject collision;
    private GameObject heldObject;


    // Adds a fixed joint component to the controller
    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }


    // Adds the object currently in contact with the controller to a fixed joint and sets it to the heldObject variable
    public void GrabObject()
    {
        if(collision){
            heldObject = collision;
            collision = null;

            FixedJoint joint = AddFixedJoint();
            joint.connectedBody = heldObject.GetComponent<Rigidbody>();
        }
    }


    // Removes the object currently being held and applies a rigid body force equal to the controllers speed. 
    public void ReleaseObject()
    {
        if(heldObject && GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());

            heldObject.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
            heldObject.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();
        }

        heldObject = null;
    }


    // Checks if the currently colliding object can be held
    public void setCollision(Collider other)
    {
        if(collision || !other.GetComponent<Rigidbody>())
        {
            return;
        }

        collision = other.gameObject;
    }


    // Update is called on each Monobehaviour class once per frame
    void Update()
    {
        if(grabAction.GetLastStateDown(handType))
        {
            if(collision)
            {
                Debug.Log("Grabbed with " + handType);
                GrabObject();
            }
        }

        if(grabAction.GetLastStateUp(handType))
        {
            if(heldObject)
            {
                Debug.Log("Released with " + handType);
                ReleaseObject();
            }
        }
    }


    // Overrides from Monobehaviour, called when the attached collider component interacts with another collider in the scene
    private void OnTriggerEnter(Collider other)
    {
        setCollision(other);
    }

    private void OnTriggerStay(Collider other)
    {
        setCollision(other);
    }

    private void OnTriggerExit(Collider other)
    {
        collision = null;
    }
}
