using System.Collections.Generic;
using System.Linq;
using CI.QuickSave;
using UnityEngine;

public class EnemyFighterLoader : MonoBehaviour
{
    public WeaponInstancer WeaponInstancer;
    public FightersCollection FightersCollection;
    private QuickSaveSettings settings = new QuickSaveSettings();

    public Dictionary<FighterNum, Fighter> EnemyFighters;
    public Dictionary<int, Weapon> AllWeapons = new Dictionary<int, Weapon>();

    private void Start()
    {
        LoadWeapons();
        EnemyFighters = FightersCollection.GetFighters();
        foreach (KeyValuePair<FighterNum,Fighter> fighter in EnemyFighters)
        {
            fighter.Value.SetSide(false);
            LoadFighter(fighter.Value);
            fighter.Value.GetComponentsInChildren<Transform>().ToList().ForEach(go => go.gameObject.layer = LayerMask.NameToLayer("Player2"));
            fighter.Value.transform.position = ObjectsPositions.EnemyFightersPos;
        }
    }
    
    public void LoadWeapons()
    {
        QuickSaveReader reader = QuickSaveReader.Create("Enemyweapons", settings);
        //List<WeaponJson> weaponJsons = JsonUtility.FromJson<JsonList<WeaponJson>>(reader.Read<string>("weapons")).list;

        foreach (string key in reader.GetAllKeys())
        {
            WeaponJson weaponJson = JsonUtility.FromJson<WeaponJson>(reader.Read<string>(key));
            Weapon weapon = WeaponInstancer.GetInstance(weaponJson);
            AllWeapons[weapon.ID] = weapon;
            weapon.transform.position = ObjectsPositions.EnemyWeaponsPos;
            weapon.gameObject.layer = LayerMask.NameToLayer("Player2");
            DontDestroyOnLoad(weapon);
        }
    }

    public void LoadFighter(Fighter fighter)
    {
        Dictionary<string, PosRot> prs = new Dictionary<string, PosRot>();

        QuickSaveReader reader = QuickSaveReader.Create("Enemy"+fighter.FighterNum.ToString(), settings);


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
        
        foreach (EditablePart editablePartpart in fighter.GetComponentsInChildren<EditablePart>())
        {
            Transform part = editablePartpart.transform;
            PartEditor.UpdateSize(part);
        }
        
        fighter.BodyParts.root.transform.localPosition = Vector3.zero;
        */

        fighter.FighterDefaultPositioner = new FighterDefaultPositioner(fighter, prs);
        fighter.FighterDefaultPositioner.ResetPosition();

        int rightWeaponId = reader.Read<int>("rightWeapon");
        Weapon rightWeapon = AllWeapons[rightWeaponId];
        fighter.RightArmWeapon.Weapon = rightWeapon;

        int leftWeaponId = reader.Read<int>("leftWeapon");
        Weapon leftWeapon = AllWeapons[leftWeaponId];
        fighter.LeftArmWeapon.Weapon = leftWeapon;
        
    }
}