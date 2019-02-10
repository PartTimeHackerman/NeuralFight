using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyCollections : MonoBehaviour
{
    public static EnemyCollections EnemyCollectionsInstance;
    public EnemyFightersCollection EnemyFightersCollection;
    public EnemyFighterPartsCollection EnemyFighterPartsCollection;
    public EnemyWeaponsCollection EnemyWeaponsCollection;
    public EquipmentGetter EquipmentGetter;
    public string PlayerID = "";
    public UserPlayer UserPlayer;

    
    public static event EnemyCreate OnCreated;
    public delegate void EnemyCreate(UserPlayer userPlayer);

    public int AverageItemsLevel = 0;
    
    public static EnemyCollections Get()
    {
        return EnemyCollectionsInstance;
    }

    private void Awake()
    {
        EnemyCollectionsInstance = this;
        EnemyFightersCollection.CreateFighters();
    }

    public async Task LoadEnemy(UserPlayer userPlayer)
    {
        UserPlayer = userPlayer;
        LoadEnemy(userPlayer.PlayerID);
    }

    public async Task LoadEnemy(string playerID)
    {
        PlayerID = playerID;
        EnemyFighterPartsCollection.PlayerID = PlayerID;
        EnemyWeaponsCollection.PlayerID = playerID;

        List<FighterPart> parts = EnemyFighterPartsCollection.AllFighterParts.Values.ToList();
        for (var i = 0; i < parts.Count; i++)
        {
            
            Destroy(parts[i].gameObject);
            
        }
        EnemyFighterPartsCollection.AllFighterParts.Clear();

        List<Weapon> weapons = EnemyWeaponsCollection.AllWeapons.Values.ToList();
        
        for (var i = 0; i < weapons.Count; i++)
        {
            Destroy(weapons[i].gameObject);
        }
        EnemyWeaponsCollection.AllWeapons.Clear();
        
        
        List<FighterJSON> fighterJsons = await EquipmentGetter.GetFighters(PlayerID);
        

        List<string> equippedParts = new List<string>();
        List<string> equippedWeapons = new List<string>();

        
        foreach (FighterJSON fighterJson in fighterJsons)
        {
            foreach (EquippedFighterPartJson equippedFighterPartJson in fighterJson.EquippedParts)
            {
                equippedParts.Add(equippedFighterPartJson.PartID);
            }
            equippedWeapons.Add(fighterJson.LeftWeaponID);
            equippedWeapons.Add(fighterJson.RightWeaponID);
        }
        
        await EnemyFighterPartsCollection.LoadFighterParts(equippedParts);
        await EnemyWeaponsCollection.LoadFighterWeapons(equippedWeapons);
        
        
        float SumLevels = 0f;
        float items = 0f;
        foreach (KeyValuePair<string,FighterPart> allFighterPart in EnemyFighterPartsCollection.AllFighterParts)
        {
            //Storage.DeleteFighterPart(allFighterPart.Value);
            SumLevels += allFighterPart.Value.Level;
            items++;
        }
        
        foreach (KeyValuePair<string,Weapon> keyValuePair in EnemyWeaponsCollection.AllWeapons)
        {
            
            //Storage.DeleteWeapon(keyValuePair.Value);
            SumLevels += keyValuePair.Value.Level;
            items++;
        }

        AverageItemsLevel = Mathf.RoundToInt(SumLevels / items);
        
        ConstructFighters(fighterJsons);
        foreach (KeyValuePair<FighterNum,Fighter> keyValuePair in EnemyFightersCollection.Fighters)
        {
            keyValuePair.Value.GetComponentsInChildren<Transform>().ToList().ForEach(go => go.gameObject.layer = LayerMask.NameToLayer("Player2"));
        }
        OnCreated?.Invoke(UserPlayer);
    }

    public void ConstructFighters(List<FighterJSON> fighterJsons)
    {
        foreach (FighterJSON fighterJson in fighterJsons)
        {
            FighterJSON.JSONTOFighter(fighterJson, EnemyFightersCollection.Fighters[fighterJson.FighterNum], EnemyWeaponsCollection, EnemyFighterPartsCollection);
            EnemyFightersCollection.Fighters[fighterJson.FighterNum].Player.ResetPlayer();
        }
    }
}