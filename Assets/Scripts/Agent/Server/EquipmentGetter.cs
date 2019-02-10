using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameSparks.Api.Responses;
using UnityEngine;

public class EquipmentGetter : MonoBehaviour
{
    public FighterPartInstancer FighterPartInstancer;
    public WeaponInstancer WeaponInstancer;

    private void Start()
    {
        
    }


    public async Task<List<FighterJSON>> GetFighters(string userID)
    {
        List<FighterJSON> fighters = new List<FighterJSON>();
        List<string> fightersJsons = new List<string>();
        var t = new TaskCompletionSource<List<string>>();
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("GetFighters")
            .SetEventAttribute("PlayerID", userID)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    fightersJsons.Add(response.ScriptData.GetGSData("data").GetGSData("F1").JSON);
                    fightersJsons.Add(response.ScriptData.GetGSData("data").GetGSData("F2").JSON);
                    fightersJsons.Add(response.ScriptData.GetGSData("data").GetGSData("F3").JSON);
                }
                else
                {
                    Debug.Log("Error Getting parts" + userID);
                }

                t.TrySetResult(fightersJsons);
            });

        await t.Task;

        foreach (string fighterString in fightersJsons)
        {
            FighterJSON fighterJson = JsonUtility.FromJson<FighterJSON>(fighterString);
            fighters.Add(fighterJson);
        }

        return fighters;
    }

    public async Task<List<FighterPart>> GetParts(string userID)
    {
        return await GetParts(userID, null);
    }

    public async Task<List<FighterPart>> GetParts(string userID, List<string> partsIDs)
    {
        string partsIDsJson = "{}";

        if (partsIDs != null)
        {
            partsIDsJson = JsonUtility.ToJson(new JsonList<string>(partsIDs));
        }

        List<FighterPart> fighterParts = new List<FighterPart>();
        List<string> partsJsons = new List<string>();
        var t = new TaskCompletionSource<List<string>>();
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("GetParts")
            .SetEventAttribute("PlayerID", userID)
            .SetEventAttribute("PartsIDs", partsIDsJson)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    partsJsons = GetResponseJSONS(response);
                }
                else
                {
                    Debug.Log("Error Getting parts" + userID);
                }

                t.TrySetResult(partsJsons);
            });

        await t.Task;

        foreach (string part in partsJsons)
        {
            FighterPartJson partJson = JsonUtility.FromJson<FighterPartJson>(part);
            FighterPart fighterPart = FighterPartInstancer.GetInstance(partJson);
            fighterParts.Add(fighterPart);
        }

        return fighterParts;
    }

    public async Task<List<Weapon>> GetWeapons(string userID)
    {
        return await GetWeapons(userID, null);
    }

    public async Task<List<Weapon>> GetWeapons(string userID, List<string> weaponsIDs)
    {
        string weaponsIDsJson = "{}";

        if (weaponsIDs != null)
        {
            weaponsIDsJson = JsonUtility.ToJson(new JsonList<string>(weaponsIDs));
        }

        List<Weapon> weapons = new List<Weapon>();
        List<string> weaponsJsons = new List<string>();
        var t = new TaskCompletionSource<List<string>>();
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("GetWeapons")
            .SetEventAttribute("PlayerID", userID)
            .SetEventAttribute("WeaponsIDs", weaponsIDsJson)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    weaponsJsons = GetResponseJSONS(response);
                }
                else
                {
                    Debug.Log("Error Getting Weapons " + userID);
                }

                t.TrySetResult(weaponsJsons);
            });

        await t.Task;

        foreach (string part in weaponsJsons)
        {
            WeaponJson weaponJson = JsonUtility.FromJson<WeaponJson>(part);
            Weapon weapon = WeaponInstancer.GetInstance(weaponJson);
            weapons.Add(weapon);
        }

        return weapons;
    }

    public List<string> GetResponseJSONS(LogEventResponse response)
    {
        List<string> JSONS = new List<string>();
        foreach (string baseDataKey in response.ScriptData.GetGSData("data").BaseData.Keys)
        {
            JSONS.Add(
                response.ScriptData.GetGSData("data").GetGSData(baseDataKey).GetGSData("Data").JSON);
        }

        return JSONS;
    }
}