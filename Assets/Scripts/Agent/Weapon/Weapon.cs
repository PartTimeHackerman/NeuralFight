
using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 0f;
    public float SPReq = 0f;
    
    public Rigidbody weaponRb;
    public Rigidbody grip;
    
    public bool OneHanded = true;
    public WeaponHand WeaponHand;
    public WeaponAttack WeaponAttack;
    public WeaponBlock WeaponBlock;

    void Awake()
    {
        foreach (Rigidbody child in GetComponentsInChildren<Rigidbody>())
        {
            if (child.name.Contains("grip")) grip = child;
            if (child.name.Contains("hitbox")) weaponRb = child;
            
        }
    }
}