using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class WalkFWReward : MonoBehaviour
{

    public bool debug = false;
    private StandingRewardHumanoid standingReward;
    private VelocityReward velocityFWReward;
    private VelocityReward velocityUPReward;
    private BodyParts bodyParts;
    public float reward;
    public float standingRewardVal;
    public float velocityRewardVal;
    
    

    private void Start()
    {
        bodyParts = GetComponent<BodyParts>();
        standingReward = new StandingRewardHumanoid(bodyParts);
        velocityFWReward = new VelocityReward(Vector2.right, 10f, bodyParts.root);
        velocityUPReward = new VelocityReward(Vector2.up, 10f, bodyParts.root);

        standingReward.multipler = new[] {1f, 1f, 0f, 0f, 0f};

        standingReward.Init();

        if (debug)
            InvokeRepeating("getReward", 0.0f, .1f);
    }

    public float getReward()
    {
        standingRewardVal = standingReward.getReward();
        velocityRewardVal = velocityFWReward.getReward() - Mathf.Abs(velocityUPReward.getReward());
        reward = (standingRewardVal + velocityRewardVal * 3f) / 4f;
        
        return reward;
    }
}
