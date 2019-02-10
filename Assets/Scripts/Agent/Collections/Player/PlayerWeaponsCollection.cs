using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CI.QuickSave;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerWeaponsCollection : WeaponsCollection
{
    public RectTransform ListContent;
    public ScrollRect ScrollRect;
    public ScrollListItem ScrollListItemPref;
    public WeaponDistanceEquipper WeaponDistanceEquipper;
    public WeaponInfo WeaponInfo;

    public void SaveWeapons()
    {
        foreach (KeyValuePair<string, Weapon> keyValuePair in AllWeapons)
        {
            Storage.SaveWeapon(keyValuePair.Value);
        }
    }

    public async Task LoadWeapons()
    {
        List<Weapon> weapons = await EquipmentGetter.GetWeapons(PlayerID);

        foreach (Weapon weapon in weapons)
        {
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


    public void AddToScrollList(Weapon weapon)
    {
        weapon.Rigidbody.isKinematic = true;
        ScrollListItem scrollListItem = Instantiate(ScrollListItemPref);
        scrollListItem.transform.parent = ListContent;
        scrollListItem.transform.localPosition = Vector3.zero;
        scrollListItem.OnChooseItem += EnableScroll;
        scrollListItem.OnChooseItem += (i, c) =>
        {
            if (c)
            {
                WeaponInfo.Upgrade(i);
            }
        };
        scrollListItem.OnSelectItem += (i) =>
        {
            WeaponInfo.SetWeapon(i);
        };
        scrollListItem.OnDragItem += item => WeaponInfo.ShowUpgrade(item);
        scrollListItem.SetItem(weapon);

        Vector2 contentSize = ListContent.sizeDelta;
        contentSize.y = .5f + (ListContent.childCount * .5f);
        ListContent.sizeDelta = contentSize;
    }

    private void EnableScroll(ScrollListItem item, bool enable)
    {
        if (!enable)
        {
            WeaponDistanceEquipper.ItemToEquip = item;
        }

        Vector2 contentSize = ListContent.sizeDelta;
        contentSize.y = .5f + (ListContent.childCount * .5f);
        ListContent.sizeDelta = contentSize;

        ScrollRect.StopMovement();
        ScrollRect.enabled = enable;
    }

    protected override void SetWeaponPos(Weapon weapon)
    {
        weapon.Rigidbody.isKinematic = true;
        weapon.transform.position = ObjectsPositions.PlayerWeaponsPos;
    }
}