using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CI.QuickSave;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerFighterPartsCollection : FighterPartsCollection
{
    public RectTransform ListContent;
    public ScrollRect ScrollRect;
    public ScrollListItem ScrollListItemPref;
    public PartDistanceEquipper PartDistanceEquipper;
    public FighterPartInfo FighterPartInfo;

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

    public void SaveFighterParts()
    {
        foreach (KeyValuePair<string, FighterPart> allFighterPart in AllFighterParts)
        {
            Storage.SaveFighterPart(allFighterPart.Value);
        }
    }

    public async Task LoadFighterParts()
    {
        List<FighterPart> parts = await EquipmentGetter.GetParts(PlayerID);

        foreach (FighterPart fighterPart in parts)
        {
            SetFighterPartPos(fighterPart);
            AddFighterPart(fighterPart);
        }

        List<FighterPart> sorted = FighterParts.Values.OrderBy(w => w.PartType).ThenByDescending(w => w.MaxHP).ToList();
        foreach (FighterPart wep in sorted)
        {
            AddToScrollList(wep);
        }
    }

    protected override void SetFighterPartPos(FighterPart fighterPart)
    {
        fighterPart.transform.position = ObjectsPositions.PlayerPartsPos;
    }

    public void AddToScrollList(FighterPart FighterPart)
    {
        ScrollListItem scrollListItem = Instantiate(ScrollListItemPref);
        scrollListItem.transform.parent = ListContent;
        scrollListItem.transform.localPosition = Vector3.zero;
        scrollListItem.OnChooseItem += EnableScroll;
        scrollListItem.OnChooseItem += (i, c) =>
        {
            if (c)
            {
                FighterPartInfo.Upgrade(i);
            }
        };
        scrollListItem.OnSelectItem += item => FighterPartInfo.SetPart(item);
        scrollListItem.OnDragItem += item => FighterPartInfo.ShowUpgrade(item);
        scrollListItem.SetItem(FighterPart);
        Vector2 contentSize = ListContent.sizeDelta;
        contentSize.y = .5f + (ListContent.childCount * .5f);
        ListContent.sizeDelta = contentSize;
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
            switch (Random.Range(0, 3))
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
    }
}