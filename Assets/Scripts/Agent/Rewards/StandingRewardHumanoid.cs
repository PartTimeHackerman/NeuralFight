﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Agent;
using UnityEngine;

public class StandingRewardHumanoid : MonoBehaviour, IReward
{
    public bool debug = false;
    private float maxDistanceRootFeets;
    private float baseDistanceCOMTorso;
    private float baseDistanceFeetsCOM;
    private BodyParts bodyParts;
    private Dictionary<string, GameObject> namedParts;
    private PhysicsUtils physics;

    public bool calcAvgReward = false;
    public float reward;
    private float avgReward = 0f;
    private float avgRewardSum = 0f;
    private float avgCounter = 0f;

    private float lastClearReward = 0;
    public float COMOverMeanOfFeetsXZReward;
    public float minimizeTorsoXZVelocityReward;
    public float rootFromBaseOverMeanOfFeetsYReward;
    public float torsoOverCOMXZReward;
    public float minimizeActuationReward;

    public float[] multipler;

    private Rigidbody root;
    private Vector3 COM;

    private ActuationReward actuationReward;

    public StandingRewardHumanoid(BodyParts bodyParts)
    {
        this.bodyParts = bodyParts;
        root = bodyParts.root;
        physics = PhysicsUtils.get();
    }

    void Start()
    {
        this.bodyParts = GetComponent<BodyParts>();
        root = bodyParts.root;
        physics = PhysicsUtils.get();
        Init();

        if (debug)
            InvokeRepeating("getReward", 0.0f, 1f);
    }

    void LateFixedUpdate()
    {
        if (calcAvgReward)
        {
            getReward();

            avgRewardSum += reward;
            avgCounter++;
            avgReward = avgRewardSum / avgCounter;
        }
    }

    public void Init()
    {
        namedParts = bodyParts.getNamedParts();

        baseDistanceCOMTorso =
            namedParts["torso"].transform.position.y - physics.getCenterOfMass(bodyParts.getRigids()).y;
        baseDistanceFeetsCOM = physics.getCenterOfMass(bodyParts.getRigids()).y - meanOfFeets().y;
        maxDistanceRootFeets = calcDistance(root.gameObject, namedParts["rfoot_end"]);

        actuationReward = new ActuationReward(bodyParts, false);
        multipler = multipler ?? new float[] {1f, 1f, 1f, 1f, 1f};
    }

    public void setMultipler(float[] multipler)
    {
        this.multipler = multipler;
    }


    public float COMOverMeanOfFeetsX()
    {
        var feetsMean = meanOfFeets();
        float distPrec = 0;
        distPrec = Mathf.Abs(Mathf.Abs(feetsMean.x - COM.x) / baseDistanceFeetsCOM - 1);
        COMOverMeanOfFeetsXZReward = Mathf.Clamp(distPrec, 0f, 1f);
        if (COM.y < feetsMean.y)
            COMOverMeanOfFeetsXZReward *= -1;
        return distPrec;
    }

    public float torsoOverCOMXZ()
    {
        Vector3 torso = namedParts["torso"].transform.position;
        float distPrec = 0;
        distPrec = Mathf.Abs(Mathf.Abs(COM.x - torso.x) / baseDistanceCOMTorso - 1);
        torsoOverCOMXZReward = Mathf.Clamp(distPrec, 0f, 1f);
        if (torso.y < COM.y)
            torsoOverCOMXZReward *= -1;
        return distPrec;
    }

    public virtual float rootFromBaseOverMeanOfFeetsY()
    {
        float distYPrec = 0;
        distYPrec = Mathf.Abs(root.transform.position.y - meanOfFeets().y) / maxDistanceRootFeets;
        rootFromBaseOverMeanOfFeetsYReward = Mathf.Clamp(distYPrec, 0f, 1f);
        rootFromBaseOverMeanOfFeetsYReward = Mathf.Clamp(rootFromBaseOverMeanOfFeetsYReward, -1f, 1f);
        if (root.transform.position.y < meanOfFeets().y)
            rootFromBaseOverMeanOfFeetsYReward *= -1;
        return distYPrec;
    }

    public float minimizeTorsoXZVelocity()
    {
        var torsoRigid = bodyParts.getNamedRigids()["torso"];
        var sumVel = torsoRigid.velocity.sqrMagnitude;
        float maxVel = 10;
        sumVel = Mathf.Clamp(sumVel, 0f, maxVel);
        minimizeTorsoXZVelocityReward = Mathf.Clamp((Mathf.Abs(sumVel / maxVel - 1)), 0f, 1f);
        return minimizeTorsoXZVelocityReward;
    }

    public Vector2 meanOfFeets()
    {
        Vector3 rfootPos = namedParts["rfoot_end"].transform.position;
        Vector2 rfoot = new Vector2(rfootPos.x, rfootPos.y);
        Vector3 lfootPos = namedParts["lfoot_end"].transform.position;
        Vector2 lfoot = new Vector2(lfootPos.x, lfootPos.y);
        return (rfoot + lfoot) / 2;
    }

    public float getReward()
    {
        COM = physics.getCenterOfMass(bodyParts.getRigids());
        COMOverMeanOfFeetsX();
        torsoOverCOMXZ();
        rootFromBaseOverMeanOfFeetsY();
        minimizeTorsoXZVelocity();
        minimizeActuationReward = actuationReward.getReward();

        reward = (COMOverMeanOfFeetsXZReward * multipler[0] +
                  torsoOverCOMXZReward * multipler[1] +
                  rootFromBaseOverMeanOfFeetsYReward * multipler[2] +
                  minimizeTorsoXZVelocityReward * multipler[3] +
                  minimizeActuationReward * multipler[4]) / multipler.Sum();
        reward = Mathf.Clamp(reward, -1f, 1f);
        reward = RewardFunctions.toleranceInvNoBounds(reward, 1f, .1f, RewardFunction.LONGTAIL);
        return reward;
    }

    public float getAvgReward()
    {
        float avgRew = avgRewardSum / (avgCounter == 0f ? 1 : avgCounter);
        avgCounter = 0f;
        avgRewardSum = 0f;
        return avgRew;
    }

    public bool terminated(int step)
    {
        return false;
    }

    public bool terminated()
    {
        return false;
    }

    private float calcDistance(GameObject o1, GameObject o2)
    {
        return Vector3.Distance(o1.transform.position, o2.transform.position);
    }
}