using System;
using System.Collections.Generic;
using Assets.Scripts.Agent;
using UnityEngine;
using UnityEngine.Collections;
using Random = UnityEngine.Random;

public class Humanoid2DStandingRewardComplicated : MonoBehaviour, IReward
{
    public bool debug = false;
    private float baseDistanceFeetsTorso;
    private BodyParts bodyParts;
    private Dictionary<string, GameObject> namedParts;
    private PhysicsUtils physics;

    public float reward;
    public float clearReward;

    private float lastClearReward = 0;
    private float COMOverMeanOfFeetsXZReward;
    private float headGroundDistReward;
    private float headStraightReward;
    private float minimizeTorsoXZVelocityReward;
    private float torsoFromBaseOverMeanOfFeetsYReward;
    private float torsoOverCOMXZReward;
    

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

    private readonly int rewards = 6;

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

        baseDistanceFeetsTorso =
            Math.Abs((namedParts["rfoot_end"].transform.position - namedParts["torso"].transform.position).y);

        if (debug)
            InvokeRepeating("log", 0.0f, .1f);
    }


    public float COMOverMeanOfFeetsXZ()
    {
        // dist = 0, reward = 1000; dist = 1, reward = 0
        float reward = 0;
        var COM = physics.getCenterOfMass(bodyParts.getRigids());
        var COMXY = new Vector2(COM.x, COM.y);
        var feetsMean = meanOfFeets();
        if (COM.y < feetsMean.y)
            return 0;
        var feetsMeanXY = new Vector2(feetsMean.x, feetsMean.y);
        var distance = Vector2.Distance(COMXY, feetsMeanXY);

        reward = distance;
        COMOverMeanOfFeetsXZReward = reward;
        return reward;
    }

    public float torsoOverCOMXZ()
    {
        // dist = 0, reward = 1000; dist = 1, reward = 0
        float reward = 0;
        var COM = physics.getCenterOfMass(bodyParts.getRigids());
        var COMXY = new Vector2(COM.x, COM.y);
        var torso = namedParts["torso"].transform.position;
        if (COM.y > torso.y)
            return 0;
        var torsoXY = new Vector2(torso.x, torso.y);
        var distance = Vector2.Distance(COMXY, torsoXY);

        reward = distance;
        torsoOverCOMXZReward = reward;
        return reward;
    }

    public float torsoFromBaseOverMeanOfFeetsY()
    {
        // dist = 1, reward = 1000; dist = 0, reward = 0
        float reward = 0;
        var distPrec = distanceFeetsTorso() / baseDistanceFeetsTorso;

        reward = distPrec;
        torsoFromBaseOverMeanOfFeetsYReward = reward;
        return reward;
    }

    public float minimizeTorsoXZVelocity()
    {
        
        var torsoRigid = bodyParts.getNamedRigids()["torso"];
        var sumVelX = torsoRigid.velocity.x;

        reward = -Mathf.Abs(sumVelX);
        minimizeTorsoXZVelocityReward = reward;
        return reward;
    }

    public Vector3 meanOfFeets()
    {
        Vector3 rfootPos = namedParts["rfoot_end"].transform.position;
        Vector2 rfoot= new Vector2(rfootPos.x, rfootPos.y);
        Vector3 lfootPos = namedParts["lfoot_end"].transform.position;
        Vector2 lfoot= new Vector2(lfootPos.x, lfootPos.y);
        return (rfoot + lfoot) / 2;
    }

    public float distanceFeetsTorso()
    {
        return Math.Abs((meanOfFeets() - namedParts["torso"].transform.position).y);
    }

    public float getReward()
    {
        COMOverMeanOfFeetsXZ();
        torsoOverCOMXZ();
        torsoFromBaseOverMeanOfFeetsY();
        minimizeTorsoXZVelocity();
        reward = COMOverMeanOfFeetsXZReward + torsoOverCOMXZReward + torsoFromBaseOverMeanOfFeetsYReward +
                 minimizeTorsoXZVelocityReward;
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