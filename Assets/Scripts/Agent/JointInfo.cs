using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JointInfo : MonoBehaviour
{
    public ConfigurableJoint joint;
    public ConfigurableJoint configurableJoint;
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

    void Reset()
    {
        init();
        if (setSettings)
            SetConfigurableJointSettings();
    }


    void Start()
    {
        init();
        if (setSettings)
            SetConfigurableJointSettings();
        isBackwards = joint.axis.z < 0f;
        /*
        medVel = maxVel / 2;
        currentMaxVel = maxVel;
        */
    }

    public void init()
    {
        joint = GetComponent<ConfigurableJoint>();
        configurableJoint = (ConfigurableJoint) joint;
        Rigidbody[] rigids = joint.GetComponentsInChildren<Rigidbody>();
        totalMass = rigids.Sum(r => r.mass);
    }

    void Update()
    {
        updateConfigurableJoint();
    }

    public void updateConfigurableJoint()
    {
        if (!setPos)
        {
            posDoc = configurableJoint.targetRotation.eulerAngles;
        }
        else
        {
            posDoc.x = Mathf.Clamp(posDoc.x, angularLimits[0][0], angularLimits[0][1]);
            posDoc.y = Mathf.Clamp(posDoc.y, angularLimits[1][0], angularLimits[1][1]);
            posDoc.z = Mathf.Clamp(posDoc.z, angularLimits[2][0], angularLimits[2][1]);
            configurableJoint.targetRotation = Quaternion.Euler(posDoc);
        }

        if (debug)
        {
            SetConfigurableJointSettings();
            debug = false;
        }
    }


    public void SetConfigurableJointSettings()
    {
        setConfigurableJointInfo();
        if (setVelSettings)
        {
            maxPosSpring = maxForce * totalMass;
            maxPosDamper = maxForce * totalMass;
        }
        else
        {
           // maxPosSpring = maxForce * totalMass;
            //maxPosDamper = (maxForce * totalMass) / 20f;
            maxPosSpring = 10000f;
            maxPosDamper = 50f;
        }

        JointDrive jointSlerpDrive = configurableJoint.slerpDrive;
        jointSlerpDrive.positionSpring = maxPosSpring;
        jointSlerpDrive.positionDamper = maxPosDamper;
        configurableJoint.slerpDrive = jointSlerpDrive;
    }


    public void setConfigurableJointInfo()
    {
        if (configurableJoint.angularXMotion != ConfigurableJointMotion.Locked)
            movableAxis[0] = true;
        if (configurableJoint.angularYMotion != ConfigurableJointMotion.Locked)
            movableAxis[1] = true;
        if (configurableJoint.angularZMotion != ConfigurableJointMotion.Locked)
            movableAxis[2] = true;


        angularLimits[0] = new float[] {0, 0};
        angularLimits[1] = new float[] {0, 0};
        angularLimits[2] = new float[] {0, 0};
        if (movableAxis[0])
        {
            angularLimits[0] = new float[]
                {configurableJoint.lowAngularXLimit.limit, configurableJoint.highAngularXLimit.limit};
        }

        if (movableAxis[1])
        {
            angularLimits[1] = new float[]
                {-configurableJoint.angularYLimit.limit, configurableJoint.angularYLimit.limit};
        }

        if (movableAxis[2])
        {
            angularLimits[2] = new float[]
                {-configurableJoint.angularZLimit.limit, configurableJoint.angularZLimit.limit};
        }

        xMinMax = angularLimits[0];
        yMinMax = angularLimits[1];
        zMinMax = angularLimits[2];
        maxForce = maxForce == 0 ? totalMass * massMultipler : maxForce;
    }


    public void setConfigurableForceAndRot(float force, Vector3 angRot)
    {
        JointDrive jointSlerpDrive = configurableJoint.slerpDrive;
        jointSlerpDrive.positionSpring = maxPosSpring;
        jointSlerpDrive.positionDamper = maxPosDamper;
        jointSlerpDrive.maximumForce = ((force + 1f) * .5f) * maxSlerpForce;
        configurableJoint.slerpDrive = jointSlerpDrive;
        currentRot = angRot.x;
        setConfigurableRot(angRot);
    }

    public void setConfigurableRot(Vector3 angRot)
    {
        configurableJoint.targetRotation = Quaternion.Euler(angRot);
    }

    public void resetJointForces()
    {
        JointDrive jointSlerpDrive = configurableJoint.slerpDrive;
        jointSlerpDrive.positionSpring = maxPosSpring;
        jointSlerpDrive.positionDamper = maxPosDamper;
        configurableJoint.slerpDrive = jointSlerpDrive;
    }

    public void resetJointPositions(Vector3 zeros)
    {
        configurableJoint.targetRotation = Quaternion.Euler(zeros);
    }

    public void setConfigurableRotVel(Vector3 angVel)
    {
        /*
        if (angVel.x < 0 && angVel.x > -threshold * maxVel)
            angVel.x = 0;
        if (angVel.x > 0 && angVel.x < threshold * maxVel)
            angVel.x = 0;
        
        float applyVel = Mathf.Abs(angVel.x);
        float velTiredness = applyVel - medVel;
        angVel.x = Mathf.Clamp(angVel.x, -currentMaxVel, currentMaxVel);
        currentMaxVel -= velTiredness;
        if (currentMaxVel < 10)
            currentMaxVel = 10;
        if (currentMaxVel > maxVel)
            currentMaxVel = maxVel;
        tiredness = -(((currentMaxVel - medVel) / medVel) - 1);
        */
        configurableJoint.targetAngularVelocity = angVel;
    }

    public override bool Equals(object other)
    {
        return other != null && ((JointInfo) other).configurableJoint.Equals(configurableJoint);
    }

    public override int GetHashCode()
    {
        return configurableJoint.GetHashCode();
    }
}