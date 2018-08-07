using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ActuationReward
{
    private BodyParts bodyParts;
    public float reward;

    public ActuationReward(BodyParts bodyParts)
    {
        this.bodyParts = bodyParts;
    }

    public float getReward()
    {
        List<JointInfo> jointInfos = bodyParts.jointsInfos;
        float maxVel = jointInfos[0].maxVel;
        int count = jointInfos.Count;
        float sumVels = 0;
        foreach (JointInfo jointInfo in jointInfos)
        {
            sumVels += Mathf.Abs(jointInfo.configurableJoint.targetAngularVelocity.x);
        }

        reward = Mathf.Abs((sumVels / count) / maxVel - 1);
        return reward;
    }
}
