using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JointInfo : MonoBehaviour
{
    public ConfigurableJoint joint;
    public bool useNeural = true;
    public bool isEnabled = true;

    public float maxForce = 1000;
    public float maxSlerpForce = 1000;
    public float maxPosSpring;
    public float maxPosDamper;
    public bool[] movableAxis = new[] {false, false, false};
    public float[][] angularLimits = new float[3][];
    public float totalMass;
    public float massMultipler = 5;
    public bool setSettings = false;
    public float[] xMinMax, yMinMax, zMinMax;

    public bool setPos = false;
    public Vector3 posDoc = new Vector3(0, 0, 0);

    public bool debug;
    public float springMult = .01f;

    public bool setVelSettings = false;
    public float maxVel = 20;
    public float currentRot = 0f;
    public bool isBackwards = false;
    public float currentForce = 0f;

    void Reset()
    {
        Init();
    }

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        joint = GetComponent<ConfigurableJoint>();
        Rigidbody[] rigids = joint.GetComponentsInChildren<Rigidbody>();
        totalMass = rigids.Sum(r => r.mass);
        SetConfigurableJointInfo();
        if (setSettings)
            SetConfigurableJointSettings();
        isBackwards = joint.axis.z < 0f;
    }
/*

    void FixedUpdate()
    {
        updateConfigurableJoint();
    }
*/

    public void SetConfigurableForceAndRot(float force, Vector3 angRot)
    {
        if (!isEnabled)
        {
            return;
        }
        currentRot = angRot.x;
        setConfigurableRot(angRot);
        JointDrive jointSlerpDrive = joint.slerpDrive;
        jointSlerpDrive.positionSpring = maxPosSpring;
        jointSlerpDrive.positionDamper = maxPosDamper;
        jointSlerpDrive.maximumForce = force * maxSlerpForce;
        joint.slerpDrive = jointSlerpDrive;
        currentForce = force;
    }

    public void setConfigurableRot(Vector3 angRot)
    {
        joint.targetRotation = Quaternion.Euler(angRot);
    }

    public void Disable()
    {
        isEnabled = false;
        JointDrive jointSlerpDrive = joint.slerpDrive;
        jointSlerpDrive.positionSpring = 0f;
        jointSlerpDrive.positionDamper = 0f;
        jointSlerpDrive.maximumForce = 0f;
        joint.slerpDrive = jointSlerpDrive;
    }
    
    public void Enable()
    {
        isEnabled = true;
        resetJointForces();
    }

    public void updateConfigurableJoint()
    {
        if (setPos)
        {
            posDoc.x = Mathf.Clamp(posDoc.x, angularLimits[0][0], angularLimits[0][1]);
            posDoc.y = Mathf.Clamp(posDoc.y, angularLimits[1][0], angularLimits[1][1]);
            posDoc.z = Mathf.Clamp(posDoc.z, angularLimits[2][0], angularLimits[2][1]);
            joint.targetRotation = Quaternion.Euler(posDoc);
        }

        if (debug)
        {
            SetConfigurableJointSettings();
            debug = false;
        }
    }


    public void SetConfigurableJointSettings()
    {
        maxPosSpring = maxForce * totalMass;
        maxPosDamper = (maxForce * totalMass) / 20f;

        JointDrive jointSlerpDrive = joint.slerpDrive;
        jointSlerpDrive.positionSpring = maxPosSpring;
        jointSlerpDrive.positionDamper = maxPosDamper;
        joint.slerpDrive = jointSlerpDrive;
    }


    public void SetConfigurableJointInfo()
    {
        if (joint.angularXMotion != ConfigurableJointMotion.Locked)
            movableAxis[0] = true;
        if (joint.angularYMotion != ConfigurableJointMotion.Locked)
            movableAxis[1] = true;
        if (joint.angularZMotion != ConfigurableJointMotion.Locked)
            movableAxis[2] = true;


        angularLimits[0] = new float[] {0, 0};
        angularLimits[1] = new float[] {0, 0};
        angularLimits[2] = new float[] {0, 0};
        if (movableAxis[0])
        {
            angularLimits[0] = new float[]
                {joint.lowAngularXLimit.limit, joint.highAngularXLimit.limit};
        }

        if (movableAxis[1])
        {
            angularLimits[1] = new float[]
                {-joint.angularYLimit.limit, joint.angularYLimit.limit};
        }

        if (movableAxis[2])
        {
            angularLimits[2] = new float[]
                {-joint.angularZLimit.limit, joint.angularZLimit.limit};
        }

        xMinMax = angularLimits[0];
        yMinMax = angularLimits[1];
        zMinMax = angularLimits[2];
        maxForce = maxForce == 0 ? totalMass * massMultipler : maxForce;
    }


    public void resetJointForces()
    {
        JointDrive jointSlerpDrive = joint.slerpDrive;
        jointSlerpDrive.positionSpring = maxPosSpring;
        jointSlerpDrive.positionDamper = maxPosDamper;
        joint.slerpDrive = jointSlerpDrive;
    }

    public void resetJointPositions(Vector3 zeros)
    {
        joint.targetRotation = Quaternion.Euler(zeros);
    }

    public override bool Equals(object other)
    {
        return other != null && ((JointInfo) other).joint.Equals(joint);
    }

    public override int GetHashCode()
    {
        return joint.GetHashCode();
    }
}