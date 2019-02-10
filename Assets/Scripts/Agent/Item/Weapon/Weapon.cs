using System;
using cakeslice;
using UnityEngine;

public class Weapon : Item
{
    public float BaseDamage = 0f;
    public float BaseSPReq = 0f;
    public WeaponType WeaponType;

    public float Damage = 0f;
    public float SPReq = 0f;

    public Rigidbody Rigidbody;
    public DamagingPart DamagingPart;


    public WeaponDirection WeaponDirection;
    public WeaponAttack WeaponAttack;
    public WeaponBlock WeaponBlock;

    public float MinAttackDistance;
    public float MaxAttackDistance;


    public override void Awake()
    {
        base.Awake();
        Rigidbody = GetComponent<Rigidbody>();
        SetUpWeapon();
    }

    public void SetUpWeapon()
    {
        SetUpStats();

        foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.material = ItemMaterial.Material;
        }
    }

    public void SetUpStats()
    {
        ItemMaterial = ItemMaterial.GetMaterialByType(ItemMaterialType);
        Damage = BaseDamage * ItemMaterial.AttackMod * (1 + (Level * .1f));
        SPReq = BaseSPReq * ItemMaterial.SpReqMod * (1 + (Level * .1f));
        DamagingPart.damage = Damage;
        Rigidbody.mass = Rigidbody.mass * ItemMaterial.MassMod;
        Mass = Rigidbody.mass;
    }

    public override void Upgrade(int level)
    {
        Level = level;
        SetUpStats();
    }

    public void SetDamaging(bool damaging)
    {
        DamagingPart.damaging = damaging;
    }

    public WeaponJson GetJsonClass()
    {
        return WeaponJson.GetJson(this);
    }

    public void SetFromJson(WeaponJson weaponJson)
    {
        WeaponJson.SetFromJson(weaponJson, this);
    }
}