using UnityEngine;

public class EditorDebugger : MonoBehaviour
{
    public WeaponEquipper WeaponEquipper;
    public PartsEditor PartsEditor;
    public FighterSaverLoader FighterSaverLoader;
    public DistanceEquipper DistanceEquipper;
    public PartDistanceEquipper PartDistanceEquipper;

    public Fighter Fighter;

    public bool setFighter = false;


    private void Update()
    {
        if (setFighter)
        {
            SetFighter(Fighter);
        }
    }

    public void SetFighter(Fighter fighter)
    {
        WeaponEquipper.SetFighter(fighter);
        PartsEditor.SetFighter(fighter);
        FighterSaverLoader.SetFighter(fighter);
        DistanceEquipper.SetFighter(fighter);
        PartDistanceEquipper.SetFighter(fighter);
        fighter.transform.position = new Vector3(-1f, 1f, -2.7f);
    }
}