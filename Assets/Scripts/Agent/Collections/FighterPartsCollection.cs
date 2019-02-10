using System;
using System.Collections.Generic;
using System.Linq;
using CI.QuickSave;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public abstract class FighterPartsCollection : MonoBehaviour
{
    public Dictionary<string, FighterPart> AllFighterParts = new Dictionary<string, FighterPart>();
    public Dictionary<string, FighterPart> FighterParts = new Dictionary<string, FighterPart>();
    public Dictionary<string, FighterPart> EquippedFighterParts = new Dictionary<string, FighterPart>();
    public FighterPartInstancer FighterPartInstancer;
    public EquipmentGetter EquipmentGetter;
    public string PlayerID = "";

    public FighterPart GetFighterPartById(string id)
    {
        if (FighterParts.ContainsKey(id))
        {
            return FighterParts[id];
        }
        else
        {
            if (EquippedFighterParts.ContainsKey(id))
            {
                throw new Exception("FighterPart with id " + id + " is already equipped");
            }

            throw new Exception("There's no FighterPart with id " + id);
        }
    }

    public void AddFighterPart(FighterPart FighterPart)
    {
        AllFighterParts[FighterPart.ID] = FighterPart;
        if (FighterPart.Equipped)
        {
            EquippedFighterParts[FighterPart.ID] = FighterPart;
        }
        else
        {
            FighterParts[FighterPart.ID] = FighterPart;
        }
    }

    protected abstract void SetFighterPartPos(FighterPart FighterPart);

    /*public void AddToScrollList(FighterPart FighterPart)
    {
        
        ScrollListItem scrollListItem = Instantiate(ScrollListItemPref);
        scrollListItem.transform.parent = ListContent;
        scrollListItem.transform.localPosition = Vector3.zero;
        scrollListItem.OnChooseItem += EnableScroll;
        scrollListItem.Item = FighterPart;
        FighterPart.transform.parent = scrollListItem.Center.transform;
        //FighterPart.transform.position = scrollListItem.transform.position - FighterPart.Center.position;
        Vector3 wepRot = FighterPart.transform.rotation.eulerAngles;
        wepRot.z = -30;
        FighterPart.transform.rotation = Quaternion.Euler(wepRot);
        FighterPart.transform.localPosition = FighterPart.transform.position - FighterPart.Center.position;

        Vector2 contentSize = ListContent.sizeDelta;
        contentSize.y = .5f + (ListContent.childCount * .5f);
        ListContent.sizeDelta = contentSize;
        SaveFighterParts();
    }

    private void EnableScroll(ScrollListItem item, bool enable)
    {
        if (!enable)
        {
            PartDistanceEquipper.ItemToEquip = item;
            //FighterPartInfo.ItemToInfo = item;
        }

        Vector2 contentSize = ListContent.sizeDelta;
        contentSize.y = .5f + (ListContent.childCount * .5f);
        ListContent.sizeDelta = contentSize;

        ScrollRect.StopMovement();
        ScrollRect.enabled = enable;
    }

    private void GenerateWeps()
    {
        List<FighterPartJson> weps = FighterPartInstancer.FighterPartsPrefs.Select(w => w.GetJsonClass()).ToList();

        for (int i = 0; i < 30; i++)
        {
            FighterPartJson wep = weps[Random.Range(0, weps.Count)];
            wep.ID = System.Guid.NewGuid().ToString();
            FighterPart FighterPart = FighterPartInstancer.GetInstance(wep);
            switch (Random.Range(0,3))
            {
             case 0: 
                 FighterPart.ItemMaterialType = ItemMaterialType.WOOD;
                 break;
             case 1: 
                 FighterPart.ItemMaterialType = ItemMaterialType.ROCK;
                 break;
             case 2: 
                 FighterPart.ItemMaterialType = ItemMaterialType.STEEL;
                 break;
                 
            }
            SetFighterPartPos(FighterPart);
            AddFighterPart(FighterPart);
        }

        SaveFighterParts();
    }*/
}