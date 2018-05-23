using System.Collections.Generic;
using UnityEngine;

public class ObservationsDTO : DTO
{
    public int hash;
    public List<float> observations;
    public float reward;
    public bool terminated;

    public static ObservationsDTO fromJSON(string json)
    {
        return JsonUtility.FromJson<ObservationsDTO>(json);
    }
}