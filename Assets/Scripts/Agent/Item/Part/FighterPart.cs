using System;
using System.Collections.Generic;
using System.Linq;
using cakeslice;
using UnityEngine;

public class FighterPart : Item
{
    public float BaseMaxHP;
    public float BaseMaxSP;
    public float BaseHpRegen;
    public float BaseSpRegen;
    public PartType PartType;

    public float MaxHP;
    public float MaxSP;
    public float HpRegen;
    public float SpRegen;
    public Dictionary<Part, Rigidbody> Rigidbodies = new Dictionary<Part, Rigidbody>();


    public override void Awake()
    {
        base.Awake();
        SetUpPart();

        Json = JsonUtility.ToJson(FighterPartJson.GetJson(this));
    }

    public void Equip(BodyPart bodyPart)
    {
        Outlines = bodyPart.GetComponentsInChildren<Outline>().ToList();
        foreach (Rigidbody rigidbody in bodyPart.Rigidbodies)
        {
            Part part = Part.GetPartByName(rigidbody.name);
            if (part == null)
            {
                Debug.Log(rigidbody.name);
            }

            rigidbody.mass = part.Mass * ItemMaterial.MassMod;
        }

        //Mass = Rigidbodies.Values.Select(r => r.mass).Sum();
        Equipped = true;

        bodyPart.MaxHP = MaxHP;
        bodyPart.MaxSP = MaxSP;
        bodyPart.HpRegen = HpRegen;
        bodyPart.SpRegen = SpRegen;

        bodyPart.MaterialChangerManager.ChangeMaterial(ItemMaterial);
        gameObject.transform.parent = bodyPart.Player.Fighter.FighterParts;
        gameObject.transform.position = ObjectsPositions.PlayerPartsPos;
    }

    public void UnEquip()
    {
        Outlines = gameObject.GetComponentsInChildren<Outline>().ToList();
        Rigidbodies.Clear();
    }

    public void SetUpPart()
    {
        SetUpStats();

        foreach (Transform basePart in GetComponentsInChildren<Transform>())
        {
            Part part = Part.GetPartByName(basePart.name);
            if (part != null)
            {
                Mass += part.Mass * ItemMaterial.MassMod;
            }
        }

        foreach (MaterialChanger materialChanger in GetComponentsInChildren<MaterialChanger>())
        {
            materialChanger.ChangeMaterial(ItemMaterial);
        }
    }

    public void SetUpStats()
    {
        ItemMaterial = ItemMaterial.GetMaterialByType(ItemMaterialType);
        MaxHP = BaseMaxHP * ItemMaterial.HpMod * (1 + (Level * .1f));
        MaxSP = BaseMaxSP * ItemMaterial.SpMod * (1 + (Level * .1f));
        HpRegen = BaseHpRegen * ItemMaterial.HpRegenMod * (1 + (Level * .1f));
        SpRegen = BaseSpRegen * ItemMaterial.SpRegenMod * (1 + (Level * .1f));
    }
    
    public override void Upgrade(int level)
    {
        Level = level;
        SetUpStats();
    }

    public FighterPartJson GetJsonClass()
    {
        return FighterPartJson.GetJson(this);
    }

    public void SetFromJson(FighterPartJson fighterPartJson)
    {
        FighterPartJson.SetFromJson(fighterPartJson, this);
    }
}