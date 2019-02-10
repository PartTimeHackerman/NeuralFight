using System.Collections.Generic;
using UnityEngine;

public class FighterCreator : MonoBehaviour
{
    public PlayerWeaponsCollection WeaponsCollection;
    public PlayerFighterPartsCollection FighterPartsCollection;
    public PlayerFightersCollection FightersCollection;
    public FighterSaverLoader FighterSaverLoader;
    public FighterEditor FighterEditor;
    
    public PlayerCollections PlayerCollections;
    
    private void Start()
    {
        Auth.OnAuth += async (userName, id) =>
        {
            await PlayerCollections.LoadPlayer(id); 
            
            foreach (KeyValuePair<FighterNum, Fighter> fighter in FightersCollection.Fighters)
            {
                //FighterSaverLoader.LoadFighter(fighter.Value);
                //FighterEditor.SetFighter(fighter.Value);

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
                    });*/
            }
        };

        

        //FighterEditor.SetFighter(FightersCollection.Fighters[FighterNum.F1]);
    }
}