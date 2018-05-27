using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPosRotVel : MonoBehaviour
{

    public Rigidbody rigid1;
    public Rigidbody rigid2;
    public int frameSkip = 10;
    private int frame = 0;

	void Update () {
	    if (frame == frameSkip)
	    {
	        frame = 0;
            ShowInfo(rigid1);
            ShowInfo(rigid2);

	    }

	    frame++;
	}

    private void ShowInfo(Rigidbody rigidbody)
    {
        Debug.Log(rigidbody.name+ " Pos: "+ rigidbody.transform.position + "Rot: " + rigidbody.transform.rotation.eulerAngles + "Vel: " + rigidbody.velocity + "RotVel: " + rigidbody.angularVelocity);
    }
}
