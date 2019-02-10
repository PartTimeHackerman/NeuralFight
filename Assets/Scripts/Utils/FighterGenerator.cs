using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class FighterGenerator : MonoBehaviour
{
    public PlayerFightersCollection FightersCollection;
    public PlayerFighterPartsCollection FighterPartsCollection;
    public PlayerWeaponsCollection WeaponsCollection;
    public FighterSaverLoader FighterSaverLoader;
    public FighterNum FighterNum;

    public Button Button;

    List<FighterPart> generatedParts = new List<FighterPart>();
    private void Start()
    {
        //Button.onClick.AddListener(() => GenerateFighter(FighterNum));
        /*Auth.OnAuth += (u, i) =>
        {
            GenerateFighter(FighterNum.F1);
            GenerateFighter(FighterNum.F2);
            GenerateFighter(FighterNum.F3);
        };*/
    }

    public void GenerateFighter(FighterNum fighterNum)
    {
        generatedParts.Clear();
        Fighter fighter = Instantiate(FightersCollection.FighterPref);
        FighterPart butt = GeneratePart(PartType.BUTT);
        FighterPart torso = GeneratePart(PartType.TORSO);
        FighterPart head = GeneratePart(PartType.HEAD);
        FighterPart rUpperArm = GeneratePart(PartType.UPPER_ARM);
        FighterPart lUpperArm = GeneratePart(PartType.UPPER_ARM);
        FighterPart rLowerArm = GeneratePart(PartType.LOWER_ARM);
        FighterPart lLowerArm = GeneratePart(PartType.LOWER_ARM);
        FighterPart rThigh = GeneratePart(PartType.THIGH);
        FighterPart lThigh = GeneratePart(PartType.THIGH);
        FighterPart rShin = GeneratePart(PartType.SHIN);
        FighterPart lShin = GeneratePart(PartType.SHIN);

        Weapon rWeapon = GenerateWeapon(WeaponType.FIST);
        Weapon lWeapon = GenerateWeapon(WeaponType.FIST);

        fighter.FighterNum = fighterNum;

        Dictionary<BodyPartType, BodyPart> bodyParts = fighter.BodyParts.AllNamedBodyParts;

        bodyParts[BodyPartType.BUTT].FighterPart = butt;
        bodyParts[BodyPartType.TORSO].FighterPart = torso;
        bodyParts[BodyPartType.HEAD].FighterPart = head;
        bodyParts[BodyPartType.R_UPPER_ARM].FighterPart = rUpperArm;
        bodyParts[BodyPartType.L_UPPER_ARM].FighterPart = lUpperArm;
        bodyParts[BodyPartType.R_LOWER_ARM].FighterPart = rLowerArm;
        bodyParts[BodyPartType.L_LOWER_ARM].FighterPart = lLowerArm;
        bodyParts[BodyPartType.R_THIGH].FighterPart = rThigh;
        bodyParts[BodyPartType.L_THIGH].FighterPart = lThigh;
        bodyParts[BodyPartType.R_SHIN].FighterPart = rShin;
        bodyParts[BodyPartType.L_SHIN].FighterPart = lShin;

        fighter.RightArmWeapon.Weapon = rWeapon;
        fighter.LeftArmWeapon.Weapon = lWeapon;

        FightersCollection.Fighters[fighterNum] = fighter;
        
        FightersCollection.SaveFighters();
        FighterPartsCollection.SaveFighterParts();
        WeaponsCollection.SaveWeapons();
    }

    private FighterPart GeneratePart(PartType type)
    {
        FighterPartJson partJson = FighterPartsCollection.FighterPartInstancer.FighterPartsPrefs
            .Select(w => w.GetJsonClass()).First(p => p.PartType == type);
        partJson.ID = System.Guid.NewGuid().ToString();
        FighterPart part = FighterPartsCollection.FighterPartInstancer.GetInstance(partJson);
        part.Upgrade(1);
        FighterPartsCollection.AllFighterParts[part.ID] = part;
        return part;
    }
    
    private Weapon GenerateWeapon(WeaponType type)
    {
        WeaponJson weaponJson = WeaponsCollection.WeaponInstancer.WeaponsPrefs
            .Select(w => w.GetJsonClass()).First(p => p.WeaponType == type);
        weaponJson.ID = System.Guid.NewGuid().ToString();
        Weapon weapon = WeaponsCollection.WeaponInstancer.GetInstance(weaponJson);
        weapon.Upgrade(1);
        WeaponsCollection.AllWeapons[weapon.ID] = weapon;
        return weapon;
    }
}