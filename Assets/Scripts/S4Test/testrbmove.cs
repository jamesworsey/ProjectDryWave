using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testrbmove : MonoBehaviour
{
    public float h;
    public float v;

    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
         h = Input.GetAxis("Horizontal");
         v = Input.GetAxis("Vertical");

        Vector3 moveVec = new Vector3(h, 0, v).normalized;

        moveVec *= 10f;

        rb.AddForce(moveVec);
    }
}
