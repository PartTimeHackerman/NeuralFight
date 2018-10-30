using System.Collections.Generic;
using UnityEngine;

public class Block : AgentAction
{
    public float upperArmAng = -45f;
    public float lowerArmAng = 125f;

    void FixedUpdate()
    {
        ActionBlock();
    }

    private void ActionBlock()
    {
        float rotation = getRotation();
        float upperRot = Mathf.Clamp(-rotation - upperArmAng, upperArm.angularLimits[0][0], upperArm.angularLimits[0][1]);
        float lowerRot = lowerArmAng;
        upperArm.setConfigurableForceAndRot(strength, new Vector3(upperRot, 0f, 0f));
        lowerArm.setConfigurableForceAndRot(strength, new Vector3(lowerRot, 0f, 0f));
    }
}