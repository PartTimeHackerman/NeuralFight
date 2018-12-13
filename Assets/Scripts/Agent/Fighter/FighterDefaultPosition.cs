using System.Collections.Generic;
using UnityEngine;

public class FighterDefaultPositioner
{
    public Fighter Fighter;
    Dictionary<string, PosRot> PosRots = new Dictionary<string, PosRot>();

    public FighterDefaultPositioner(Fighter fighter, Dictionary<string, PosRot> posRots)
    {
        Fighter = fighter;
        PosRots = posRots;
    }

    public void ResetPosition()
    {
        foreach (GameObject part in Fighter.BodyParts.getParts())
        {
            string name = part.name;
            Transform transform = part.transform;
            if (PosRots.ContainsKey(name))
            {
                transform.position = PosRots[name].pos;
                transform.rotation = PosRots[name].rot;
            }
        }
        
        foreach (EditablePart editablePartpart in Fighter.GetComponentsInChildren<EditablePart>())
        {
            Transform part = editablePartpart.transform;
            PartEditor.UpdateSize(part);
        }
        Fighter.BodyParts.root.transform.localPosition = Vector3.zero;
        Fighter.BodyParts.root.transform.localRotation = Quaternion.Euler(Vector3.zero);
        
    }
}