﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtUpRot : MonoBehaviour
{


    public float up;
    public int dir;
    public float force;
    public Rigidbody rigidbody;

	void Start ()
	{
	    rigidbody = GetComponent<Rigidbody>();
	}
	
	
	void FixedUpdate()
	{
	    up = Mathf.Abs(((rigidbody.transform.up.y + 1) / 2)-1)+.01f;
	    if (rigidbody.transform.rotation.eulerAngles.z > 180)
	        dir = 1;
	    else
	        dir = -1;

	    float angVel = Mathf.Abs(rigidbody.angularVelocity.z);
	    float newForce = angVel == 0 ? force : force / angVel;
	    newForce *= rigidbody.mass;
	    if (up > 0.0105f)
	           rigidbody.AddTorque(0f, 0f, up * newForce * dir, ForceMode.Force);
	     
        //rigidbody.transform.Rotate(up * force * dir, 0f, 0f);
    }
}
