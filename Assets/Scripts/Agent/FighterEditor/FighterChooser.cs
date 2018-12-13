using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterChooser : MonoBehaviour
{

    public Button Button1;
    public Button Button2;
    public Button Button3;
    
    public FightersCollection FightersCollection;
    public FighterEditor FighterEditor;
    
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
        foreach (KeyValuePair<FighterNum,Fighter> fighter in FightersCollection.Fighters)
        {
            if (fighter.Key == fighterNum)
            {
                choosedFighter = fighter.Value;
            }
            else
            {
                FighterEditor.FighterSaverLoader.SaveFighter(fighter.Value);
                fighter.Value.transform.position = new Vector3(-200f, -100f, 0f);
            }
        }
        
        choosedFighter.transform.position = new Vector3(-2f, .5f, -1.33f);
        FighterEditor.SetFighter(choosedFighter);
        OnChooseFighter?.Invoke(choosedFighter);
    }
}