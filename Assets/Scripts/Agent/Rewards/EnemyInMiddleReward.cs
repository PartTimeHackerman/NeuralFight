using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class EnemyInMiddleReward
{
    private float reward;
    private Vector3 COM;
    private float arenaLen;
    private BodyParts bodyParts;
    private PhysicsUtils physicsUtils;

    public EnemyInMiddleReward(BodyParts bodyParts, float arenaLen)
    {
        this.bodyParts = bodyParts;
        this.physicsUtils = PhysicsUtils.get();
        this.arenaLen = arenaLen;
    }

    public float getReward()
    {
        COM = physicsUtils.getCenterOfMass(bodyParts.getRigids());
        reward = Mathf.Abs(COM.x) / arenaLen;
        return reward;
    }
    
    public float getReward(float arenaLen)
    {
        COM = bodyParts.root.position;
        reward = Mathf.Abs(COM.x) / arenaLen;
        return reward;
    }
}
