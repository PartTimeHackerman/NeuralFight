using System;
using System.Collections.Generic;
using System.Linq;
using CI.QuickSave;
using UnityEngine;

public class FighterSaverLoader : MonoBehaviour
{
    public Fighter fighter;
    public WeaponEquipper WeaponEquipper;
    public WeaponsCollection WeaponsCollection;
    public bool saveB;
    public bool loadB;

    public PartsEditor PartsEditor;
    private QuickSaveSettings settings = new QuickSaveSettings();

    private void Start()
    {
        //settings.SecurityMode = SecurityMode.Base64;
    }

    public void SetFighter(Fighter fighter)
    {
        this.fighter = fighter;
    }


    private void FixedUpdate()
    {
        if (saveB)
        {
            SaveFighter(fighter);
            saveB = false;
        }

        if (loadB)
        {
            LoadFighter(fighter);
            loadB = false;
        }
    }

    public void SaveFighter(Fighter fighter)
    {
        Dictionary<string, PosRot> prs = new Dictionary<string, PosRot>();

        foreach (GameObject part in fighter.BodyParts.getParts())
        {
            string name = part.name;
            Transform transform = part.transform;
            PosRot pr = new PosRot {name = transform.name, pos = transform.position, rot = transform.rotation};
            prs[name] = pr;
        }

        List<PosRot> positions = prs.Values.ToList();
        QuickSaveWriter writer = QuickSaveWriter.Create(fighter.FighterNum.ToString(), settings);
        writer.Write("positions", JsonUtility.ToJson(new JsonList<PosRot>(positions)));
        writer.Write("rightWeapon", fighter.RightArmWeapon.weapon.ID);
        writer.Write("leftWeapon", fighter.LeftArmWeapon.weapon.ID);
        writer.Commit();
    }

    public void LoadFighter(Fighter fighter)
    {
        Dictionary<string, PosRot> prs = new Dictionary<string, PosRot>();
        QuickSaveReader reader = QuickSaveReader.Create(fighter.FighterNum.ToString(), settings);

        List<PosRot> positions = JsonUtility.FromJson<JsonList<PosRot>>(reader.Read<string>("positions")).list;

        foreach (PosRot position in positions)
        {
            prs[position.name] = position;
        }
        /*
        foreach (GameObject part in fighter.BodyParts.getParts())
        {
            string name = part.name;
            Transform transform = part.transform;
            if (prs.ContainsKey(name))
            {
                transform.position = prs[name].pos;
                transform.rotation = prs[name].rot;
            }
        }
        
        fighter.BodyParts.root.transform.localPosition = Vector3.zero;
        */

        fighter.FighterDefaultPositioner = new FighterDefaultPositioner(fighter, prs);
        fighter.FighterDefaultPositioner.ResetPosition();

        int rightWeaponId = reader.Read<int>("rightWeapon");
        Weapon rightWeapon = WeaponsCollection.AllWeapons[rightWeaponId];
        WeaponEquipper.EquipWeapon(fighter.RightArmWeapon, rightWeapon);
        
        int leftWeaponId = reader.Read<int>("leftWeapon");
        Weapon leftWeapon = WeaponsCollection.AllWeapons[leftWeaponId];
        WeaponEquipper.EquipWeapon(fighter.LeftArmWeapon, leftWeapon);
    }
}