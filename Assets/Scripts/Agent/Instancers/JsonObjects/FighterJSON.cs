using System;
using System.Collections.Generic;
using System.Linq;
using CI.QuickSave;
using UnityEngine;

[Serializable]
public class FighterJSON
{
    public FighterNum FighterNum;
    public List<PosRot> PosRot;
    public List<EquippedFighterPartJson> EquippedParts;
    public string RightWeaponID;
    public string LeftWeaponID;

    public FighterJSON()
    {
    }

    public static FighterJSON FighterToJSON(Fighter fighter)
    {
        FighterJSON fighterJson = new FighterJSON();
        Dictionary<string, PosRot> prs = new Dictionary<string, PosRot>();

        foreach (GameObject part in fighter.BodyParts.getParts())
        {
            string name = part.name;
            Transform transform = part.transform;
            PosRot pr = new PosRot {name = transform.name, pos = transform.position, rot = transform.rotation};
            prs[name] = pr;
        }

        List<EquippedFighterPartJson> equippedFighterPartJsons = new List<EquippedFighterPartJson>();
        foreach (KeyValuePair<BodyPartType,BodyPart> bodyPart in fighter.BodyParts.AllNamedBodyParts)
        {
            EquippedFighterPartJson equippedFighterPartJson = new EquippedFighterPartJson();
            equippedFighterPartJson.BodyPart = bodyPart.Key;
            equippedFighterPartJson.PartID = bodyPart.Value.fighterPart.ID;
            equippedFighterPartJsons.Add(equippedFighterPartJson);
        }
        
        List<PosRot> positions = prs.Values.ToList();
        fighterJson.FighterNum = fighter.FighterNum;
        fighterJson.PosRot = positions;
        fighterJson.EquippedParts = equippedFighterPartJsons;
        fighterJson.RightWeaponID = fighter.RightArmWeapon.weapon.ID;
        fighterJson.LeftWeaponID = fighter.LeftArmWeapon.weapon.ID;

        return fighterJson;
    }

    public static void JSONTOFighter(FighterJSON fighterJson, Fighter fighter, WeaponsCollection weaponsCollection, FighterPartsCollection fighterPartsCollection)
    {
        Dictionary<string, PosRot> prs = new Dictionary<string, PosRot>();
        List<PosRot> positions = fighterJson.PosRot;

        foreach (PosRot position in positions)
        {
            prs[position.name] = position;
        }
        
        fighter.FighterDefaultPositioner = new FighterDefaultPositioner(fighter, prs);
        fighter.FighterDefaultPositioner.ResetPosition();

        foreach (EquippedFighterPartJson equippedFighterPartJson in fighterJson.EquippedParts)
        {
            BodyPart bodyPart = fighter.BodyParts.AllNamedBodyParts[equippedFighterPartJson.BodyPart];
            FighterPart fighterPart = fighterPartsCollection.AllFighterParts[equippedFighterPartJson.PartID];
            bodyPart.FighterPart = fighterPart;
        }

        string rightWeaponId = fighterJson.RightWeaponID;
        Weapon rightWeapon = weaponsCollection.AllWeapons[rightWeaponId];
        fighter.RightArmWeapon.Weapon = rightWeapon;

        string leftWeaponId = fighterJson.LeftWeaponID;
        Weapon leftWeapon = weaponsCollection.AllWeapons[leftWeaponId];
        fighter.LeftArmWeapon.Weapon = leftWeapon;

        fighter.FighterNum = fighterJson.FighterNum;
    }
    
    public static void JSONTOFighter(FighterJSON fighterJson, Fighter fighter, Dictionary<string, Weapon> weaponsCollection, Dictionary<string, FighterPart> fighterPartsCollection)
    {
        Dictionary<string, PosRot> prs = new Dictionary<string, PosRot>();
        List<PosRot> positions = fighterJson.PosRot;

        foreach (PosRot position in positions)
        {
            prs[position.name] = position;
        }
        
        fighter.FighterDefaultPositioner = new FighterDefaultPositioner(fighter, prs);
        fighter.FighterDefaultPositioner.ResetPosition();

        foreach (EquippedFighterPartJson equippedFighterPartJson in fighterJson.EquippedParts)
        {
            BodyPart bodyPart = fighter.BodyParts.AllNamedBodyParts[equippedFighterPartJson.BodyPart];
            FighterPart fighterPart = fighterPartsCollection[equippedFighterPartJson.PartID];
            bodyPart.FighterPart = fighterPart;
        }

        string rightWeaponId = fighterJson.RightWeaponID;
        Weapon rightWeapon = weaponsCollection[rightWeaponId];
        fighter.RightArmWeapon.Weapon = rightWeapon;

        string leftWeaponId = fighterJson.LeftWeaponID;
        Weapon leftWeapon = weaponsCollection[leftWeaponId];
        fighter.LeftArmWeapon.Weapon = leftWeapon;
        
        
        fighter.FighterNum = fighterJson.FighterNum;
    }
    
    public static void JSONTOFighter(FighterJSON fighterJson, Fighter fighter)
    {
        Dictionary<string, PosRot> prs = new Dictionary<string, PosRot>();
        List<PosRot> positions = fighterJson.PosRot;

        foreach (PosRot position in positions)
        {
            prs[position.name] = position;
        }
        
        fighter.FighterDefaultPositioner = new FighterDefaultPositioner(fighter, prs);
        fighter.FighterDefaultPositioner.ResetPosition();
        fighter.FighterNum = fighterJson.FighterNum;
    }
}