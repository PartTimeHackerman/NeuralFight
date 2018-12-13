using UnityEngine;

public class ArenaMatch
{
    public Fighter currentPlayerFighter;

    public Fighter CurrentPlayerFighter
    {
        get { return currentPlayerFighter; }
        set
        {
            if (currentPlayerFighter != null) currentPlayerFighter.Player.OnDied -= PlayerDied;
            currentPlayerFighter = value;
            CurrentPlayerFighter.Player.OnDied += PlayerDied;
        }
    }

    public Fighter currentEnemyFighter;

    public Fighter CurrentEnemyFighter
    {
        get { return currentEnemyFighter; }
        set
        {
            if (currentEnemyFighter != null) currentEnemyFighter.Player.OnDied -= EnemyDied;
            currentEnemyFighter = value;
            CurrentEnemyFighter.Player.OnDied += EnemyDied;
        }
    }

    private ArenaPlayer arenaPlayer;

    public ArenaPlayer ArenaPlayer
    {
        get { return arenaPlayer; }
        set { arenaPlayer = value; }
    }

    private ArenaPlayer arenaEnemy;

    public ArenaPlayer ArenaEnemy
    {
        get { return arenaEnemy; }
        set { arenaEnemy = value; }
    }

    public int Round = 0;

    private void PlayerDied(Player player)
    {
        ArenaPlayer.FighterLost(player.Fighter);
        ArenaEnemy.FighterWon(CurrentEnemyFighter);
        RoundEnded(CurrentEnemyFighter, CurrentPlayerFighter);
    }

    private void EnemyDied(Player enemy)
    {
        ArenaEnemy.FighterLost(enemy.Fighter);
        ArenaPlayer.FighterWon(CurrentPlayerFighter);
        RoundEnded(CurrentPlayerFighter, CurrentEnemyFighter);
    }

    private void RoundEnded(Fighter winner, Fighter loser)
    {
        Round++;
        
    }

    public void MatchEnded(ArenaPlayer winner)
    {
        ArenaPlayer loser = ArenaPlayer == winner ? ArenaEnemy : ArenaPlayer;
        
        Debug.LogFormat("Arena match ended {2} : {3}, Winner: {0}, Rounds: {1}", winner.PlayerName, Round,
            winner.WonFights, loser.WonFights);
    }
}