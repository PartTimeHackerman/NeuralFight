using UnityEngine;

public class DTO
{
    public string toJSON()
    {
        return JsonUtility.ToJson(this);
    }
}