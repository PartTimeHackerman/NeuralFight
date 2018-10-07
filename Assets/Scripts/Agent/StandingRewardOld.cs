using System;
using System.Collections.Generic;
using UnityEngine;

public class StandingRewardOld : MonoBehaviour
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

    private void Start()
    {
        bodyParts = GetComponent<BodyParts>();
        namedParts = bodyParts.getNamedParts();
        physics = PhysicsUtils.get();

        baseDistanceFeetsTorso =
            Math.Abs((namedParts["rfoot"].transform.position - namedParts["torso"].transform.position).y);

        if(debug)
            InvokeRepeating("log", 0.0f, .1f);

        unpenaltySpeed = penaltySpeed / 3;
        penaltyPerPoint = maxPenalty / maxPenaltyCount;
        penaltyAdd = penaltySpeed * maxPenaltyCount / maxPenaltyCount;
        unpenaltyAdd = unpenaltySpeed / maxPenaltyCount;
    }

    public float COMOverMeanOfFeetsXZ()
    {
        // dist = 0, reward = 1000; dist = 1, reward = 0
        float reward = 0;
        var COM = physics.getCenterOfMass(bodyParts.getRigids());
        var COMXZ = new Vector2(COM.x, COM.z);
        var feetsMean = meanOfFeets();
        if (COM.y < feetsMean.y)
            return 0;
        var feetsMeanXZ = new Vector2(feetsMean.x, feetsMean.z);
        var distance = Vector2.Distance(COMXZ, feetsMeanXZ);

        reward = (int) (-0.9872102 + 1000.987 * Math.Pow(Math.E, -6.921614 * distance));
        if (reward < 0) reward = 0;
        if (reward > 1000) reward = 1000;
        reward /= 1000f;
        COMOverMeanOfFeetsXZReward = reward;
        return reward;
    }

    public float torsoOverCOMXZ()
    {
        // dist = 0, reward = 1000; dist = 1, reward = 0
        float reward = 0;
        var COM = physics.getCenterOfMass(bodyParts.getRigids());
        var COMXZ = new Vector2(COM.x, COM.z);
        var torso = namedParts["torso"].transform.position;
        if (COM.y > torso.y)
            return 0;
        var torsoXZ = new Vector2(torso.x, torso.z);
        var distance = Vector2.Distance(COMXZ, torsoXZ);

        reward = (int) (-0.9872102 + 1000.987 * Math.Pow(Math.E, -6.921614 * distance));
        if (reward < 0) reward = 0;
        if (reward > 1000) reward = 1000;
        reward /= 1000f;
        torsoOverCOMXZReward = reward;
        return reward;
    }

    public float torsoFromBaseOverMeanOfFeetsY()
    {
        // dist = 1, reward = 1000; dist = 0, reward = 0
        float reward = 0;
        var distPrec = distanceFeetsTorso() / baseDistanceFeetsTorso;

        reward = (int) (-39.04754 + 39.04754 * Math.Pow(Math.E, 3.28128 * distPrec));
        if (reward < 0) reward = 0;
        if (reward > 1000) reward = 1000;
        reward /= 1000f;
        torsoFromBaseOverMeanOfFeetsYReward = reward;
        return reward;
    }

    public float minimizeTorsoXZVelocity()
    {
        // vel = 0, reward = 1000; dist = 1, reward = 0
        float reward = 0;
        var torsoRigid = bodyParts.getNamedRigids()["torso"];
        var sumVelXZ = torsoRigid.velocity.x + torsoRigid.velocity.z;

        reward = (int) (-0.9872102 + 1000.987 * Math.Pow(Math.E, -6.921614 * sumVelXZ));
        if (reward < 0) reward = 0;
        if (reward > 1000) reward = 1000;
        reward /= 1000f;
        minimizeTorsoXZVelocityReward = reward;
        return reward;
    }

    public float headStraight()
    {
        var headUpY = namedParts["head"].transform.up.y;
        if (headUpY < .5f)
            return 0;

        float reward = 0;
        headUpY = headUpY / .5f - 1;
        reward = (int) (-39.04754 + 39.04754 * Math.Pow(Math.E, 3.28128 * headUpY));
        if (reward < 0) reward = 0;
        if (reward > 1000) reward = 1000;
        reward /= 1000f;
        headStraightReward = reward;
        return reward;
    }

    public float headGroundDist()
    {
        float reward = 0;
        var headYPos = namedParts["head"].transform.position.y;
        var headFromGroundPrec = headYPos / 1.57f;

        reward = (int) (-39.04754 + 39.04754 * Math.Pow(Math.E, 3.28128 * headFromGroundPrec));
        if (reward < 0) reward = 0;
        if (reward > 1000) reward = 1000;
        reward /= 1000f;
        headGroundDistReward = reward;
        return reward;
    }

    public Vector3 meanOfFeets()
    {
        return (namedParts["rfoot"].transform.position + namedParts["lfoot"].transform.position) / 2;
    }

    public float distanceFeetsTorso()
    {
        return Math.Abs((meanOfFeets() - namedParts["torso"].transform.position).y);
    }


    public float getReward()
    {
        //COMOverMeanOfFeetsZ();
        //torsoOverCOMXZ();
        //rootFromBaseOverMeanOfFeetsY();
        minimizeTorsoXZVelocity();
        //headStraight();
        headGroundDist();
        clearReward = (
                  //COMOverMeanOfFeetsXZReward +
                  //torsoOverCOMXZReward +
                  //rootFromBaseOverMeanOfFeetsYReward +
                  minimizeTorsoXZVelocityReward +
                  //headStraightReward +
                  headGroundDistReward * 3
                                ) / 4;

        calcPenalty(clearReward);
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
        bool terminated = this.terminated() && penaltyCount >= maxPenaltyCount;
        if (terminated)
            penaltyCount = 0;
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