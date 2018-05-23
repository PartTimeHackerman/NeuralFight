using System.Collections.Generic;
using UnityEngine;

public class ObservationsSmallXY : Observations
{
    private readonly float maxPos = 100;
    private readonly float maxVel = 100;
    private readonly float minPos = -100;
    private readonly float minVel = -100;


    protected override void Start()
    {
        base.Start();
    }

    public override List<float> getObservations()
    {
        observations.Clear();
        var root = bodyParts.getNamedRigids()["torso"];
        var rootPos = root.transform.position;
        var rootRot = root.transform.rotation;
        observations.Add(normPos(rootPos.x));
        observations.Add(normPos(rootPos.y));
        observations.Add(normPos(rootPos.z));
        observations.Add(rootRot.x);
        observations.Add(rootRot.y);
        observations.Add(rootRot.z);
        observations.Add(rootRot.w);

        var com = getCenterOfMass();
        observations.Add(normPos(com.x));
        observations.Add(normPos(com.y));
        observations.Add(normPos(com.z));

        foreach (var joint in bodyParts.getJoints())
        {
            var jointRb = joint.GetComponent<Rigidbody>();
            var jointTransform = joint.transform;
            var eulerRot = jointTransform.localRotation.eulerAngles;

            observations.Add(jointTransform.localPosition.x);
            observations.Add(jointTransform.localPosition.y);

            observations.Add(normEuler(eulerRot.x));

            observations.Add(normVel(jointRb.angularVelocity.x));

            observations.Add(normVel(jointRb.velocity.x));
            observations.Add(normVel(jointRb.velocity.y));

            //observations.Add(jointRb.inertiaTensor.x);
            //observations.Add(jointRb.inertiaTensorRotation.x);
        }

        return observations;
    }

    public float normPos(float pos)
    {
        return (Mathf.Clamp(pos, minPos, maxPos) - minPos) / (maxPos - minPos);
    }

    public float normVel(float vel)
    {
        return (Mathf.Clamp(vel, minVel, maxVel) - minVel) / (maxVel - minVel);
    }

    public float normEuler(float rot)
    {
        rot = rot % 360;
        return rot / 360;
    }
}