using UnityEngine;

public class FighterEditor : MonoBehaviour
{

    public WeaponEquipper WeaponEquipper;
    public PartsEditor PartsEditor;
    public FighterSaverLoader FighterSaverLoader;
    public WeaponDistanceEquipper WeaponDistanceEquipper;
    public PartDistanceEquipper PartDistanceEquipper;
    private Fighter CurrentFighter;
    public FighterStats FighterStats;

    public void SetFighter(Fighter fighter)
    {
        //WeaponEquipper.SetFighter(fighter);
        //FighterSaverLoader.SetFighter(fighter);
        FighterStats.SetFighter(fighter);
        PartsEditor.SetFighter(fighter);
        WeaponDistanceEquipper.SetFighter(fighter);
        PartDistanceEquipper.SetFighter(fighter);
        UnSetFighter();
        CurrentFighter = fighter;
        Vector3 fighterPos = ObjectsPositions.FighterEditorPos;
        fighterPos.x += transform.position.x;
        fighter.transform.parent = transform;
        fighter.transform.position = fighterPos;
    }

    public void UnSetFighter()
    {
        if (CurrentFighter != null)
        {
            CurrentFighter.transform.parent = null;
            CurrentFighter.transform.position = ObjectsPositions.PlayerFightersPos;
            CurrentFighter = null;
        }
    }
}