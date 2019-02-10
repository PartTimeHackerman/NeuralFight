using System;

[Serializable]
public class WeaponJson : ItemJson
{
    public float BaseDamage;
    public float BaseSPReq;
    public WeaponType WeaponType;
    
    public WeaponJson()
    {
    }

    public static WeaponJson GetJson(Weapon weapon)
    {
        return new WeaponJson
            {
                ID = weapon.ID, 
                Level = weapon.Level,
                Description = weapon.Description, 
                ItemType = weapon.ItemType,
                ItemMaterialType = weapon.ItemMaterialType,
                Name = weapon.Name, 
                BaseDamage = weapon.BaseDamage, 
                BaseSPReq = weapon.BaseSPReq, 
                WeaponType = weapon.WeaponType, 
                Equipped = weapon.Equipped};
    }
    
    public static void SetFromJson(WeaponJson weaponJson, Weapon weapon)
    {
        weapon.ID = weaponJson.ID;
        weapon.Level = weaponJson.Level;
        weapon.Description = weaponJson.Description;
        weapon.ItemType = weaponJson.ItemType;
        weapon.ItemMaterialType = weaponJson.ItemMaterialType;
        weapon.BaseDamage = weaponJson.BaseDamage;
        weapon.BaseSPReq = weaponJson.BaseSPReq;
        weapon.Equipped = weaponJson.Equipped;
        weapon.SetUpWeapon();
    }
}