using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaFighterChooser : MonoBehaviour
{
    public Button Button1;
    public Button Button2;
    public Button Button3;

    public FightersCollection FightersCollection;

    public event ChooseFighter OnChooseFighter;
    public delegate void ChooseFighter(Fighter fighter);

    private void Start()
    {
        Button1.onClick.AddListener(() => SetFighter(FighterNum.F1));
        Button2.onClick.AddListener(() => SetFighter(FighterNum.F2));
        Button3.onClick.AddListener(() => SetFighter(FighterNum.F3));
    }

    public void SetFighter(FighterNum fighterNum)
    {
        Fighter choosedFighter = null;
        foreach (KeyValuePair<FighterNum, Fighter> fighter in FightersCollection.Fighters)
        {
            if (fighter.Key == fighterNum)
            {
                choosedFighter = fighter.Value;
            }
            else
            {
                fighter.Value.transform.position = new Vector3(-200f, -100f, 0f);
            }
        }
        OnChooseFighter?.Invoke(choosedFighter);
    }
}