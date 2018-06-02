using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpToLimit : MonoBehaviour
{

    public float upVel = 10f;
    public float limit = 1.6f;
    public float vel;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //limit = rb.position.y;
    }

    void FixedUpdate()
    {
        if (rb.position.y < limit)
        {

            vel = upVel * Mathf.Abs((rb.position.y / limit) - 1);
            rb.velocity = new Vector3(rb.velocity.x, vel, rb.velocity.z);
        }

    }
}
