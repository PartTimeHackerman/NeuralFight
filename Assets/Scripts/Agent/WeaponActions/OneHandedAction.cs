using System.Collections.Generic;
using System.Runtime.Remoting;
using UnityEngine;

public abstract class OneHandedAction : WeaponAction
{

    public Transform root;
    protected JointInfo upperArm;
    protected JointInfo lowerArm;
    public Vector3 eulerRot;
    private Vector3 relativePos;


    public override void setUpAction(BodyParts bodyParts)
    {
        base.setUpAction(bodyParts);

        foreach (KeyValuePair<string, JointInfo> keyValuePair in BodyParts.namedJoints)
        {
            if (keyValuePair.Key.Contains("upper")) upperArm = keyValuePair.Value;
            if (keyValuePair.Key.Contains("lower")) lowerArm = keyValuePair.Value;
        }

        root = BodyParts.root.transform;
    }

    protected float getRotation()
    {
        return getRotation(root, target);
    }
    
    protected float getRotation(Transform from, Transform to)
    {
        relativePos = from.transform.InverseTransformPoint(to.position);
        Quaternion rotation = LookAt(relativePos);
        eulerRot = rotation.eulerAngles;
        if (eulerRot.z > 180)
        {
            eulerRot.z = eulerRot.z - 360f;
        }

        return eulerRot.z;
    }

    protected Quaternion LookAt(Vector3 relativePos)
    {
        float rotationZ = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0.0f, 0.0f, -rotationZ);
    }
}