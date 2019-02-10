using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyFighterPartsCollection : FighterPartsCollection
{
    public async Task LoadFighterParts(List<string> partsIDs)
    {
        List<FighterPart> parts =  await EquipmentGetter.GetParts(PlayerID, partsIDs);
        
        foreach (FighterPart fighterPart in parts)
        {
            SetFighterPartPos(fighterPart);
            AddFighterPart(fighterPart);
            fighterPart.gameObject.layer = LayerMask.NameToLayer("Player2");
        }
    }

    protected override void SetFighterPartPos(FighterPart FighterPart)
    {
        FighterPart.transform.position = ObjectsPositions.EnemyPartsPos;
    }
}