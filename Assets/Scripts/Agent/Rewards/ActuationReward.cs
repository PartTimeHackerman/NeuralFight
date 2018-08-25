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
    private List<float> lastVels;
    private List<float> newVels = new List<float>();

    public ActuationReward(BodyParts bodyParts)
    {
        this.bodyParts = bodyParts;
    }

    public float getReward()
    {
        List<JointInfo> jointInfos = bodyParts.jointsInfos;
        float maxVel = jointInfos[0].maxVel * 2;
        float sumVelDiff = 0f;
        float jointsCount = jointInfos.Count;
        newVels.Clear();

        foreach (JointInfo jointInfo in jointInfos)
            newVels.Add(jointInfo.configurableJoint.targetAngularVelocity.x);

        if (lastVels == null)
            lastVels = new List<float>(newVels);

        if (!listsEqual(lastVels, newVels))
        {
            if (lastVels.Count != newVels.Count)
                lastVels = newVels;

            for (int i = 0; i < newVels.Count; i++)
            {
                sumVelDiff += Mathf.Abs(lastVels[i] - newVels[i]) / maxVel;
            }
            reward = Mathf.Abs((sumVelDiff / jointsCount) - 1f);
        }

        lastVels = new List<float>(newVels);
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
