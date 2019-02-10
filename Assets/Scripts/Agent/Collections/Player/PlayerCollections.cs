using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerCollections : MonoBehaviour
{
    public PlayerFightersCollection PlayerFightersCollection;
    public PlayerFighterPartsCollection PlayerFighterPartsCollection;
    public PlayerWeaponsCollection PlayerWeaponsCollection;
    public EquipmentGetter EquipmentGetter;
    public string PlayerID = "";
    public static UserPlayer UserPlayer;
    
    public static event PlayerCreate OnCreated;
    public delegate void PlayerCreate();

    private static PlayerCollections PlayerCollectionsInstance;

    private void Awake()
    {
        PlayerCollectionsInstance = this;
        PlayerFightersCollection.CreateFighters();
    }

    public static PlayerCollections Get()
    {
        return PlayerCollectionsInstance;
    }

    public int AverageItemsLevel = 0;
    
    public async Task LoadPlayer(string playerID)
    {
        PlayerID = playerID;
        PlayerFighterPartsCollection.PlayerID = PlayerID;
        PlayerWeaponsCollection.PlayerID = playerID;
        
        await PlayerFighterPartsCollection.LoadFighterParts();
        await PlayerWeaponsCollection.LoadWeapons();

        float SumLevels = 0f;
        float items = 0f;
        foreach (KeyValuePair<string,FighterPart> allFighterPart in PlayerFighterPartsCollection.AllFighterParts)
        {
            //Storage.DeleteFighterPart(allFighterPart.Value);
            SumLevels += allFighterPart.Value.Level;
            items++;
        }
        
        foreach (KeyValuePair<string,Weapon> keyValuePair in PlayerWeaponsCollection.AllWeapons)
        {
            
            //Storage.DeleteWeapon(keyValuePair.Value);
            SumLevels += keyValuePair.Value.Level;
            items++;
        }

        AverageItemsLevel = Mathf.RoundToInt(SumLevels / items);

        List<FighterJSON> fighterJsons = await EquipmentGetter.GetFighters(PlayerID);
        
        ConstructFighters(fighterJsons);
        
        
        OnCreated?.Invoke();
    }

    public void ConstructFighters(List<FighterJSON> fighterJsons)
    {
        foreach (FighterJSON fighterJson in fighterJsons)
        {
            FighterJSON.JSONTOFighter(fighterJson, PlayerFightersCollection.Fighters[fighterJson.FighterNum], PlayerWeaponsCollection, PlayerFighterPartsCollection);
            PlayerFightersCollection.Fighters[fighterJson.FighterNum].Player.ResetPlayer();
        }
    }

    public void SaveAll()
    {
        Storage.SaveFighterParts(PlayerFighterPartsCollection.AllFighterParts.Values.ToList());
        /*foreach (KeyValuePair<string,FighterPart> allFighterPart in PlayerFighterPartsCollection.AllFighterParts)
        {
            Storage.SaveFighterPart(allFighterPart.Value);
            
        }*/
        
        Storage.SaveWeapons(PlayerWeaponsCollection.AllWeapons.Values.ToList());
        /*foreach (KeyValuePair<string,Weapon> keyValuePair in PlayerWeaponsCollection.AllWeapons)
        {
            
            Storage.SaveWeapon(keyValuePair.Value);
            
        }*/
        
        foreach (KeyValuePair<FighterNum,Fighter> keyValuePair in PlayerFightersCollection.Fighters)
        {
            Storage.SaveFighter(keyValuePair.Value);
        }
    }
    
    public void SaveFighters()
    {
        
        foreach (KeyValuePair<FighterNum,Fighter> keyValuePair in PlayerFightersCollection.Fighters)
        {
            Storage.SaveFighter(keyValuePair.Value);
        }
    }
    
}