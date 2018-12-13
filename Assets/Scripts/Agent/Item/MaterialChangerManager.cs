using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialChangerManager : MonoBehaviour
{
    private List<MaterialChanger> MaterialChangers;

    private void Start()
    {
        MaterialChangers = GetComponentsInChildren<MaterialChanger>().ToList();
    }
    
    public void ChangeMaterial(ItemMaterial itemMaterial)
    {
        foreach (MaterialChanger materialChanger in MaterialChangers)
        {
            materialChanger.ChangeMaterial(itemMaterial);
        }
    }
}