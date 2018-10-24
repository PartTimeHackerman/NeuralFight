using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ActuationReward
{
    private BodyParts bodyParts;
    public float reward = 0f;
    public bool vel = false;
    private List<float> lastRot;
    private List<float> newRot = new List<float>();
    private List<float> limitsSum;

    public ActuationReward(BodyParts bodyParts, bool vel)
    {
        this.vel = vel;
        this.bodyParts = bodyParts;
        if (!vel)
        {
            limitsSum = new List<float>();
            foreach (JointInfo jointInfo in bodyParts.jointsInfos)
            {
                limitsSum.Add(Mathf.Abs(jointInfo.joint.lowAngularXLimit.limit) +
                              jointInfo.joint.highAngularXLimit.limit);
            }
        }
    }

    public float getReward()
    {
        if (vel)
            return getRewardVel();
        else
            return getRewardTargetRotation();
    }

    public float getRewardVel()
    {
        List<JointInfo> jointInfos = bodyParts.jointsInfos;
        float maxVel = jointInfos[0].maxVel * 2;
        float sumVelDiff = 0f;
        float jointsCount = jointInfos.Count;
        newRot.Clear();

        foreach (JointInfo jointInfo in jointInfos)
            newRot.Add(jointInfo.configurableJoint.targetAngularVelocity.x);

        if (lastRot == null)
            lastRot = new List<float>(newRot);

        if (!listsEqual(lastRot, newRot))
        {
            if (lastRot.Count != newRot.Count)
                lastRot = newRot;

            for (int i = 0; i < newRot.Count; i++)
            {
                sumVelDiff += Mathf.Abs(lastRot[i] - newRot[i]) / maxVel;
            }

            reward = Mathf.Abs((sumVelDiff / jointsCount) - 1f);
        }

        lastRot = new List<float>(newRot);
        return reward;
    }

    public float getRewardTargetRotation()
    {
        List<JointInfo> jointInfos = bodyParts.jointsInfos;
        float sumVelDiff = 0f;
        float jointsCount = jointInfos.Count;
        newRot.Clear();

        foreach (JointInfo jointInfo in jointInfos)
            newRot.Add(jointInfo.currentRot);

        if (lastRot == null)
            lastRot = new List<float>(newRot);

        if (!listsEqual(lastRot, newRot))
        {
            if (lastRot.Count != newRot.Count)
                lastRot = newRot;

            for (int i = 0; i < newRot.Count; i++)
            {
                sumVelDiff += (Mathf.Abs(Mathf.DeltaAngle(lastRot[i], newRot[i])) / limitsSum[i]) * jointInfos[i].currentForce;
            }

            reward = Mathf.Abs((sumVelDiff / jointsCount) - 1f);
        }

        lastRot = new List<float>(newRot);
        return reward;
    }

    private bool listsEqual(List<float> l1, List<float> l2)
    {
        if (l1.Count != l2.Count)
            return false;

        for (int i = 0; i < l1.Count; i++)
        {
            if (!l1[i].Equals(l2[i]))
                return false;
        }

        return true;
    }
}