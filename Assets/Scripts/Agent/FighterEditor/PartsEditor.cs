using System.Collections.Generic;
using UnityEngine;

public class PartsEditor : MonoBehaviour
{
    public BodyParts BodyParts;
    public List<GameObject> partsToEdit = new List<GameObject>();
    public List<PartEditor> partEditors = new List<PartEditor>();
    public PartEditor PartEditorPref;
    public ArmWeapon rightWeapon;
    public ArmWeapon leftWeapon;
    
    public void SetFighter(Fighter fighter)
    {
        foreach (PartEditor partEditor in partEditors)
        {
            Destroy(partEditor.gameObject);
        }
        partEditors.Clear();
        
        BodyParts = fighter.BodyParts;
        rightWeapon = fighter.RightArmWeapon;
        leftWeapon = fighter.LeftArmWeapon;
        
        partsToEdit = BodyParts.editableParts;
        foreach (GameObject part in partsToEdit)
        {
            PartEditor partEditor = Instantiate(PartEditorPref);
            partEditor.name = part.name;
            partEditor.transform.parent = transform;
            partEditor.Init(part.gameObject);
            partEditors.Add(partEditor);

            if (part.name.Equals("rlowerarm")) partEditor.OnChangePart += () => rightWeapon.ReEquipWeapon();
            if (part.name.Equals("llowerarm")) partEditor.OnChangePart += () => leftWeapon.ReEquipWeapon();
        }
    }
}