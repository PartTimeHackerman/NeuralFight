using System.Collections.Generic;
using System.Linq;
using CI.QuickSave;
using UnityEngine;

public class EnemyFighterLoader : MonoBehaviour
{
    public WeaponInstancer WeaponInstancer;
    public FighterPartInstancer FighterPartInstancer;
    public FightersCollection FightersCollection;
    private QuickSaveSettings settings = new QuickSaveSettings();

    public Dictionary<FighterNum, Fighter> EnemyFighters;
    public Dictionary<string, Weapon> AllWeapons = new Dictionary<string, Weapon>();
    public Dictionary<string, FighterPart> AllFighterParts = new Dictionary<string, FighterPart>();

    private void Start()
    {
        LoadWeapons();
        LoadParts();
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
        List<WeaponJson> weaponJsons = JsonUtility.FromJson<JsonList<WeaponJson>>(reader.Read<string>("weapons")).list;

        foreach (WeaponJson weaponJson in weaponJsons)
        {
            Weapon weapon = WeaponInstancer.GetInstance(weaponJson);
            AllWeapons[weapon.ID] = weapon;
            weapon.transform.position = ObjectsPositions.EnemyWeaponsPos;
            weapon.gameObject.layer = LayerMask.NameToLayer("Player2");
            DontDestroyOnLoad(weapon);
        }
    }
    
    public void LoadParts()
    {
        QuickSaveReader reader = QuickSaveReader.Create("EnemyFighterParts", settings);
        List<FighterPartJson> FighterPartJsons = JsonUtility.FromJson<JsonList<FighterPartJson>>(reader.Read<string>("FighterParts")).list;

        foreach (FighterPartJson FighterPartJson in FighterPartJsons)
        {
            FighterPart FighterPart = FighterPartInstancer.GetInstance(FighterPartJson);
            AllFighterParts[FighterPart.ID] = FighterPart;
            FighterPart.transform.position = ObjectsPositions.EnemyWeaponsPos;
            FighterPart.gameObject.layer = LayerMask.NameToLayer("Player2");
            DontDestroyOnLoad(FighterPart);
        }
    }

    public void LoadFighter(Fighter fighter)
    {
        Dictionary<string, PosRot> prs = new Dictionary<string, PosRot>();
        QuickSaveReader reader = QuickSaveReader.Create("Enemy"+fighter.FighterNum.ToString(), settings);
        FighterJSON fighterJson = JsonUtility.FromJson<FighterJSON>(reader.Read<string>("fighter"));
        FighterJSON.JSONTOFighter(fighterJson, fighter, AllWeapons, AllFighterParts);
        
    }
}