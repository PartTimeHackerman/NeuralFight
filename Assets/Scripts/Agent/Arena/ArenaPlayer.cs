using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArenaPlayer
{
    public string PlayerName;
    public Arena Arena;
    public Dictionary<FighterNum, Fighter> Fighters = new Dictionary<FighterNum, Fighter>();
    public int WonFights = 0;
    public bool[] AvaiableFighters = new[] {true, true, true};
    public PlayerHUD PlayerHud;
    public bool IsPlayer = false;
    public Fighter CurrentFighter;

    public UserPlayer Player;
    
    /*
    public ArenaPlayer(Arena arena, UserPlayer userPlayer, Dictionary<FighterNum, Fighter> fighters, PlayerHUD playerHud)
    {
        Player = userPlayer;
        Arena = arena;
        PlayerName = userPlayer.UserName;
        Fighters = fighters;
        PlayerHud = playerHud;
        
        PlayerHud.Star1.enabled = false;
        PlayerHud.Star2.enabled = false;
        PlayerHud.Star3.enabled = false;
    }
    */

    public void SetPlayer(Arena arena, UserPlayer userPlayer, Dictionary<FighterNum, Fighter> fighters, PlayerHUD playerHud)
    {
        Player = userPlayer;
        Arena = arena;
        PlayerName = userPlayer.UserName;
        Fighters = fighters;
        PlayerHud = playerHud;

        WonFights = 0;
        PlayerHud.Star1.enabled = false;
        PlayerHud.Star2.enabled = false;
        PlayerHud.Star3.enabled = false;
        CurrentFighter = null;
        AvaiableFighters = new[] {true, true, true};
    }

    public bool HasAnyAvaiableFighters()
    {
        return AvaiableFighters.Contains(true);
    }

    public Fighter GetFirstAvaiableFighter()
    {
        Fighter fighter = null;

        if (!AvaiableFighters.Contains(true))
        {
            fighter = null;
        }
        else
        {
            if (AvaiableFighters[0])
            {
                fighter = Fighters[FighterNum.F1];
            }
            else if (AvaiableFighters[1])
            {
                fighter = Fighters[FighterNum.F2];
            }
            else if (AvaiableFighters[2])
            {
                fighter = Fighters[FighterNum.F3];
            }
        }

        CurrentFighter = fighter;
        return fighter;
    }

    public void FighterLost(Fighter fighter)
    {
        switch (fighter.FighterNum)
        {
            case FighterNum.F1:
                AvaiableFighters[0] = false;
                break;
            case FighterNum.F2:
                AvaiableFighters[1] = false;
                break;
            case FighterNum.F3:
                AvaiableFighters[2] = false;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void FighterWon(Fighter fighter)
    {
        WonFights++;
        AddStar();
    }

    public void AddStar()
    {
        if (!PlayerHud.Star1.enabled)
        {
            PlayerHud.Star1.enabled = true;
        }
        else if (!PlayerHud.Star2.enabled)
        {
            PlayerHud.Star2.enabled = true;
        }
        else if (!PlayerHud.Star3.enabled)
        {
            PlayerHud.Star3.enabled = true;
        }
    }

}