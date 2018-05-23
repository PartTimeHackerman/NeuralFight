using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointDebug : MonoBehaviour
{


    public ConfigurableJoint joint;
    public Vector3 jointRotation;
    public Vector3 jointRotationGlobal;
    public Vector3 jointAngularVel;
    public Vector3 jointVel;
    public Vector3 posLoc;
    public Vector3 posDoc = new Vector3(0 ,0 ,0);

    public Quaternion jointQRot;

    void Reset()
    {
        joint = GetComponent<ConfigurableJoint>();
        joint.GetComponent<Rigidbody>().sleepThreshold = 0;
    }
    
	void Update () {
	    jointRotation = joint.transform.localRotation.eulerAngles;
	    jointRotationGlobal = joint.transform.rotation.eulerAngles;
	    jointAngularVel = joint.GetComponent<Rigidbody>().angularVelocity;
	    jointVel = joint.GetComponent<Rigidbody>().velocity;
	    posLoc = joint.transform.localPosition;
	    jointQRot = joint.transform.localRotation;

        joint.targetRotation = Quaternion.Euler(posDoc);
    }
}
