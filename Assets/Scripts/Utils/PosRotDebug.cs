using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosRotDebug : MonoBehaviour
{

    public Vector3 globalPos;
    public Vector3 globalRot;
    public Vector3 localPos;
    public Vector3 localRot;
	
	void Update ()
	{
	    get();
	}

    void Reset()
    {
        get();
    }

    public void get()
    {
        globalPos = transform.position;
        globalRot = transform.rotation.eulerAngles;

        localPos = transform.localPosition;
        localRot = transform.localRotation.eulerAngles;
    }
}
