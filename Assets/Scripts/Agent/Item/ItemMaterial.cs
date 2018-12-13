using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaterial
{
    public static readonly ItemMaterial WOOD = new ItemMaterial("Wood", .8f, 1.2f, 1.2f, .8f, .8f, .8f, .8f);
    public static readonly ItemMaterial ROCK = new ItemMaterial("Rock", 1.2f, .8f, .8f, 1.2f, 1.1f, 1.1f, 1.3f);
    public static readonly ItemMaterial STEEL = new ItemMaterial("Steel", 1.1f, 1.1f, .9f, 9f, 1.2f, 1.2f, 1.1f);

    public static IEnumerable<ItemMaterial> Values
    {
        get
        {
            yield return WOOD;
            yield return ROCK;
            yield return STEEL;
        }
    }

    public string Name { get; private set; }
    public float HpMod { get; private set; }
    public float SpMod { get; private set; }
    public float HpRegenMod { get; private set; }
    public float SpRegenMod { get; private set; }
    public float AttackMod { get; private set; }
    public float SpReqMod { get; private set; }
    public float MassMod { get; private set; }
    public Material Material { get; private set; }
    public readonly List<Material> Materials = new List<Material>();

    public ItemMaterial(string name, float hpMod, float spMod, float hpRegenMod, float spRegenMod, float attackMod,
        float spReqMod, float massMod)
    {
        Name = name;
        HpMod = hpMod;
        SpMod = spMod;
        HpRegenMod = hpRegenMod;
        SpRegenMod = spRegenMod;
        AttackMod = attackMod;
        SpReqMod = spReqMod;
        MassMod = massMod;
        Materials.Insert(0, Resources.Load<Material>("Materials/" + Name + "_1"));
        Materials.Insert(1, Resources.Load<Material>("Materials/" + Name + "_2"));
        Materials.Insert(2, Resources.Load<Material>("Materials/" + Name + "_3"));
        Material = Resources.Load<Material>("Materials/" + Name+ "_2");

    }

    public static ItemMaterial GetMaterialByType(ItemMaterialType type)
    {
        switch (type)
        {
            case ItemMaterialType.WOOD:
                return WOOD;
                break;
            case ItemMaterialType.ROCK:
                return ROCK;
                break;
            case ItemMaterialType.STEEL:
                return STEEL;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public override string ToString()
    {
        return Name;
    }
}

public enum ItemMaterialType
{
    WOOD,
    ROCK,
    STEEL
}