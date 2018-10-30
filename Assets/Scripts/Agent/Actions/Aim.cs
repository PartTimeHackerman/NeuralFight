using UnityEngine;

public class Aim : AgentAction
{
    public float upperArmAng = -90f;

    void FixedUpdate()
    {
        ActionBlock();
    }

    private void ActionBlock()
    {
        float rotation = getRotation();
        float upperRot = Mathf.Clamp(-rotation - upperArmAng, upperArm.angularLimits[0][0], upperArm.angularLimits[0][1]);
        upperArm.setConfigurableForceAndRot(strength, new Vector3(upperRot, 0f, 0f));
        lowerArm.setConfigurableForceAndRot(strength, new Vector3(0f, 0f, 0f));
    }
}