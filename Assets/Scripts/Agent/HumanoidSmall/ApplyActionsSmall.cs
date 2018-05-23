using System.Collections.Generic;
using UnityEngine;

public class ApplyActionsSmall : MonoBehaviour
{
    private int actionSpace;

    private BodyParts bodyParts;
    private List<ConfigurableJoint> joints;
    private Observations observations;

    private void Start()
    {
        bodyParts = GetComponent<BodyParts>();
        observations = GetComponent<Observations>();
        joints = bodyParts.getJoints();
        actionSpace = 22;
    }

    private void Update()
    {
    }

    public void applyActions(List<float> actions)
    {
        var actionIdx = 0;
        var size = actions.Count;

        if (size != actionSpace)
        {
            Debug.Log("Wrong actions size: " + size + " != " + actionSpace);
            return;
        }

        foreach (var joint in joints)
        {
            float x = 0, y = 0, z = 0;
            if (joint.angularXMotion != ConfigurableJointMotion.Locked)
                x = actions[actionIdx++];
            if (joint.angularYMotion != ConfigurableJointMotion.Locked)
                y = actions[actionIdx++];
            if (joint.angularZMotion != ConfigurableJointMotion.Locked)
                z = actions[actionIdx++];
            joint.targetAngularVelocity = new Vector3(x, y, z);
        }
    }
}