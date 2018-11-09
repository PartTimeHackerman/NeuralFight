using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWeaponEquipper : MonoBehaviour
{
    public Player Player;
    public List<Weapon> OneHandedWeapons = new List<Weapon>();
    private WeaponEquipper WeaponEquipper;

    void Start()
    {
        WeaponEquipper = Player.WeaponEquipper;
        StartCoroutine(Waiter.WaitForFrames(1, () => { }, Equip));
    }

    private void Equip()
    {
        Weapon right = Instantiate(OneHandedWeapons[Random.Range(0, OneHandedWeapons.Count)]);
        Weapon left = Instantiate(OneHandedWeapons[Random.Range(0, OneHandedWeapons.Count)]);
        WeaponEquipper.EquipWeapon(WeaponHand.RIGHT, right);
        WeaponEquipper.EquipWeapon(WeaponHand.LEFT, left);
    }

}