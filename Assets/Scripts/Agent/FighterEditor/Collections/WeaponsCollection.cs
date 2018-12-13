using System;
using System.Collections.Generic;
using System.Linq;
using CI.QuickSave;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WeaponsCollection : MonoBehaviour
{
    public Dictionary<int, Weapon> AllWeapons = new Dictionary<int, Weapon>();
    public Dictionary<int, Weapon> Weapons = new Dictionary<int, Weapon>();
    public Dictionary<int, Weapon> EquippedWeapons = new Dictionary<int, Weapon>();
    public WeaponInstancer WeaponInstancer;

    public bool saveWeapons = false;
    public Weapon weapon;
    public bool addWeapon = false;
    private float posX = 0f;

    private QuickSaveSettings settings = new QuickSaveSettings();

    private Transform WeaponsParent;

    public RectTransform ListContent;
    public ScrollRect ScrollRect;
    public ScrollListItem ScrollListItemPref;
    public DistanceEquipper DistanceEquipper;
    public WeaponInfo WeaponInfo;

    private void Start()
    {
        WeaponsParent = GetComponentsInChildren<Transform>().First(t => t.name.Equals("Weapons"));
        //GenerateWeps();
    }

    private void FixedUpdate()
    {
        if (saveWeapons)
        {
            SaveWeapons();
            saveWeapons = false;
        }

        if (addWeapon)
        {
            AddWeapon(weapon);
            weapon = null;
            addWeapon = false;
        }
    }

    public Weapon GetWeaponById(int id)
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

    private void SaveWeapons()
    {
        List<WeaponJson> weaponsJsons = new List<WeaponJson>();
        foreach (KeyValuePair<int, Weapon> weapon in AllWeapons)
        {
            weaponsJsons.Add(weapon.Value.GetJsonClass());
        }

        QuickSaveWriter writer = QuickSaveWriter.Create("weapons", settings);
        //writer.Write("weapons", JsonUtility.ToJson(new JsonList<WeaponJson>(weaponsJsons)));
        foreach (WeaponJson weaponsJson in weaponsJsons)
        {
            writer.Write(weaponsJson.ID.ToString(), JsonUtility.ToJson(weaponsJson));
        }

        writer.Commit();
    }

    public void LoadWeapons()
    {
        QuickSaveReader reader = QuickSaveReader.Create("weapons", settings);
        //List<WeaponJson> weaponJsons = JsonUtility.FromJson<JsonList<WeaponJson>>(reader.Read<string>("weapons")).list;

        foreach (string key in reader.GetAllKeys())
        {
            WeaponJson weaponJson = JsonUtility.FromJson<WeaponJson>(reader.Read<string>(key));
            Weapon weapon = WeaponInstancer.GetInstance(weaponJson);
            SetWeaponPos(weapon);
            AddWeapon(weapon);
            //weapon.transform.parent = WeaponsParent;
        }


        List<Weapon> sorted = Weapons.Values.OrderBy(w => w.WeaponType).ThenByDescending(w => w.Damage).ToList();
        foreach (Weapon wep in sorted)
        {
            AddToScrollList(wep);
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

    private void SetWeaponPos(Weapon weapon)
    {
        weapon.Rigidbody.isKinematic = true;
        weapon.transform.position = ObjectsPositions.PlayerWeaponsPos;
    }

    public void AddToScrollList(Weapon weapon)
    {
        weapon.Rigidbody.isKinematic = true;
        ScrollListItem scrollListItem = Instantiate(ScrollListItemPref);
        scrollListItem.transform.parent = ListContent;
        scrollListItem.transform.localPosition = Vector3.zero;
        scrollListItem.OnChooseItem += EnableScroll;
        scrollListItem.Item = weapon;
        weapon.transform.parent = scrollListItem.Center.transform;
        //weapon.transform.position = scrollListItem.transform.position - weapon.Center.position;
        Vector3 wepRot = weapon.transform.rotation.eulerAngles;
        wepRot.z = -30;
        weapon.transform.rotation = Quaternion.Euler(wepRot);
        weapon.transform.localPosition = weapon.transform.position - weapon.Center.position;

        Vector2 contentSize = ListContent.sizeDelta;
        contentSize.y = .5f + (ListContent.childCount * .5f);
        ListContent.sizeDelta = contentSize;
        SaveWeapons();
    }

    private void EnableScroll(ScrollListItem item, bool enable)
    {
        if (!enable)
        {
            DistanceEquipper.ItemToEquip = item;
            WeaponInfo.ItemToInfo = item;
        }

        Vector2 contentSize = ListContent.sizeDelta;
        contentSize.y = .5f + (ListContent.childCount * .5f);
        ListContent.sizeDelta = contentSize;

        ScrollRect.StopMovement();
        ScrollRect.enabled = enable;
    }

    private void GenerateWeps()
    {
        List<WeaponJson> weps = WeaponInstancer.WeaponsPrefs.Select(w => w.GetJsonClass()).ToList();

        for (int i = 0; i < 30; i++)
        {
            WeaponJson wep = weps[Random.Range(0, weps.Count)];
            wep.ID = i;
            Weapon weapon = WeaponInstancer.GetInstance(wep);
            switch (Random.Range(0,3))
            {
                case 0: 
                    weapon.ItemMaterialType = ItemMaterialType.WOOD;
                    break;
                case 1: 
                    weapon.ItemMaterialType = ItemMaterialType.ROCK;
                    break;
                case 2: 
                    weapon.ItemMaterialType = ItemMaterialType.STEEL;
                    break;
                 
            }
            SetWeaponPos(weapon);
            AddWeapon(weapon);
        }

        SaveWeapons();
    }
}