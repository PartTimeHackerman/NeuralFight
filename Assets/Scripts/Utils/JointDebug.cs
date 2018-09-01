using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointDebug : MonoBehaviour
{

    public Rigidbody root;
    public float threshold = 0.1f;
    public Vector3 jointRotation;
    public Vector3 jointRotationGlobal;
    public Vector3 jointAngularVel;
    public Vector3 jointVel;
    public Vector3 posLoc;
    public Vector3 posLocRelativeToRoot;
    public Vector3 posWrd;
    public Vector3 rotLocRelativeToRoot;

    public Quaternion jointQRot;
    public Vector3 cross;
    public float up;
    public float vel = 100f;
    void Reset()
    {
        GetComponent<Rigidbody>().sleepThreshold = 0;
    }
    
	void FixedUpdate () {
	    jointRotation = setThreshold(transform.localRotation.eulerAngles);
	    jointRotationGlobal = setThreshold(transform.rotation.eulerAngles);
	    jointAngularVel = setThreshold(GetComponent<Rigidbody>().angularVelocity);
	    jointVel = setThreshold(GetComponent<Rigidbody>().velocity);
	    posLoc = transform.localPosition;
	    posLocRelativeToRoot = root.transform.InverseTransformPoint(transform.position);
	    posWrd = transform.position;
	    rotLocRelativeToRoot = (Quaternion.Inverse(root.rotation) * transform.rotation).eulerAngles;
	    jointQRot = transform.localRotation;

	    Vector3 dir = root.transform.position - transform.position;
	    dir.Normalize();
	    Vector3 side = Vector3.Cross(dir, -transform.right);
	    cross = Vector3.Cross(dir, side).normalized;
	    if (transform.position.x > root.transform.position.x)
	        cross.x *= -1;
	    cross.y = Mathf.Abs(cross.y);
	    up = Mathf.Abs(((transform.up.y + 1) / 2) - 1);
	    cross.x *= up;
	    cross.y *= up;
        GetComponent<Rigidbody>().AddForce(cross.x * vel, cross.y * vel , 0);

    }

    Vector3 setThreshold(Vector3 v)
    {
        for (int i = 0; i < 3; i++)
            v[i] = (v[i] > 0 && v[i] <= threshold) || v[i] < 0 && v[i] >= -threshold ? 0 : v[i];
        return v;
    }
}
