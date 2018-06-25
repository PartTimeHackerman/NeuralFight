using System;
using System.Collections.Generic;
using Assets.Scripts.Agent;
using UnityEngine;
using UnityEngine.Collections;
using Random = UnityEngine.Random;

public class Humanoid2DStandingRewardComplicated : MonoBehaviour, IReward
{
    public bool debug = false;
    private float maxDistanceTorsoFeets;
    private float baseDistanceCOMTorso;
    private float baseDistanceFeetsCOM;
    private BodyParts bodyParts;
    private Dictionary<string, GameObject> namedParts;
    private PhysicsUtils physics;

    public float reward;
    //public float clearReward;

    public float lastClearReward = 0;
    public float COMOverMeanOfFeetsXZReward;
    public float headGroundDistReward;
    public float minimizeTorsoXZVelocityReward;
    public float torsoFromBaseOverMeanOfFeetsYReward;
    public float torsoOverCOMXZReward;
    public float distanceZReward;
    public float minimizeActuationReward;


    public float penalty = 0;

    public float maxPenaltyCount = 100;
    public float maxPenalty = .3f;
    public float penaltySpeed = 1f;
    public float penaltyThreshold = .1f;
    public float penaltyStart = .5f;

    private float penaltyCount = 0;
    private float penaltyPerPoint = 0;

    private float penaltyAdd = 0;

    private float unpenaltySpeed = 0;
    private float unpenaltyAdd = 0;

    private readonly int rewards = 7;

    public int step = 0;
    public int maxStep = 300;

    private void Start()
    {
        bodyParts = GetComponent<BodyParts>();
        namedParts = bodyParts.getNamedParts();
        physics = PhysicsUtils.get();


        unpenaltySpeed = penaltySpeed / 3;
        penaltyPerPoint = maxPenalty / maxPenaltyCount;
        penaltyAdd = penaltySpeed * maxPenaltyCount / maxPenaltyCount;
        unpenaltyAdd = unpenaltySpeed / maxPenaltyCount;


        baseDistanceCOMTorso = namedParts["torso"].transform.position.y - physics.getCenterOfMass(bodyParts.getRigids()).y;
        baseDistanceFeetsCOM = physics.getCenterOfMass(bodyParts.getRigids()).y - meanOfFeets().y;
        maxDistanceTorsoFeets = calcDistance(namedParts["torso"], namedParts["rfoot_end"]);

        if (debug)
            InvokeRepeating("log", 0.0f, .1f);
    }


    public float COMOverMeanOfFeetsZ()
    {
        Vector2 COM = physics.getCenterOfMass(bodyParts.getRigids());
        var feetsMean = meanOfFeets();
        float distPrec = 0;
        if (COM.y > feetsMean.y)
            distPrec = Mathf.Abs(Mathf.Abs(feetsMean.x - COM.x) / baseDistanceFeetsCOM - 1);
        COMOverMeanOfFeetsXZReward = Mathf.Clamp(distPrec, 0f, 1f);
        return distPrec;
    }

    public float torsoOverCOMXZ()
    {
        Vector2 COM = physics.getCenterOfMass(bodyParts.getRigids());
        Vector3 torso = namedParts["torso"].transform.position;
        float distPrec = 0;
        if (torso.y > COM.y)
            distPrec = Mathf.Abs(Mathf.Abs(COM.x - torso.z) / baseDistanceCOMTorso - 1);
        torsoOverCOMXZReward = Mathf.Clamp(distPrec, 0f, 1f);
        return distPrec;
    }

    public float torsoFromBaseOverMeanOfFeetsY()
    {
        float distYPrec = 0;
        /*
        if (namedParts["torso"].transform.position.y > meanOfFeets().y)
            distYPrec = Mathf.Abs(Mathf.Abs(meanOfFeets().x - namedParts["torso"].transform.position.z) /
                                  maxDistanceTorsoFeets - 1);
       */
        if (namedParts["torso"].transform.position.y > meanOfFeets().y)
            distYPrec = Mathf.Abs(namedParts["torso"].transform.position.y - meanOfFeets().y) / maxDistanceTorsoFeets;
        torsoFromBaseOverMeanOfFeetsYReward = Mathf.Clamp(distYPrec, 0f, 1f);
        return distYPrec;
    }

