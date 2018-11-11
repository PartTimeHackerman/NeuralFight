using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWeaponEquipper : MonoBehaviour
{
    public Player Player;
    public List<Weapon> OneHandedWeapons = new List<Weapon>();
    private WeaponEquipper WeaponEquipper;
    public bool equip = false;

    void Start()
    {
        WeaponEquipper = Player.WeaponEquipper;
        //StartCoroutine(Waiter.WaitForFrames(1, () => { }, Equip));
    }

    private void FixedUpdate()
    {
        if (equip)
        {
            Equip();
            equip = false;
        }
    }

    public void Equip()
    {
        Weapon right = Instantiate(OneHandedWeapons[Random.Range(0, OneHandedWeapons.Count)]);
        Weapon left = Instantiate(OneHandedWeapons[Random.Range(0, OneHandedWeapons.Count)]);
        Weapon rightOldWeapon = WeaponEquipper.RightArmWeapon.weapon;
        Weapon leftOldWeapon = WeaponEquipper.LeftArmWeapon.weapon;
        WeaponEquipper.EquipWeapon(WeaponHand.RIGHT, right);
        WeaponEquipper.EquipWeapon(WeaponHand.LEFT, left);

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