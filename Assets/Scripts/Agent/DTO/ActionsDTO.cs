using System.Collections.Generic;
using UnityEngine;

public class ActionsDTO : DTO
{
    public List<float> actions;

    public int hash;

    public static ActionsDTO fromJSON(string json)
    {
        return JsonUtility.FromJson<ActionsDTO>(json);
    }

    public static List<ActionsDTO> listFromJSON(string json)
    {
        return JsonUtility.FromJson<List<ActionsDTO>>(json);
    }
}