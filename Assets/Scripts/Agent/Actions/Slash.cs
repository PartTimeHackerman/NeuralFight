using System.Collections.Generic;
using UnityEngine;

public class Slash : AgentAction
{
    public float upperArmReadyAng = -145f;
    public float lowerArmReadyAng = 150f;
    public float upperArmSlashAng = -30f;
    public float lowerArmSlashAng = 20f;

    public int readyFrames = 10;
    public int upperFrames = 10;
    public int lowerFrames = 12;
    public int maxFrames = 30;
    public int framesElapsed = 0;
    
    public bool push = false;
    public bool done = true;
    void Start()
    {
        BodyParts = GetComponent<BodyParts>();

        foreach (KeyValuePair<string,JointInfo> keyValuePair in BodyParts.namedJoints)
        {
            if (keyValuePair.Key.Contains("upper")) upperArm = keyValuePair.Value;
            if (keyValuePair.Key.Contains("lower")) lowerArm = keyValuePair.Value;
        }

        root = BodyParts.root.transform;
    }

    void FixedUpdate()
    {
        ActionSlash();
    }

    private void ActionSlash()
    {
        if (push)
        {
            done = false;
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
                //push = false;
                done = true;
            }
        }
    }

    private void ActionGetReady()
    {
        float rotation = getRotation();
        float upperRot = Mathf.Clamp(-rotation - upperArmReadyAng, upperArm.angularLimits[0][0], upperArm.angularLimits[0][1]);
        float lowerRot = lowerArmReadyAng;
        upperArm.setConfigurableForceAndRot(strength, new Vector3(upperRot, 0f, 0f));
        lowerArm.setConfigurableForceAndRot(strength, new Vector3(lowerRot, 0f, 0f));
    }
    
    private void ActionSlashUpper()
    {
        float rotation = getRotation();
        float upperRot = Mathf.Clamp(-rotation - upperArmSlashAng, upperArm.angularLimits[0][0], upperArm.angularLimits[0][1]);
        upperArm.setConfigurableForceAndRot(strength, new Vector3(upperRot, 0f, 0f));
    }
    
    private void ActionSlashLower()
    {
        lowerArm.setConfigurableForceAndRot(strength, new Vector3(lowerArmSlashAng, 0f, 0f));
    }

}