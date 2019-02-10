using System;
using System.Collections.Generic;
using UnityEngine;

public static class Storage
{
    public static string PlayerID = "";
    
    public static void SaveFighterPart(FighterPart fighterPart)
    {
        FighterPartJson fighterPartJSON = fighterPart.GetJsonClass();
        string JSON = JsonUtility.ToJson(fighterPartJSON);

        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("AddPart")
            .SetEventAttribute("PlayerID", PlayerID)
            .SetEventAttribute("Data", JSON)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    //Debug.Log("Part Saved To GameSparks...");
                }
                else
                {
                    Debug.Log("Error Saving Part Data...");
                }
            });
    }
    
    public static void SaveFighterParts(List<FighterPart> fighterPart)
    {
        List<FighterPartJson> jsons = new List<FighterPartJson>();
        foreach (FighterPart part in fighterPart)
        {
            jsons.Add(part.GetJsonClass());
        }
        string JSON = JsonUtility.ToJson(new JsonList<FighterPartJson>(jsons));
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("AddParts")
            .SetEventAttribute("PlayerID", PlayerID)
            .SetEventAttribute("PartsList", JSON)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    //Debug.Log("Part Saved To GameSparks...");
                }
                else
                {
                    Debug.Log("Error Saving Part Data...");
                }
            });
    }
    
    public static void DeleteFighterPart(FighterPart fighterPart)
    {
        FighterPartJson fighterPartJSON = fighterPart.GetJsonClass();

        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("DeletePart")
            .SetEventAttribute("ID", fighterPartJSON.ID)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    //Debug.Log("Part Saved To GameSparks...");
                }
                else
                {
                    Debug.Log("Error Deleting Part Data...");
                }
            });
    }
    public static void SaveWeapon(Weapon weapon)
    {
        WeaponJson weaponJson = weapon.GetJsonClass();
        string JSON = JsonUtility.ToJson(weaponJson);

        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("AddWeapon")
            .SetEventAttribute("PlayerID", PlayerID)
            .SetEventAttribute("Data", JSON)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    //Debug.Log("Weapon Saved To GameSparks...");
                }
                else
                {
                    Debug.Log("Error Saving Weapon Data...");
                }
            });
    }
    
    public static void SaveWeapons(List<Weapon> weapons)
    {
        List<WeaponJson> jsons = new List<WeaponJson>();
        foreach (Weapon weapon in weapons)
        {
            jsons.Add(weapon.GetJsonClass());
        }
        
        string JSON = JsonUtility.ToJson(new JsonList<WeaponJson>(jsons));
        
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("AddWeapons")
            .SetEventAttribute("PlayerID", PlayerID)
            .SetEventAttribute("WeaponsList", JSON)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    //Debug.Log("Weapon Saved To GameSparks...");
                }
                else
                {
                    Debug.Log("Error Saving Weapon Data...");
                }
            });
    }
    
    public static void DeleteWeapon(Weapon weapon)
    {
        WeaponJson weaponJson = weapon.GetJsonClass();

        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("DeleteWeapon")
            .SetEventAttribute("ID", weaponJson.ID)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    //Debug.Log("Part Saved To GameSparks...");
                }
                else
                {
                    Debug.Log("Error Deleting Weapon Data...");
                }
            });
    }
    
    public static void SaveFighter(Fighter fighter)
    {
        FighterJSON fighterJson = FighterJSON.FighterToJSON(fighter);
        string JSON = JsonUtility.ToJson(fighterJson);

        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("AddFighter")
            .SetEventAttribute("PlayerID", PlayerID)
            .SetEventAttribute("Data", JSON)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    //Debug.Log("Weapon Saved To GameSparks...");
                }
                else
                {
                    Debug.Log("Error Saving Fighter Data...");
                }
            });
    }
    public static void SaveItem(Item item)
    {
        if (item.GetType() == typeof(FighterPart))
        {
            SaveFighterPart((FighterPart)item);
        }
        else
        {
            SaveWeapon((Weapon)item);
        }
    }
    
    public static void DeleteItem(Item item)
    {
        if (item.GetType() == typeof(FighterPart))
        {
            DeleteFighterPart((FighterPart)item);
        }
        else
        {
            DeleteWeapon((Weapon)item);
        }
    }
}