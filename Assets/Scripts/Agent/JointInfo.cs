using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JointInfo : MonoBehaviour
{
    private static readonly Dictionary<string, float> maxForces = getMaxForcesDict();

    public ConfigurableJoint joint;
    public float maxForce = 150;
    public float maxPosSpring;
    public float maxPosDamper;
    public bool[] movableAxis;
    public float[][] angularLimits;
    public float totalMass;
    public float massMultipler = 5;
    public bool setSettings = false;
    public float[] xMinMax, yMinMax, zMinMax;
    public int conAxis = -1;


    public bool setPos = false;
    public Vector3 posDoc = new Vector3(0, 0, 0);

    public bool debug;
    public float springMult = .1f;

    public bool setVelSettings = false;
    public float maxVel = 30;
    public float force = 1000;
    

    void Reset()
    {
        init();
    }


    void Start()
    {
        init();
        if (setSettings)
        {
            SetSettings();
        }
    }

    public void init()
    {
        joint = GetComponent<ConfigurableJoint>();
        /*
          foreach (KeyValuePair<string, float> force in maxForces)
            if (joint.name.Contains(force.Key))
                maxForce = force.Value;
         */
        //maxForce = 100;
        setJointInfo();
    }
    
    void Update()
    {
        if (!setPos)
        {
            posDoc = joint.targetRotation.eulerAngles;
        }else
        {
            posDoc.x = Mathf.Clamp(posDoc.x, angularLimits[0][0], angularLimits[0][1]);
            posDoc.y = Mathf.Clamp(posDoc.y, angularLimits[1][0], angularLimits[1][1]);
            posDoc.z = Mathf.Clamp(posDoc.z, angularLimits[2][0], angularLimits[2][1]);
            joint.targetRotation = Quaternion.Euler(posDoc);
        }

        if (debug)
        {
            SetSettings();
            debug = false;
        }

        
    }

    public void SetSettings()
    {
        if (setVelSettings)
        {
            maxPosSpring = force;
            maxPosDamper = force;
        }
        else
        {

        maxPosSpring = maxForce * totalMass;
        maxPosDamper = maxPosSpring * springMult;
        }

        JointDrive jointSlerpDrive = joint.slerpDrive;
        jointSlerpDrive.positionSpring = maxPosSpring;
        jointSlerpDrive.positionDamper = maxPosDamper;
        joint.slerpDrive = jointSlerpDrive;
        //joint.massScale = totalMass;
    }

    public void setJointInfo()
    {
        movableAxis = new[] { false, false, false };
        if (joint.angularXMotion != ConfigurableJointMotion.Locked)
            movableAxis[0] = true;
        if (joint.angularYMotion != ConfigurableJointMotion.Locked)
            movableAxis[1] = true;
        if (joint.angularZMotion != ConfigurableJointMotion.Locked)
            movableAxis[2] = true;

        angularLimits = new float[3][];
        angularLimits[0] = new float[] {0, 0};
        angularLimits[1] = new float[] {0, 0};
        angularLimits[2] = new float[] {0, 0};
        if (movableAxis[0])
        {
            angularLimits[0] = new float[] { joint.lowAngularXLimit.limit, joint.highAngularXLimit.limit };
        }
        if (movableAxis[1])
        {
            angularLimits[1] = new float[] { -joint.angularYLimit.limit, joint.angularYLimit.limit };
        }
        if (movableAxis[2])
        {
            angularLimits[2] = new float[] { -joint.angularZLimit.limit, joint.angularZLimit.limit };
        }

        xMinMax = angularLimits[0];
        yMinMax = angularLimits[1];
        zMinMax = angularLimits[2];

        Rigidbody[] rigids = joint.GetComponentsInChildren<Rigidbody>();
        totalMass = rigids.Sum(r => r.mass);
        maxForce = maxForce == 0 ? totalMass * massMultipler : maxForce;

    }

    private static Dictionary<string, float> getMaxForcesDict()
    {
        Dictionary<string, float> maxForces = new Dictionary<string, float>
        {
            ["lwaist"] = 300,
            ["uwaist"] = 200,
            ["torso"] = 150,
            ["head"] = 50,
            ["upperarm"] = 120,
            ["lowerarm"] = 80,
            ["hand"] = 40,
            ["thigh"] = 400,
            ["shin"] = 200,
            ["foot"] = 100
        };
        return maxForces;
    }

    public override bool Equals(object other)
    {
        return other != null && ((JointInfo)other).joint.Equals(joint);
    }

    public override int GetHashCode()
    {
        return joint.GetHashCode();
    }

}