using UnityEngine;

public class FightPlayerReward : MonoBehaviour
{
    private BodyParts BodyParts;
    private BodyParts EnemyBodyParts;
    private Player Player;
    private Player EnemyPlayer;
    public Walls Walls;
    private ForwardReward forwardReward;
    private StayInMiddleReward stayInMiddleReward;
    private EnemyInMiddleReward enemyInMiddleReward;

    public float forwardRewardVal;
    public float stayInMiddleRewardVal;
    public float enemyInMiddleRewardVal;
    public float standingRewardVal;
    public float HpRewardVal;
    public float enemyHpRewardVal;
    public float timeRewardVal;
    public float inFrontOfEnemyVal;
    public float distToEnemyVal;
    public float reward;

    public bool debug = false;
    private StandingRewardHumanoid standingReward;

    private void Start()
    {
        
    }

    public void SetUp(FightPlayerAgent player, FightPlayerAgent enemy)
    {
        Player = player.PlayerFighter.Player;
        EnemyPlayer = enemy.PlayerFighter.Player;
        BodyParts = player.PlayerFighter.BodyParts;
        EnemyBodyParts = enemy.PlayerFighter.BodyParts;
        
        standingReward = new StandingRewardHumanoid(BodyParts);
        //forwardReward = new ForwardReward(BodyParts, EnemyBodyParts);
        stayInMiddleReward = new StayInMiddleReward(BodyParts, 10f);
        enemyInMiddleReward = new EnemyInMiddleReward(EnemyBodyParts, 10f);
        standingReward.multipler = new[] {1f, 1f, 1f, 0, 0};
        standingReward.Init();
        if (debug)
            InvokeRepeating("getReward", 0.0f, .1f);
    }
    
    public float getReward()
    {
        stayInMiddleRewardVal = stayInMiddleReward.getReward(Walls.getCurrentPos());
        enemyInMiddleRewardVal = enemyInMiddleReward.getReward(Walls.getCurrentPos());
        //forwardRewardVal = forwardReward.getReward() * (enemyInMiddleRewardVal + Mathf.Abs(stayInMiddleRewardVal - 1f));
        HpRewardVal = Player.hp / Player.MaxHP;
        enemyHpRewardVal = Mathf.Abs((EnemyPlayer.hp / EnemyPlayer.MaxHP) - 1f);
        standingRewardVal = standingReward.getReward();
        timeRewardVal = -Mathf.Min(GameTimer.get().Elapsed / 30f, 1f);
        inFrontOfEnemyVal = inFrontOfEnemy();
        distToEnemyVal = Mathf.Abs(Vector3.Distance(BodyParts.root.position, EnemyBodyParts.root.position) /
                         (Walls.getCurrentPos() * 2f) - 1f);
        reward = stayInMiddleRewardVal + enemyInMiddleRewardVal + HpRewardVal + enemyHpRewardVal +
                 standingRewardVal + timeRewardVal + inFrontOfEnemyVal + distToEnemyVal;
        return reward;
    }

    private float inFrontOfEnemy()
    {
        if (Player.left)
        {
            return BodyParts.root.position.x < EnemyBodyParts.root.position.x ? 1f : 0f;
        }
        else
        {
            return BodyParts.root.position.x > EnemyBodyParts.root.position.x ? 1f : 0f;
        }
    }

    public float getPlayerHpSpRewards()
    {
        float sum = 0f;
        sum += Player.hp / Player.MaxHP;
        return sum;
    }
}