using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquipper : MonoBehaviour
{
    public Weapon Weapon;
    public WeaponHand WeaponHand;

    public ArmWeapon RightArmWeapon;
    public ArmWeapon LeftArmWeapon;

    public bool equip = false;

    private void FixedUpdate()
    {
        if (equip)
        {
            EquipWeapon(WeaponHand, Weapon);
        }
    }

    public void SetFighter(Fighter fighter)
    {
        RightArmWeapon = fighter.RightArmWeapon;
        LeftArmWeapon = fighter.LeftArmWeapon;
    }

    public void EquipWeapon(WeaponHand weaponHand, Weapon weapon)
    {
        WeaponHand = weaponHand;
        Weapon = weapon;
        
        switch (WeaponHand)
        {
            case WeaponHand.RIGHT:
                if (LeftArmWeapon.Weapon == Weapon)
                    LeftArmWeapon.Weapon = null;
                //Weapon.WeaponHand = WeaponHand.RIGHT;
                RightArmWeapon.Weapon = Weapon;
                break;
            case WeaponHand.LEFT:
                if (RightArmWeapon.Weapon == Weapon)
                    RightArmWeapon.Weapon = null;
                //Weapon.WeaponHand = WeaponHand.LEFT;
                LeftArmWeapon.Weapon = Weapon;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        equip = false;
        Weapon = null;
    }
    
    public void EquipWeapon(ArmWeapon armWeapon, Weapon weapon)
    {
        armWeapon.Weapon = weapon;
    }
}