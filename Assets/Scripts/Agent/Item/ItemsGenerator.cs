using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemsGenerator : MonoBehaviour
{
    public FighterPartInstancer FighterPartInstancer;
    public WeaponInstancer WeaponInstancer;

    private static ItemsGenerator ItemsGeneratorInstance;

    private void Awake()
    {
        ItemsGeneratorInstance = this;
    }

    public static ItemsGenerator Get()
    {
        return ItemsGeneratorInstance;
    }

    public Item GenerateItem(int level)
    {
        level = Math.Max(1, level);
        Item item = null;

        if (Random.value >= .33f)
        {
            Array Types = Enum.GetValues(typeof(PartType));
            item = GeneratePart((PartType) Types.GetValue(Random.Range(0, Types.Length)));
        }
        else
        {
            Array Types = Enum.GetValues(typeof(WeaponType));
            item = GenerateWeapon((WeaponType) Types.GetValue(Random.Range(0, Types.Length)));
        }

        item.Level = level;
        return item;
    }

    private FighterPart GeneratePart(PartType type)
    {
        FighterPartJson partJson = FighterPartInstancer.FighterPartsPrefs
            .Select(w => w.GetJsonClass()).First(p => p.PartType == type);
        partJson.ID = System.Guid.NewGuid().ToString();
        FighterPart part = FighterPartInstancer.GetInstance(partJson);
        switch (Random.Range(0, 3))
        {
            case 0:
                part.ItemMaterialType = ItemMaterialType.WOOD;
                break;
            case 1:
                part.ItemMaterialType = ItemMaterialType.ROCK;
                break;
            case 2:
                part.ItemMaterialType = ItemMaterialType.STEEL;
                break;
        }
        part.SetUpPart();
        return part;
    }

    private Weapon GenerateWeapon(WeaponType type)
    {
        WeaponJson weaponJson = WeaponInstancer.WeaponsPrefs
            .Select(w => w.GetJsonClass()).First(p => p.WeaponType == type);
        weaponJson.ID = System.Guid.NewGuid().ToString();
        Weapon weapon = WeaponInstancer.GetInstance(weaponJson);
        switch (Random.Range(0, 3))
        {
            case 0:
                weapon.ItemMaterialType = ItemMaterialType.WOOD;
                break;
            case 1:
                weapon.ItemMaterialType = ItemMaterialType.ROCK;
                break;
            case 2:
                weapon.ItemMaterialType = ItemMaterialType.STEEL;
                break;
        }
        weapon.SetUpWeapon();
        return weapon;
    }
}