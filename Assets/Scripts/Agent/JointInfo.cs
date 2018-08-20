using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JointInfo : MonoBehaviour
{

    public Joint joint;
    public ConfigurableJoint configurableJoint;
    private HingeJoint hingeJoint;
    public bool hinge = true;
    public float maxForce = 1000;
    public float maxPosSpring;
    public float maxPosDamper;
    public bool[] movableAxis = new[] { false, false, false };
    public float[][] angularLimits = new float[3][];
    public float totalMass;
    public float massMultipler = 5;
    public bool setSettings = false;
    public float[] xMinMax, yMinMax, zMinMax;
    public int conAxis = -1;


    public bool setPos = false;
    public Vector3 posDoc = new Vector3(0, 0, 0);

    public bool debug;
    public float springMult = .01f;

    public bool setVelSettings = false;
    public float maxVel = 20;

    /*
    public float medVel = 10;
    public float currentMaxVel = 20;
    public float tiredness = 0;
    */

    public float force = 1000;
    public float maxHingeVel = 800f;
    public float threshold = .1f;


    void Reset()
    {
        init();
        if (setSettings)
        {
            if (!hinge)
                SetConfigurableJointSettings();
            else
                SetHingeJointSettings();
        }
    }


    void Start()
    {
        init();
        if (setSettings)
        {
            if (!hinge)
                SetConfigurableJointSettings();
            else
                SetHingeJointSettings();
        }
        /*
        medVel = maxVel / 2;
        currentMaxVel = maxVel;
        */
    }

    public void init()
    {
        if (!hinge)
        {

            joint = GetComponent<ConfigurableJoint>();
            configurableJoint = (ConfigurableJoint)joint;
        }
        else
        {
            joint = GetComponent<HingeJoint>();
            hingeJoint = (HingeJoint)joint;
        }

        Rigidbody[] rigids = joint.GetComponentsInChildren<Rigidbody>();
        totalMass = rigids.Sum(r => r.mass);
    }

    void Update()
    {

        if (!hinge)
        {
            updateConfigurableJoint();
        }

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

    public void SetHingeJointSettings()
    {
        JointMotor motor = hingeJoint.motor;
        motor.force = maxForce * totalMass;
        hingeJoint.motor = motor;
        movableAxis[0] = true;
        angularLimits[0] = new float[] { hingeJoint.limits.min, hingeJoint.limits.max };
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

            maxPosSpring = maxForce * totalMass;
            maxPosDamper = maxPosSpring * springMult;
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


        angularLimits[0] = new float[] { 0, 0 };
        angularLimits[1] = new float[] { 0, 0 };
        angularLimits[2] = new float[] { 0, 0 };
        if (movableAxis[0])
        {
            angularLimits[0] = new float[] { configurableJoint.lowAngularXLimit.limit, configurableJoint.highAngularXLimit.limit };
        }
        if (movableAxis[1])
        {
            angularLimits[1] = new float[] { -configurableJoint.angularYLimit.limit, configurableJoint.angularYLimit.limit };
        }
        if (movableAxis[2])
        {
            angularLimits[2] = new float[] { -configurableJoint.angularZLimit.limit, configurableJoint.angularZLimit.limit };
        }

        xMinMax = angularLimits[0];
        yMinMax = angularLimits[1];
        zMinMax = angularLimits[2];
        maxForce = maxForce == 0 ? totalMass * massMultipler : maxForce;

    }

    public void setHingeMotorVel(float motorVel)
    {
        JointMotor motor = hingeJoint.motor;
        motor.targetVelocity = motorVel;
        hingeJoint.motor = motor;
    }

    public void setConfigurableForceAndRot(float force, Vector3 angRot)
    {
        JointDrive jointSlerpDrive = configurableJoint.slerpDrive;
        jointSlerpDrive.positionSpring = force * maxPosSpring;
        jointSlerpDrive.positionDamper = force * maxPosDamper;
        configurableJoint.slerpDrive = jointSlerpDrive;
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
        if (hinge)
            return;

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
        return other != null && ((JointInfo)other).configurableJoint.Equals(configurableJoint);
    }

    public override int GetHashCode()
    {
        return configurableJoint.GetHashCode();
    }

}