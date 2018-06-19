using System.Collections;
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
	    up = Mathf.Abs(((rigidbody.transform.up.y + 1) / 2)-1);
	    if (rigidbody.transform.rotation.eulerAngles.x > 180)
	        dir = 1;
	    else
	        dir = -1;
        
        rigidbody.AddTorque(up * force * dir, 0f, 0f, ForceMode.Force);
        //rigidbody.transform.Rotate(up * force * dir, 0f, 0f);
    }
}
