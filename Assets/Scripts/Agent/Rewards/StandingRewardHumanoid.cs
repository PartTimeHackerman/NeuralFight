using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Agent;
using UnityEngine;

public class StandingRewardHumanoid : IReward
{
    private float maxDistanceTorsoFeets;
    private float baseDistanceCOMTorso;
    private float baseDistanceFeetsCOM;
    private BodyParts bodyParts;
    private Dictionary<string, GameObject> namedParts;
    private PhysicsUtils physics;

    public float reward;

    private float lastClearReward = 0;
    public float COMOverMeanOfFeetsXZReward;
    public float minimizeTorsoXZVelocityReward;
    public float torsoFromBaseOverMeanOfFeetsYReward;
    public float torsoOverCOMXZReward;
    public float minimizeActuationReward;
    

    private Vector3 COM;

    public StandingRewardHumanoid(BodyParts bodyParts)
    {
        this.bodyParts = bodyParts;
        physics = PhysicsUtils.get();
    }

    public void Init()
    {
        namedParts = bodyParts.getNamedParts();
        
        baseDistanceCOMTorso = namedParts["torso"].transform.position.y - physics.getCenterOfMass(bodyParts.getRigids()).y;
        baseDistanceFeetsCOM = physics.getCenterOfMass(bodyParts.getRigids()).y - meanOfFeets().y;
        maxDistanceTorsoFeets = calcDistance(namedParts["torso"], namedParts["rfoot_end"]);
    }


    public float COMOverMeanOfFeetsZ()
    {
        var feetsMean = meanOfFeets();
        float distPrec = 0;
        distPrec = Mathf.Abs(Mathf.Abs(feetsMean.x - COM.x) / baseDistanceFeetsCOM - 1);
        COMOverMeanOfFeetsXZReward = RewardFunctions.toleranceInvNoBounds(Mathf.Clamp(distPrec, 0f, 1f), .4f, .1f, RewardFunction.LONGTAIL);
        if (COM.y < feetsMean.y)
            COMOverMeanOfFeetsXZReward *= -1;
        return distPrec;
    }

    public float torsoOverCOMXZ()
    {
        Vector3 torso = namedParts["torso"].transform.position;
        float distPrec = 0;
        distPrec = Mathf.Abs(Mathf.Abs(COM.x - torso.x) / baseDistanceCOMTorso - 1);
        torsoOverCOMXZReward = RewardFunctions.toleranceInvNoBounds(Mathf.Clamp(distPrec, 0f, 1f), .4f, .1f, RewardFunction.LONGTAIL);
        if (torso.y < COM.y)
            torsoOverCOMXZReward *= -1;
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
        distYPrec = Mathf.Abs(namedParts["torso"].transform.position.y - meanOfFeets().y) / maxDistanceTorsoFeets;
        torsoFromBaseOverMeanOfFeetsYReward = RewardFunctions.toleranceInvNoBounds(Mathf.Clamp(distYPrec, 0f, 1f), .4f, .1f, RewardFunction.LONGTAIL);
        if (namedParts["torso"].transform.position.y < meanOfFeets().y)
            torsoFromBaseOverMeanOfFeetsYReward *= -1;
        return distYPrec;
    }

    public float minimizeTorsoXZVelocity()
    {

        var torsoRigid = bodyParts.getNamedRigids()["torso"];
        var sumVel = Math.Abs(torsoRigid.velocity.x) + Math.Abs(torsoRigid.velocity.y);
        float maxVel = 10;
        reward = Mathf.Clamp((Mathf.Abs(sumVel / maxVel - 1)), 0f, 1f);
        minimizeTorsoXZVelocityReward = RewardFunctions.toleranceInvNoBounds(reward, .4f, .1f, RewardFunction.LONGTAIL);
        return reward;
    }

    public Vector2 meanOfFeets()
    {
        Vector3 rfootPos = namedParts["rfoot_end"].transform.position;
        Vector2 rfoot = new Vector2(rfootPos.x, rfootPos.y);
        Vector3 lfootPos = namedParts["lfoot_end"].transform.position;
        Vector2 lfoot = new Vector2(lfootPos.x, lfootPos.y);
        return (rfoot + lfoot) / 2;
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
        COM = physics.getCenterOfMass(bodyParts.getRigids());
        COMOverMeanOfFeetsZ();
        torsoOverCOMXZ();
        torsoFromBaseOverMeanOfFeetsY();
        minimizeTorsoXZVelocity();
        minimizeActuation();
        reward = (COMOverMeanOfFeetsXZReward +
                  torsoOverCOMXZReward +
                  torsoFromBaseOverMeanOfFeetsYReward +
                  minimizeTorsoXZVelocityReward +
                  minimizeActuationReward) / 5f ;
        reward = Mathf.Clamp(reward, -1f, 1f);
        return reward;
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
