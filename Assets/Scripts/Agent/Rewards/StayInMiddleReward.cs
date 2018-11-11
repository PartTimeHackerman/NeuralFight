using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class StayInMiddleReward
{
    private float reward;
    private Vector3 COM;
    private float arenaLen;
    private BodyParts bodyParts;
    private PhysicsUtils physicsUtils;

    public StayInMiddleReward(BodyParts bodyParts, float arenaLen)
    {
        this.bodyParts = bodyParts;
        this.physicsUtils = PhysicsUtils.get();
        this.arenaLen = arenaLen;
    }

    public float getReward()
    {
        COM = bodyParts.root.position;//physicsUtils.getCenterOfMass(bodyParts.getRigids());

        if (Mathf.Abs(COM.x) > arenaLen)
            return 0f;

        reward = Mathf.Abs(Mathf.Abs(COM.x) / arenaLen - 1);
        return reward;

    }
    
    public float getReward(float arenaLen)
    {
        COM = bodyParts.root.position;//physicsUtils.getCenterOfMass(bodyParts.getRigids());

        if (Mathf.Abs(COM.x) > arenaLen)
            return 0f;

        reward = Mathf.Abs(Mathf.Abs(COM.x) / arenaLen - 1);
        return reward;

    }


}
