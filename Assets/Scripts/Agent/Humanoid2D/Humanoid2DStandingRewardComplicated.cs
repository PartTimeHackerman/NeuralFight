﻿using System;
using System.Collections.Generic;
using Assets.Scripts.Agent;
using UnityEngine;
using UnityEngine.Collections;
using Random = UnityEngine.Random;

public class Humanoid2DStandingRewardComplicated : MonoBehaviour, IReward
{
    public bool debug = false;
    private float baseDistanceFeetsTorso;
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


    public float penalty = 0;

    public float maxPenaltyCount = 100;
    public float maxPenalty = .3f;
    public float penaltySpeed = 1f;
    public float penaltyThreshold = .1f;
    public float penaltyStart = .5f;

    private float penaltyCount = 0;
    private float penaltyPerPoint = 0;
    
    private float penaltyAdd = 0;

    private float unpenaltySpeed  = 0;
    private float unpenaltyAdd = 0;

    private readonly int rewards = 5;

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


        baseDistanceCOMTorso = distanceTorsoOverCOMXZ();
        baseDistanceFeetsCOM = distanceCOMOverMeanOfFeetsXZ();
        baseDistanceFeetsTorso = distanceFeetsTorso();

        if (debug)
            InvokeRepeating("log", 0.0f, .1f);
    }


    public float COMOverMeanOfFeetsXZ()
    {
        var distPrec = distanceCOMOverMeanOfFeetsXZ() / baseDistanceFeetsCOM;
        COMOverMeanOfFeetsXZReward = distPrec;
        return distPrec;
    }

    public float torsoOverCOMXZ()
    {
        var distPrec = distanceTorsoOverCOMXZ() / baseDistanceCOMTorso;
        torsoOverCOMXZReward = distPrec;
        return distPrec;
    }

    public float torsoFromBaseOverMeanOfFeetsY()
    {
        var distPrec = distanceFeetsTorso() / baseDistanceFeetsTorso;
        torsoFromBaseOverMeanOfFeetsYReward = distPrec;
        return distPrec;
    }

    public float minimizeTorsoXZVelocity()
    {
        
        var torsoRigid = bodyParts.getNamedRigids()["torso"];
        var sumVelX = torsoRigid.velocity.x;

        reward = Mathf.Abs(Mathf.Abs(sumVelX)*10-1);
        minimizeTorsoXZVelocityReward = reward;
        return reward;
    }

    public float headGroundDist()
    {
        var headYPos = namedParts["head_end"].transform.position.y;
        var headFromGroundPrec = headYPos / 1.9f;
        headGroundDistReward = headFromGroundPrec;
        return headFromGroundPrec;
    }

    public Vector2 meanOfFeets()
    {
        Vector3 rfootPos = namedParts["rfoot_end"].transform.position;
        Vector2 rfoot= new Vector2(rfootPos.x, rfootPos.y);
        Vector3 lfootPos = namedParts["lfoot_end"].transform.position;
        Vector2 lfoot= new Vector2(lfootPos.x, lfootPos.y);
        return (rfoot + lfoot) / 2;
    }

    public float distanceCOMOverMeanOfFeetsXZ()
    {
        // dist = 0, reward = 1000; dist = 1, reward = 0
        float reward = 0;
        var COM = physics.getCenterOfMass(bodyParts.getRigids());
        var COMXY = new Vector2(COM.x, COM.y);
        var feetsMean = meanOfFeets();
        return Vector2.Distance(COMXY, feetsMean);
    }

    public float distanceTorsoOverCOMXZ()
    {
        var COM = physics.getCenterOfMass(bodyParts.getRigids());
        var COMXY = new Vector2(COM.x, COM.y);
        var torso = namedParts["torso"].transform.position;
        var torsoXY = new Vector2(torso.x, torso.y);
        return Vector2.Distance(COMXY, torsoXY);
    }

    public float distanceFeetsTorso()
    {
        return Vector2.Distance(meanOfFeets(), namedParts["torso"].transform.position);
    }

    public float getReward()
    {
        COMOverMeanOfFeetsXZ();
        torsoOverCOMXZ();
        torsoFromBaseOverMeanOfFeetsY();
        minimizeTorsoXZVelocity();
        headGroundDist();
        minimizeTorsoXZVelocityReward *= headGroundDistReward;
        reward = (COMOverMeanOfFeetsXZReward + torsoOverCOMXZReward + torsoFromBaseOverMeanOfFeetsYReward +
                 minimizeTorsoXZVelocityReward + headGroundDistReward) / rewards;
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
        bool terminated = step >= maxStep || (this.terminated() && penaltyCount >= maxPenaltyCount) ;
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
}