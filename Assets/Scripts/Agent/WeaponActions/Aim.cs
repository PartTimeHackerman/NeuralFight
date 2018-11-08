using UnityEngine;

public class Aim : OneHandedAction
{
    public float upperArmAng = -20f;
    public float lowerArmAng = -70f;

    protected override void TakeAction()
    {
        float upRotation = getRotation();
        float upperRot = Mathf.Clamp(-upRotation - upperArmAng, upperArm.angularLimits[0][0], upperArm.angularLimits[0][1]);
        upperArm.setConfigurableForceAndRot(strength, new Vector3(upperRot, 0f, 0f));
        
        float lowRotation = getRotation(upperArm.transform, target);
        float lowerRot = Mathf.Clamp(lowRotation - lowerArmAng, lowerArm.angularLimits[0][0], lowerArm.angularLimits[0][1]);
        lowerArm.setConfigurableForceAndRot(strength, new Vector3(lowerRot, 0f, 0f));
        done = true;
    }
}