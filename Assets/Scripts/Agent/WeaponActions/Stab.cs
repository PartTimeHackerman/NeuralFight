using System.Collections.Generic;
using UnityEngine;

public class Stab : OneHandedAction
{
    public float upperArmAng = 45f;
    public float lowerArmAng = 150f;

    public int readyFrames = 10;
    public int upperStabFrames = 10;
    public int lowerStabFrames = 12;
    public int maxFrames = 30;
    public int framesElapsed = 0;
    
    protected override void TakeAction()
    {
            framesElapsed++;
            if (framesElapsed < readyFrames)
            {
                ActionGetReady();
            }

            if (framesElapsed > upperStabFrames)
            {
                ActionStabUpper();
            }
            if (framesElapsed > lowerStabFrames)
            {
                ActionStabLower();
            }

            if ( framesElapsed > maxFrames)
            {
                framesElapsed = 0;
                done = true;
            }
    }

    private void ActionGetReady()
    {
        float rotation = getRotation();
        float upperRot = Mathf.Clamp(-rotation - upperArmAng, upperArm.angularLimits[0][0], upperArm.angularLimits[0][1]);
        float lowerRot = lowerArmAng;
        upperArm.setConfigurableForceAndRot(strength, new Vector3(upperRot, 0f, 0f));
        lowerArm.setConfigurableForceAndRot(strength, new Vector3(lowerRot, 0f, 0f));
    }
    
    private void ActionStabUpper()
    {
        float rotation = getRotation();
        float upperRot = Mathf.Clamp(-rotation + 90f, upperArm.angularLimits[0][0], upperArm.angularLimits[0][1]);
        upperArm.setConfigurableForceAndRot(strength, new Vector3(upperRot, 0f, 0f));
    }
    
    private void ActionStabLower()
    {
        lowerArm.setConfigurableForceAndRot(strength, new Vector3(0f, 0f, 0f));
    }

}