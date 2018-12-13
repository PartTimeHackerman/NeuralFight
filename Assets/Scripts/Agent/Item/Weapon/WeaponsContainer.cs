using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponsContainer : MonoBehaviour
{
    public Weapon rightWeapon;
    public Weapon leftWeapon;
    public Weapon twoHandedWeapon;
    public List<Weapon> weapons;

    void Awake()
    {
        /*
         weapons = GetComponentsInChildren<Weapon>().ToList();
        foreach (Weapon weapon in weapons)
        {
            if (weapon.WeaponHand == WeaponHand.RIGHT)
            {
                if (rightWeapon == null)
                    rightWeapon = weapon;
                else
                    throw new Exception("Only one weapon per hand allowed [R]");
            }
            if (weapon.WeaponHand == WeaponHand.LEFT)
            {
                if (leftWeapon == null)
                    leftWeapon = weapon;
                else
                    throw new Exception("Only one weapon per hand allowed [L]");
            }
            
            if (weapon.WeaponHand == WeaponHand.BOTH)
            {
                if (rightWeapon == null && leftWeapon == null && twoHandedWeapon == null)
                    twoHandedWeapon = weapon;
                else
                    throw new Exception("Only one two handed weapon allowed");
            }
        }
        */
    }
}