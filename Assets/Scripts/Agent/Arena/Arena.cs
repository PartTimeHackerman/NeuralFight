using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class Arena : MonoBehaviour
{
    private Fighter PlayerFighter;
    private Fighter EnemyFighter;

    public FightersCollection FightersCollection;
    public FightersCollection EnemyFightersCollection;

    public FighterArenaSetter FighterArenaSetter;
    public ArenaFighterChooser ArenaFighterChooser;

    public PlayerHUD PlayerHUD;
    public PlayerHUD EnemyHUD;
    public Button StartButton;
    public Button BackButton;
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

    private ArenaPlayer arenaPlayer = new ArenaPlayer();

    public ArenaPlayer ArenaPlayer
    {
        get { return arenaPlayer; }
        set { arenaPlayer = value; }
    }

    private ArenaPlayer arenaEnemy = new ArenaPlayer();

    public ArenaPlayer ArenaEnemy
    {
        get { return arenaEnemy; }
        set { arenaEnemy = value; }
    }

    public int Round = 1;
    public Canvas ArenaUICanvas;


    public static event MatchEnd OnMatchEnd;

    public delegate void MatchEnd(UserPlayer winner, UserPlayer loser);

    public static event ExitArena OnArenaExit;

    public delegate void ExitArena();

    public UserPlayer Player;
    public UserPlayer Enemy;

    private void Start()
    {
        StartButton.onClick.AddListener(StartFight);
        //SetUpButton.onClick.AddListener(SetUpArena);
        BackButton.onClick.AddListener(Endfight);
        ArenaFighterChooser.OnChooseFighter += f =>
        {
            PlayerFighter = f;
            SetUpPlayer(PlayerFighter);
        };
    }


    public void SetUpArena(UserPlayer player, UserPlayer enemy)
    {
        Player = player;
        Enemy = enemy;
        SetUpButton.GetComponent<Image>().color = new HSBColor(.5f, 1, 1).ToColor();

        ArenaPlayer.SetPlayer(this, Player, FightersCollection.Fighters, PlayerHUD);
        ArenaEnemy.SetPlayer(this, Enemy, EnemyFightersCollection.Fighters, EnemyHUD);
        ArenaPlayer.IsPlayer = true;
        ArenaUiAnimations.SetUp();
        SetUpRound();
    }

    public void SetUpRound()
    {
        PlayerFighter = ArenaPlayer.GetFirstAvaiableFighter();
        EnemyFighter = ArenaEnemy.GetFirstAvaiableFighter();
        PlayerFighter.Player.SP = PlayerFighter.Player.MaxSP;
        EnemyFighter.Player.SP = EnemyFighter.Player.MaxSP;
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
        winnerFighter.StopFightWinner();
        loserFighter.StopFight();
        //loserFighter.Player.ResetPlayer();
        winnerFighter.Player.SP = winnerFighter.Player.MaxSP;
        Walls.StopWalls();

        if (ArenaPlayer.HasAnyAvaiableFighters() && ArenaEnemy.HasAnyAvaiableFighters())
        {
            ArenaUiAnimations.EndRound(Round, winner.PlayerName);

            Action roundEnded = () =>
            {
                
                Walls.ResetWalls();
                ArenaCameraFollow.ResetPosition();
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
            winnerFighter.StopFight();
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
        if (!winner.Player.PlayerID.Equals(loser.Player.PlayerID))
        {
            PlayerEvents.MatchEnd(winner.Player, loser.Player, winner.WonFights, loser.WonFights);
            AddItem(winner.Player, loser.Player);
            
        }
        OnMatchEnd?.Invoke(winner.Player, loser.Player);
    }


    private void Endfight()
    {
        ArenaCameraFollow.StopFollow();
        ArenaUiAnimations.ExitArena();
        CleanUp();
        OnArenaExit?.Invoke();
    }

    private void CleanUp()
    {
        
        Round = 1;
        Walls.ResetWalls();
        foreach (KeyValuePair<FighterNum, Fighter> keyValuePair in ArenaPlayer.Fighters)
        {
            keyValuePair.Value.ResetFighterTotal();
        }

        foreach (KeyValuePair<FighterNum, Fighter> keyValuePair in ArenaEnemy.Fighters)
        {
            keyValuePair.Value.ResetFighterTotal();
        }
    }

    private void AddItem(UserPlayer winner, UserPlayer loser)
    {
        bool playerWins = winner.PlayerID.Equals(Auth.UserId);
        Item item = null;
        if (playerWins)
        {
            item = ItemsGenerator.Get().GenerateItem(EnemyCollections.Get().AverageItemsLevel);
        }
        else
        {
            item = ItemsGenerator.Get().GenerateItem(PlayerCollections.Get().AverageItemsLevel);
        }

        if (item.GetType() == typeof(Weapon))
        {
            PlayerCollections.Get().PlayerWeaponsCollection.AddWeapon((Weapon) item);
            Storage.SaveWeapon((Weapon) item);
        }
        else
        {
            PlayerCollections.Get().PlayerFighterPartsCollection.AddFighterPart((FighterPart) item);
            Storage.SaveFighterPart((FighterPart) item);
        }
        ArenaUiAnimations.ReceiveItem(item);
    }
}