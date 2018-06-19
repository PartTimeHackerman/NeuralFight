using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointDebug : MonoBehaviour
{

    
    public Vector3 jointRotation;
    public Vector3 jointRotationGlobal;
    public Vector3 jointAngularVel;
    public Vector3 jointVel;
    public Vector3 posLoc;
    public Vector3 posDoc = new Vector3(0 ,0 ,0);

    public Quaternion jointQRot;

    void Reset()
    {
        GetComponent<Rigidbody>().sleepThreshold = 0;
    }
    
	void Update () {
	    jointRotation = transform.localRotation.eulerAngles;
	    jointRotationGlobal = transform.rotation.eulerAngles;
	    jointAngularVel = GetComponent<Rigidbody>().angularVelocity;
	    jointVel = GetComponent<Rigidbody>().velocity;
	    posLoc = transform.localPosition;
	    jointQRot = transform.localRotation;
    }
}
