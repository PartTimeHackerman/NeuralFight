using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class Arena : MonoBehaviour
{
    private Fighter PlayerFighter;
    private Fighter EnemyFighter;

    public FightersCollection FightersCollection;

    public EnemyFighterLoader EnemyFighterLoader;
    public FighterArenaSetter FighterArenaSetter;
    public ArenaFighterChooser ArenaFighterChooser;

    public PlayerHUD PlayerHUD;
    public PlayerHUD EnemyHUD;
    public Button StartButton;
    public Button SetUpButton;
    public ArenaUIAnimations ArenaUiAnimations;
    public Walls Walls;
    public ArenaCameraFollow ArenaCameraFollow;

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

    public int Round = 1;

    private void Start()
    {
        StartButton.onClick.AddListener(StartFight);
        SetUpButton.onClick.AddListener(SetUpArena);
        ArenaFighterChooser.OnChooseFighter += f =>
        {
            PlayerFighter = f;
            SetUpPlayer(PlayerFighter);
        };
    }

    public void SetUpArena()
    {
        SetUpButton.GetComponent<Image>().color = new HSBColor(.5f, 1, 1).ToColor();

        ArenaPlayer = new ArenaPlayer(this, "Player", FightersCollection.Fighters, PlayerHUD);
        ArenaEnemy = new ArenaPlayer(this, "Enemy", EnemyFighterLoader.EnemyFighters, EnemyHUD);
        ArenaPlayer.IsPlayer = true;
        ArenaUiAnimations.SetUp();
        SetUpRound();
    }

    public void SetUpRound()
    {
        PlayerFighter = ArenaPlayer.GetFirstAvaiableFighter();
        EnemyFighter = ArenaEnemy.GetFirstAvaiableFighter();

        Transform PlayerButt = PlayerFighter.BodyParts.root.transform;
        Transform EnemyButt = EnemyFighter.BodyParts.root.transform;
        ArenaCameraFollow.SetTransforms(PlayerButt, EnemyButt);
        SetUpPlayer(PlayerFighter);
        SetUpEnemy(EnemyFighter);
    }

    public void SetUpPlayer(Fighter fighter)
    {
        FighterArenaSetter.SetPlayerFighterPos(fighter);
        PlayerHUD.SetFighter(ArenaPlayer.PlayerName, fighter);
        CurrentPlayerFighter = fighter;
    }

    public void SetUpEnemy(Fighter fighter)
    {
        FighterArenaSetter.SetEnemyFighterPos(fighter);
        EnemyHUD.SetFighter(ArenaEnemy.PlayerName, fighter);
        CurrentEnemyFighter = fighter;
    }

    public void StartFight()
    {
        ArenaUiAnimations.StartArena();
        ArenaUiAnimations.StartRound(Round);

        Action startFight = () =>
        {
            ArenaUiAnimations.EnableTimer(true);
            PlayerFighter.SetEnemy(EnemyFighter);
            EnemyFighter.SetEnemy(PlayerFighter);
            PlayerFighter.StartFight();
            EnemyFighter.StartFight();
            Walls.ResetWalls();
            Walls.StartWalls();
            ArenaCameraFollow.StartFollow();
        };
        Waiter.Get().WaitForSecondsC(2f, () => { }, startFight);
    }


    private void PlayerDied(Player player)
    {
        Fighter playerFighter = player.Fighter;
        //playerFighter.ResetFighter();
        //playerFighter.Player.ResetPlayer();
        ArenaPlayer.FighterLost(playerFighter);
        ArenaEnemy.FighterWon(CurrentEnemyFighter);
        RoundEnded(ArenaEnemy, ArenaPlayer);
    }

    private void EnemyDied(Player enemy)
    {
        Fighter enemyFighter = enemy.Fighter;
        //enemyFighter.ResetFighter();
        ArenaEnemy.FighterLost(enemyFighter);
        ArenaPlayer.FighterWon(CurrentPlayerFighter);
        RoundEnded(ArenaPlayer, ArenaEnemy);
    }

    private void RoundEnded(ArenaPlayer winner, ArenaPlayer loser)
    {
        ArenaUiAnimations.EnableTimer(false);
        Fighter winnerFighter = winner.CurrentFighter;
        Fighter loserFighter = loser.CurrentFighter;
        winnerFighter.StopFight();
        loserFighter.StopFight();
        loserFighter.Player.ResetPlayer();
        winnerFighter.Player.SP = winnerFighter.Player.MaxSP;
        Walls.StopWalls();
        ArenaCameraFollow.ResetPosition();
        
        if (ArenaPlayer.HasAnyAvaiableFighters() && ArenaEnemy.HasAnyAvaiableFighters())
        {
            ArenaUiAnimations.EndRound(Round, winner.PlayerName);
            Action roundEnded = () =>
            {
                winnerFighter.ResetFighter();
                loserFighter.ResetFighter();
                Round++;
                SetUpRound();
                StartFight();
            };

            Waiter.Get().WaitForSecondsC(2f, () => { }, roundEnded);
        }
        else
        {
            ArenaPlayer matchWinner;
            ArenaPlayer matchLoser;
            matchWinner = ArenaPlayer.WonFights == 3 ? ArenaPlayer : ArenaEnemy;
            matchLoser = matchWinner == ArenaPlayer ? ArenaEnemy : ArenaPlayer;
            MatchEnded(matchWinner, matchLoser);
        }
    }

    public void MatchEnded(ArenaPlayer winner, ArenaPlayer loser)
    {
        ArenaUiAnimations.EndMatch(winner.PlayerName);
        Debug.LogFormat("Arena match ended {2} : {3}, Winner: {0}, Rounds: {1}", winner.PlayerName, Round,
            winner.WonFights, loser.WonFights);
    }
}