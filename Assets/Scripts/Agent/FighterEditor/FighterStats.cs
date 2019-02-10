using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FighterStats : MonoBehaviour
{
    public TextMeshProUGUI AvgLevel;
    public TextMeshProUGUI HP;
    public TextMeshProUGUI SP;
    public TextMeshProUGUI SPReg;
    public TextMeshProUGUI Damage;
    public TextMeshProUGUI SpReq;
    public TextMeshProUGUI Mass;

    public void SetFighter(Fighter fighter)
    {
        float avgLv = 0;
        float hp = 0;
        float sp = 0;
        float spReg = 0;
        float mass = 0;
        float damage = 0;
        float spReq = 0;
        
        float levelSum = 0;
        float partsCount = 0;

        foreach (KeyValuePair<string,BodyPart> keyValuePair in fighter.BodyParts.AllBodyParts)
        {
            BodyPart bodyPart = keyValuePair.Value;
            FighterPart fighterPart = bodyPart.fighterPart;

            hp += fighterPart.MaxHP;
            sp += fighterPart.MaxSP;
            spReg += fighterPart.SpRegen;
            mass += fighterPart.Mass;
            
            levelSum += fighterPart.Level;
            partsCount++;
        }

        levelSum += fighter.RightArmWeapon.weapon.Level;
        mass += fighter.RightArmWeapon.weapon.Mass;
        levelSum += fighter.LeftArmWeapon.weapon.Level;
        mass += fighter.LeftArmWeapon.weapon.Mass;
        partsCount += 2;

        avgLv = levelSum / partsCount;
        
        damage = (fighter.RightArmWeapon.weapon.Damage + fighter.LeftArmWeapon.weapon.Damage) / 2f;
        spReq = (fighter.RightArmWeapon.weapon.SPReq+ fighter.LeftArmWeapon.weapon.SPReq) / 2f;

        AvgLevel.text = avgLv.ToString("F1");
        HP.text = ThousandsConverter.ToKs(hp);
        SP.text = ThousandsConverter.ToKs(sp);
        SPReg.text = ThousandsConverter.ToKs(spReg);
        Damage.text = ThousandsConverter.ToKs(damage);
        SpReq.text = ThousandsConverter.ToKs(spReq);
        Mass.text = mass.ToString("F1") + "kg";
    }
}