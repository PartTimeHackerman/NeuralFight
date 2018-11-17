
using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 0f;
    public float SPReq = 0f;
    
    public Rigidbody Rigidbody;
    public DamagingPart DamagingPart;
    
    public bool OneHanded = true;
    public WeaponHand DefaultWeaponHand;
    public WeaponDirection WeaponDirection;
    public WeaponHand WeaponHand;
    public WeaponAttack WeaponAttack;
    public WeaponBlock WeaponBlock;

    public float MinAttackDistance;
    public float MaxAttackDistance;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        DamagingPart.damage = damage;
    }
}