using System;
using System.Collections.Generic;
using System.Linq;
using CI.QuickSave;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FighterPartsCollection : MonoBehaviour
{
    public Dictionary<int, FighterPart> AllFighterParts = new Dictionary<int, FighterPart>();
    public Dictionary<int, FighterPart> FighterParts = new Dictionary<int, FighterPart>();
    public Dictionary<int, FighterPart> EquippedFighterParts = new Dictionary<int, FighterPart>();
    public FighterPartInstancer FighterPartInstancer;

    public bool saveFighterParts = false;
    public FighterPart FighterPart;
    public bool addFighterPart = false;
    private float posX = 0f;

    private QuickSaveSettings settings = new QuickSaveSettings();

    private Transform FighterPartsParent;

    public RectTransform ListContent;
    public ScrollRect ScrollRect;
    public ScrollListItem ScrollListItemPref;
    public PartDistanceEquipper PartDistanceEquipper;

    private void Start()
    {
        FighterPartsParent = GetComponentsInChildren<Transform>().First(t => t.name.Equals("FighterParts"));
        //GenerateWeps();
    }

    private void FixedUpdate()
    {
        if (saveFighterParts)
        {
            SaveFighterParts();
            saveFighterParts = false;
        }

        if (addFighterPart)
        {
            AddFighterPart(FighterPart);
            FighterPart = null;
            addFighterPart = false;
        }
    }

    public FighterPart GetFighterPartById(int id)
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

    private void SaveFighterParts()
    {
        List<FighterPartJson> FighterPartsJsons = new List<FighterPartJson>();
        foreach (KeyValuePair<int, FighterPart> FighterPart in AllFighterParts)
        {
            FighterPartsJsons.Add(FighterPart.Value.GetJsonClass());
        }

        QuickSaveWriter writer = QuickSaveWriter.Create("FighterParts", settings);
        //writer.Write("FighterParts", JsonUtility.ToJson(new JsonList<FighterPartJson>(FighterPartsJsons)));
        foreach (FighterPartJson FighterPartsJson in FighterPartsJsons)
        {
            writer.Write(FighterPartsJson.ID.ToString(), JsonUtility.ToJson(FighterPartsJson));
        }

        writer.Commit();
    }

    public void LoadFighterParts()
    {
        QuickSaveReader reader = QuickSaveReader.Create("FighterParts", settings);
        //List<FighterPartJson> FighterPartJsons = JsonUtility.FromJson<JsonList<FighterPartJson>>(reader.Read<string>("FighterParts")).list;

        foreach (string key in reader.GetAllKeys())
        {
            FighterPartJson FighterPartJson = JsonUtility.FromJson<FighterPartJson>(reader.Read<string>(key));
            FighterPart FighterPart = FighterPartInstancer.GetInstance(FighterPartJson);
            SetFighterPartPos(FighterPart);
            AddFighterPart(FighterPart);
            //FighterPart.transform.parent = FighterPartsParent;
        }


        List<FighterPart> sorted = FighterParts.Values.OrderBy(w => w.PartType).ThenByDescending(w => w.MaxHP).ToList();
        foreach (FighterPart wep in sorted)
        {
            AddToScrollList(wep);
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

    private void SetFighterPartPos(FighterPart FighterPart)
    {
        FighterPart.transform.position = ObjectsPositions.PlayerPartsPos;
    }

    public void AddToScrollList(FighterPart FighterPart)
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
            wep.ID = i;
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
    }
}