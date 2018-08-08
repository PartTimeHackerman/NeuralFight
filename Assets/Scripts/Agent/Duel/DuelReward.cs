using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DuelReward : MonoBehaviour
{
    public bool debug = false;
    public BodyParts enemyBodyParts;
    private StandingRewardHumanoid standingReward;
    private ForwardReward forwardReward;
    private ActuationReward actuationReward;
    private StayInMiddleReward stayInMiddleReward;
    private EnemyInMiddleReward enemyInMiddleReward;
    private BodyParts bodyParts;

    public float standingRewardVal;
    public float forwardRewardVal;
    public float actuationRewardVal;
    public float stayInMiddleRewardVal;
    public float enemyInMiddleRewardVal;
    public float reward;


    private void Start()
    {
        bodyParts = GetComponent<BodyParts>();
        standingReward = new StandingRewardHumanoid(bodyParts);
        forwardReward = new ForwardReward(bodyParts, enemyBodyParts);
        actuationReward = new ActuationReward(bodyParts);
        stayInMiddleReward = new StayInMiddleReward(bodyParts, 4f);
        enemyInMiddleReward = new EnemyInMiddleReward(enemyBodyParts, 4f);

        standingReward.Init();

        if (debug)
            InvokeRepeating("getReward", 0.0f, .1f);
    }

    public float getReward()
    {
        standingRewardVal = standingReward.getReward() * 3f;
        actuationRewardVal = actuationReward.getReward() * 2;
        stayInMiddleRewardVal = stayInMiddleReward.getReward();
        enemyInMiddleRewardVal = enemyInMiddleReward.getReward();
        forwardRewardVal = forwardReward.getReward() * (enemyInMiddleRewardVal + Mathf.Abs(stayInMiddleRewardVal - 1f));
        reward = standingRewardVal + forwardRewardVal + actuationRewardVal + stayInMiddleRewardVal + enemyInMiddleRewardVal;
        return reward;
    }

}

