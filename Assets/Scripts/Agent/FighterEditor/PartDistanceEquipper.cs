using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartDistanceEquipper : MonoBehaviour
{
    public PlayerFighterPartsCollection FighterPartsCollection;
    public float minDist = 1f;
    public FighterSaverLoader FighterSaverLoader;
    private Fighter Fighter;

    
    public List<BodyPart> AllBodyParts = new List<BodyPart>();
    public List<BodyPart> AvaiableBodyParts = new List<BodyPart>();
    public BodyPart BodyPartToEquip;
    
    public ScrollListItem itemToEquip;
    public ScrollListItem ItemToEquip
    {
        get { return itemToEquip; }
        set
        {
            itemToEquip = value;
            FighterPart part = (FighterPart)itemToEquip.Item;
            SetAvaiableParts(part);
            itemToEquip.OnChooseItem += (item, choose) =>
            {
                if (BodyPartToEquip != null)
                {
                    FighterPart oldPart = BodyPartToEquip.FighterPart;
                    part.EnableOutlines(false);
                    BodyPartToEquip.FighterPart = part;
                    Destroy(itemToEquip.gameObject);
                    Storage.SaveFighterPart(oldPart);
                    Storage.SaveFighterPart(part);
                    Storage.SaveFighter(Fighter);
                    
                    FighterPartsCollection.AddToScrollList(oldPart);
                    //FighterSaverLoader.SaveFighter(Fighter);
                    itemToEquip = null;
                    BodyPartToEquip = null;
                }
            };
        }
    }

    public void SetFighter(Fighter fighter)
    {
        Fighter = fighter;
        AllBodyParts = fighter.BodyParts.AllBodyParts.Values.ToList();
        //RightArmWeapon = fighter.RightArmWeapon;
        //LeftArmWeapon = fighter.LeftArmWeapon;
    }

    private void SetAvaiableParts(FighterPart part)
    {
        AvaiableBodyParts.Clear();
        switch (part.PartType)
        {
            case PartType.BUTT:
                AvaiableBodyParts.Add(AllBodyParts.Find(p => p.name.Contains("butt")));
                break;
            case PartType.TORSO:
                AvaiableBodyParts.Add(AllBodyParts.Find(p => p.name.Contains("lwaist")));
                break;
            case PartType.HEAD:
                AvaiableBodyParts.Add(AllBodyParts.Find(p => p.name.Contains("head")));
                break;
            case PartType.UPPER_ARM:
                AvaiableBodyParts.AddRange(AllBodyParts.FindAll(p => p.name.Contains("upperarm")));
                break;
            case PartType.LOWER_ARM:
                AvaiableBodyParts.AddRange(AllBodyParts.FindAll(p => p.name.Contains("lowerarm")));
                break;
            case PartType.THIGH:
                AvaiableBodyParts.AddRange(AllBodyParts.FindAll(p => p.name.Contains("thigh")));
                break;
            case PartType.SHIN:
                AvaiableBodyParts.AddRange(AllBodyParts.FindAll(p => p.name.Contains("shin")));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void FixedUpdate()
    {
        if (ItemToEquip != null)
        {
            Vector2 itemPos = ItemToEquip.transform.position;
            float currentMinDist = minDist;
            foreach (BodyPart bodyPart in AvaiableBodyParts)
            {
                Vector2 bodyPartPos = bodyPart.transform.position;
                float dist = Vector2.Distance(itemPos, bodyPartPos);
                if (dist <= currentMinDist)
                {
                    if (BodyPartToEquip != null)
                    {
                        //armToEquip.Weapon.EnableOutlines(false);
                    }
                    currentMinDist = dist;
                    Debug.DrawRay(itemPos, bodyPartPos);
                    BodyPartToEquip = bodyPart;
                }
            }

            if (BodyPartToEquip != null)
            {
                //armToEquip.Weapon.EnableOutlines(false);
            }
            
            
        }
    }
}