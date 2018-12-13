using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWeaponEquipper : MonoBehaviour
{
    public List<Weapon> OneHandedWeapons = new List<Weapon>();
    private WeaponEquipper WeaponEquipper;
    public bool equip = false;

    void Start()
    {
        //StartCoroutine(Waiter.WaitForFrames(1, () => { }, Equip));
    }

    private void FixedUpdate()
    {
        if (equip)
        {
            //Equip();
            equip = false;
        }
    }

    public void Equip(Fighter fighter)
    {
        Weapon right = Instantiate(OneHandedWeapons[Random.Range(0, OneHandedWeapons.Count)]);
        Weapon left = Instantiate(OneHandedWeapons[Random.Range(0, OneHandedWeapons.Count)]);
        
        Weapon rightOldWeapon = fighter.RightArmWeapon.weapon;
        Weapon leftOldWeapon = fighter.LeftArmWeapon.weapon;

        fighter.RightArmWeapon.Weapon = right;
        fighter.LeftArmWeapon.Weapon = left;

        if (rightOldWeapon != null)
        {
            Destroy(rightOldWeapon.gameObject);
        }
        
        if (leftOldWeapon != null)
        {
            Destroy(leftOldWeapon.gameObject);
        }
        
        
        
    }

}