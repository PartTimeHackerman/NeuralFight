using System.Collections.Generic;
using UnityEngine;

public class FighterCreator : MonoBehaviour
{
    public WeaponsCollection WeaponsCollection;
    public FighterPartsCollection FighterPartsCollection;
    public FightersCollection FightersCollection;
    public FighterSaverLoader FighterSaverLoader;
    public FighterEditor FighterEditor;

    private void Start()
    {
        WeaponsCollection.LoadWeapons();
        FighterPartsCollection.LoadFighterParts();
        FightersCollection.CreateFighters();

        foreach (KeyValuePair<FighterNum, Fighter> fighter in FightersCollection.Fighters)
        {
            FighterSaverLoader.LoadFighter(fighter.Value);
            FighterEditor.SetFighter(fighter.Value);

            fighter.Value.transform.position = ObjectsPositions.PlayerFightersPos;
            //fighter.Value.gameObject.SetActive(false);

            /*Waiter.Get().WaitForFramesC(3, () =>
                {
                    FighterEditor.SetFighter(fighter.Value);
                    FighterSaverLoader.LoadFighter(fighter.Value);
                },
                () =>
                {
                    fighter.Value.gameObject.SetActive(false);
                });
            */

        }

        //FighterEditor.SetFighter(FightersCollection.Fighters[FighterNum.F1]);
    }
}