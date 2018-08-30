using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotVelTest : MonoBehaviour
{
    private Rigidbody rb;

    public Vector3 vel;
    public Vector3 rotVel;

	void Start ()
	{
	    rb = GetComponent<Rigidbody>();

        rb.AddForce(5f, 5f, 0f, ForceMode.Impulse);
        rb.AddTorque(0f,0f, 10f, ForceMode.Impulse);
	}
	
	void Update ()
	{
	    vel = rb.velocity;
	    rotVel = rb.angularVelocity;
	}
}
