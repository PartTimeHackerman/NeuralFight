using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyWeaponsCollection : WeaponsCollection
{
    
    public async Task LoadFighterWeapons(List<string> weaponsIDs)
    {
        List<Weapon> weapons = await EquipmentGetter.GetWeapons(PlayerID, weaponsIDs);

        foreach (Weapon weapon in weapons)
        {
            SetWeaponPos(weapon);
            AddWeapon(weapon);
            weapon.gameObject.layer = LayerMask.NameToLayer("Player2");
            //weapon.transform.parent = WeaponsParent;
        }
    }
    
    protected override void SetWeaponPos(Weapon weapon)
    {
        weapon.Rigidbody.isKinematic = true;
        weapon.transform.position = ObjectsPositions.EnemyWeaponsPos;
    }
}