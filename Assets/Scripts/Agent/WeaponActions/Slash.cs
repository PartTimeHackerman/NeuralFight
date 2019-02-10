using System.Collections.Generic;
using UnityEngine;

public class Slash : OneHandedAction
{
    public float upperArmReadyAng = -145f;
    public float lowerArmReadyAng = 90f;
    public float upperArmSlashAng = 0f;
    public float lowerArmSlashAng = 10f;

    public int readyFrames = 10;
    public int upperFrames = 10;
    public int lowerFrames = 11;
    public int maxFrames = 25;
    public int framesElapsed = 0;
    
    
    protected override void TakeAction()
    {
        framesElapsed++;
        if (framesElapsed < readyFrames)
        {
            ActionGetReady();
        }

        if (framesElapsed > upperFrames)
        {
            ActionSlashUpper();
        }
        if (framesElapsed > lowerFrames)
        {
            ActionSlashLower();
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
        float upperRot = Mathf.Clamp(-rotation - upperArmReadyAng, upperArm.angularLimits[0][0], upperArm.angularLimits[0][1]);
        float lowerRot = lowerArmReadyAng;
        upperArm.SetConfigurableForceAndRot(strength, new Vector3(upperRot, 0f, 0f));
        lowerArm.SetConfigurableForceAndRot(strength, new Vector3(lowerRot, 0f, 0f));
    }
    
    private void ActionSlashUpper()
    {
        float rotation = getRotation();
        float upperRot = Mathf.Clamp(-rotation - upperArmSlashAng, upperArm.angularLimits[0][0], upperArm.angularLimits[0][1]);
        upperArm.SetConfigurableForceAndRot(strength, new Vector3(upperRot, 0f, 0f));
    }
    
    private void ActionSlashLower()
    {
        lowerArm.SetConfigurableForceAndRot(strength, new Vector3(lowerArmSlashAng, 0f, 0f));
    }

}