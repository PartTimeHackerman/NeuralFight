using System.Collections.Generic;
using UnityEngine;

public class WeaponInstancer : MonoBehaviour
{
    
    public List<Weapon> WeaponsPrefs = new List<Weapon>();
    private Dictionary<WeaponType, Weapon> WeaponsPrefabsDict = new Dictionary<WeaponType, Weapon>();

    private void Awake()
    {
        foreach (Weapon weaponsPref in WeaponsPrefs)
        {
            WeaponsPrefabsDict[weaponsPref.WeaponType] = weaponsPref;
        }
    }

    public Weapon GetInstance(WeaponJson weaponJson)
    {
        Weapon weapon = Instantiate(WeaponsPrefabsDict[weaponJson.WeaponType]);
        weapon.SetFromJson(weaponJson);
        return weapon;
    }
}