    public float minimizeTorsoXZVelocity()
    {

        var torsoRigid = bodyParts.getNamedRigids()["torso"];
        var sumVelZ = torsoRigid.velocity.z;
        float maxVel = 1;
        reward = Mathf.Clamp((Mathf.Abs(Mathf.Abs(sumVelZ) / maxVel - 1)), 0f, 1f);
        minimizeTorsoXZVelocityReward = reward;
        return reward;
    }

    public float headGroundDist()
    {
        var headYPos = namedParts["head_end"].transform.position.y;
        var headFromGroundPrec = headYPos / 1.9f;
        if (headYPos > 1.9f)
        {
            headFromGroundPrec = -(1 - headFromGroundPrec) * 2;
        }
        headFromGroundPrec = Mathf.Clamp(headFromGroundPrec, 0f, 1f);
        headGroundDistReward = headFromGroundPrec;
        return headFromGroundPrec;
    }

    public Vector2 meanOfFeets()
    {
        Vector3 rfootPos = namedParts["rfoot_end"].transform.position;
        Vector2 rfoot = new Vector2(rfootPos.z, rfootPos.y);
        Vector3 lfootPos = namedParts["lfoot_end"].transform.position;
        Vector2 lfoot = new Vector2(lfootPos.z, lfootPos.y);
        return (rfoot + lfoot) / 2;
    }

    public float DistanceZ()
    {
        float reward = 0;
        float threshold = 0.1f;
        float maxZDist = 1f;
        float posZ = Mathf.Min(Mathf.Abs(namedParts["butt"].transform.position.z), maxZDist);
        if (posZ < threshold)
            reward = 1;
        else
            reward = Mathf.Abs(posZ / maxZDist - 1);

        distanceZReward = reward;
        return reward;
    }

    public float minimizeActuation()
    {
        List<JointInfo> jointInfos = bodyParts.jointsInfos;
        float maxVel = jointInfos[0].maxVel;
        int count = jointInfos.Count;
        float sumVels = 0;
        foreach (JointInfo jointInfo in jointInfos)
        {
            sumVels += Mathf.Abs(jointInfo.configurableJoint.targetAngularVelocity.x);
        }

        minimizeActuationReward = Mathf.Abs((sumVels / count) / maxVel - 1);
        return minimizeActuationReward;
    }

    public float getReward()
    {
        COMOverMeanOfFeetsZ();
        torsoOverCOMXZ();
        torsoFromBaseOverMeanOfFeetsY();
        minimizeTorsoXZVelocity();
        headGroundDist();
        DistanceZ();
        minimizeActuation();
        //minimizeTorsoXZVelocityReward *= headGroundDistReward;
        reward = (//COMOverMeanOfFeetsXZReward +
                  //torsoOverCOMXZReward +
                  //torsoFromBaseOverMeanOfFeetsYReward +
                  //minimizeTorsoXZVelocityReward +
                  headGroundDistReward +
                  distanceZReward / 2 +
                  minimizeActuationReward) / 2.5f;
        reward = Mathf.Clamp(reward, 0f, 1f);
        return reward;
    }

    public float calcPenalty(float clearReward)
    {
        unpenaltySpeed = penaltySpeed / 3;
        penaltyPerPoint = maxPenalty / maxPenaltyCount;
        penaltyAdd = penaltySpeed * maxPenaltyCount / maxPenaltyCount;
        unpenaltyAdd = unpenaltySpeed / maxPenaltyCount;
        penalty = penaltyPerPoint * penaltyCount;
        reward = clearReward - penalty;
        if (clearReward <= lastClearReward + penaltyThreshold && clearReward < penaltyStart)
        {
            if (penaltyCount < maxPenaltyCount)
                penaltyCount += penaltyAdd;
            if (penaltyCount > maxPenaltyCount)
                penaltyCount = maxPenaltyCount;
        }
        else
        {
            penaltyCount -= unpenaltyAdd;
            if (clearReward >= penaltyStart)
                penaltyCount -= unpenaltyAdd * 2;

            if (penaltyCount < 0)
                penaltyCount = 0;
        }

        lastClearReward = clearReward;
        return reward;
    }

    public bool terminated(int step)
    {
        this.step = step;
        bool terminated = step >= maxStep || (this.terminated() && penaltyCount >= maxPenaltyCount);
        if (terminated)
        {
            penaltyCount = 0;
        }

        return terminated;
    }

    public bool terminated()
    {
        bool terminated = reward <= 0f;
        return terminated;
    }


    public void log()
    {
        getReward();
        terminated();
    }

    private float calcDistance(GameObject o1, GameObject o2)
    {
        return Vector3.Distance(o1.transform.position, o2.transform.position);
    }
}