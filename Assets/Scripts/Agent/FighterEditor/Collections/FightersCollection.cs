using System;
using System.Collections.Generic;
using UnityEngine;

public class FightersCollection : MonoBehaviour
{
    public Dictionary<FighterNum, Fighter> Fighters = new Dictionary<FighterNum, Fighter>();
    public Fighter FighterPref;

    public void CreateFighters()
    {
        foreach (FighterNum fighterNum in Enum.GetValues(typeof(FighterNum)))
        {
            Fighter fighter = Instantiate(FighterPref);
            fighter.FighterNum = fighterNum;
            fighter.transform.position = Vector3.zero;
            //fighter.gameObject.SetActive(false);
            fighter.SetSide(true);
            Fighters[fighterNum] = fighter;
        }
    }

    public Dictionary<FighterNum, Fighter> GetFighters()
    {
        Dictionary<FighterNum, Fighter> fighters = new Dictionary<FighterNum, Fighter>();
        foreach (FighterNum fighterNum in Enum.GetValues(typeof(FighterNum)))
        {
            Fighter fighter = Instantiate(FighterPref);
            fighter.FighterNum = fighterNum;
            fighter.transform.position = Vector3.zero;
            //fighter.gameObject.SetActive(false);
            fighter.SetSide(true);
            fighters[fighterNum] = fighter;
        }

        return fighters;
    }
}