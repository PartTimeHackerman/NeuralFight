using UnityEngine;

public class CrouchingReward : StandingRewardHumanoid
{
    public CrouchingReward(BodyParts bodyParts) : base(bodyParts)
    {
    }

    public override float rootFromBaseOverMeanOfFeetsY()
    {
        float reward = base.rootFromBaseOverMeanOfFeetsY();
        reward = (reward - 0.5f) * 2f;
        reward = -Mathf.Abs(reward) + 1f;
        rootFromBaseOverMeanOfFeetsYReward = reward;
        return reward;
    }
}