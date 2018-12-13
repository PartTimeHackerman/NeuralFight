using UnityEngine;

public class FighterEditor : MonoBehaviour
{

    public WeaponEquipper WeaponEquipper;
    public PartsEditor PartsEditor;
    public FighterSaverLoader FighterSaverLoader;
    public DistanceEquipper DistanceEquipper;
    public PartDistanceEquipper PartDistanceEquipper;

    public void SetFighter(Fighter fighter)
    {
        WeaponEquipper.SetFighter(fighter);
        PartsEditor.SetFighter(fighter);
        FighterSaverLoader.SetFighter(fighter);
        DistanceEquipper.SetFighter(fighter);
        PartDistanceEquipper.SetFighter(fighter);
        fighter.transform.position = ObjectsPositions.FighterEditorPos;
    }
}