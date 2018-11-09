using UnityEngine;

public class FightPlayerReward : MonoBehaviour
{
    
    public BodyParts BodyParts;
    public BodyParts EnemyBodyParts;
    public Player Player;
    private ForwardReward forwardReward;
    private StayInMiddleReward stayInMiddleReward;
    private EnemyInMiddleReward enemyInMiddleReward;

    public float forwardRewardVal;
    public float stayInMiddleRewardVal;
    public float enemyInMiddleRewardVal;
    public float reward;

    public bool debug = false;

    private void Start()
    {
        forwardReward = new ForwardReward(BodyParts, EnemyBodyParts);
        stayInMiddleReward = new StayInMiddleReward(BodyParts, 10f);
        enemyInMiddleReward = new EnemyInMiddleReward(EnemyBodyParts, 10f);

        if (debug)
            InvokeRepeating("getReward", 0.0f, .1f);
    }

    public float getReward()
    {
        stayInMiddleRewardVal = stayInMiddleReward.getReward();
        enemyInMiddleRewardVal = enemyInMiddleReward.getReward();
        forwardRewardVal = forwardReward.getReward() * (enemyInMiddleRewardVal + Mathf.Abs(stayInMiddleRewardVal - 1f));
        reward = forwardRewardVal + stayInMiddleRewardVal + enemyInMiddleRewardVal + getPlayerHpSpRewards();
        return reward;
    }

    public float getPlayerHpSpRewards()
    {
        float sum = 0f;
        sum += Player.hp / Player.MaxHP;
        sum += Player.hp / Player.CurrentMaxHP;
        sum += Player.sp / Player.MaxSP;
        sum += Player.sp / Player.CurrentMaxSP;
        sum += Player.HpRegen / Player.MaxHP;
        sum += Player.SpRegen / Player.MaxSP;
        return sum;
    }
}