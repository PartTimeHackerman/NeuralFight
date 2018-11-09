using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquipper : MonoBehaviour
{
    public BodyParts BodyParts;
    public Weapon Weapon;
    public WeaponHand WeaponHand;

    public ArmWeapon RightArmWeapon;
    public ArmWeapon LeftArmWeapon;
    public ArmWeapon BothArmsWeapon;

    public bool equip = false;

    private void FixedUpdate()
    {
        if (equip)
        {
            EquipWeapon(WeaponHand, Weapon);
        }
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
            case WeaponHand.BOTH:
                //Weapon.WeaponHand = WeaponHand.BOTH;
                BothArmsWeapon.Weapon = Weapon;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        equip = false;
        Weapon = null;
    }
}