using System.Collections.Generic;
using System.Runtime.Remoting;
using UnityEngine;

public abstract class AgentAction : MonoBehaviour
{
    public Transform target;
    public float strength = 1f;

    protected BodyParts BodyParts;
    protected Transform root;
    protected JointInfo upperArm;
    protected JointInfo lowerArm;
    private Vector3 eulerRot;
    private Vector3 relativePos;

    public bool activate = false;
    public bool done = true;

    protected void Start()
    {
        BodyParts = GetComponent<BodyParts>();

        foreach (KeyValuePair<string, JointInfo> keyValuePair in BodyParts.namedJoints)
        {
            if (keyValuePair.Key.Contains("upper")) upperArm = keyValuePair.Value;
            if (keyValuePair.Key.Contains("lower")) lowerArm = keyValuePair.Value;
        }

        root = BodyParts.root.transform;
    }

    void FixedUpdate()
    {
        if (activate)
        {
            done = false;
            TakeAction();
            if (done) activate = false;
        }
    }

    protected abstract void TakeAction();

    protected virtual float getRotation()
    {
        relativePos = root.transform.InverseTransformPoint(target.position);
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