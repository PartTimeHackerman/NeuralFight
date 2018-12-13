using System;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class Item : MonoBehaviour
{
    public int ID = 0;
    public string Name;
    public string Description;
    public ItemType ItemType;

    public ItemMaterialType ItemMaterialType;
    [NonSerialized]
    public ItemMaterial ItemMaterial;
    
    public int Level;
    public int Stars;

    public bool Equipped = false;

    public float Mass = 0f;
    
    public float BaseRotZ = 0f;
    public Transform Center;
    public List<Outline> Outlines;

    public string Json;


    public virtual void Awake()
    {
        ItemMaterial = ItemMaterial.GetMaterialByType(ItemMaterialType);
        SetMeshMaterial();
    }

    public void EnableOutlines(bool enable)
    {
        Outlines.ForEach( o => o.enabled = enable);
    }

    public void SetMeshMaterial()
    {
        foreach (Renderer meshRenderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            meshRenderer.material = ItemMaterial.Material;
        }
    }
}