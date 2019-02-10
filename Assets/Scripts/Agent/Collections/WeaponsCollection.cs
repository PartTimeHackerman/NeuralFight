using System;
using System.Collections.Generic;
using System.Linq;
using CI.QuickSave;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public abstract class WeaponsCollection : MonoBehaviour
{
    public Dictionary<string, Weapon> AllWeapons = new Dictionary<string, Weapon>();
    public Dictionary<string, Weapon> Weapons = new Dictionary<string, Weapon>();
    public Dictionary<string, Weapon> EquippedWeapons = new Dictionary<string, Weapon>();
    public WeaponInstancer WeaponInstancer;
    public EquipmentGetter EquipmentGetter;
    public string PlayerID = "";
    


    public Weapon GetWeaponById(string id)
    {
        if (Weapons.ContainsKey(id))
        {
            return Weapons[id];
        }
        else
        {
            if (EquippedWeapons.ContainsKey(id))
            {
                throw new Exception("Weapon with id " + id + " is already equipped");
            }

            throw new Exception("There's no weapon with id " + id);
        }
    }
    
    public void AddWeapon(Weapon weapon)
    {
        AllWeapons[weapon.ID] = weapon;
        if (weapon.Equipped)
        {
            EquippedWeapons[weapon.ID] = weapon;
        }
        else
        {
            Weapons[weapon.ID] = weapon;
        }
    }

    protected abstract void SetWeaponPos(Weapon weapon);
}