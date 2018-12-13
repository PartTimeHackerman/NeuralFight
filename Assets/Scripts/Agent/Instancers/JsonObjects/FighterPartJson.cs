using System;

[Serializable]
public class FighterPartJson : ItemJson
{
    public float BaseMaxHP;
    public float BaseMaxSP;
    public float BaseHpRegen;
    public float BaseSpRegen;
    public PartType PartType;

    public FighterPartJson()
    {
    }

    public static FighterPartJson GetJson(FighterPart fighterPart)
    {
        return new FighterPartJson
        {
            ID = fighterPart.ID,
            Description = fighterPart.Description,
            Name = fighterPart.Name,
            ItemType = fighterPart.ItemType,
            ItemMaterialType = fighterPart.ItemMaterialType,
            BaseMaxHP = fighterPart.BaseMaxHP,
            BaseMaxSP = fighterPart.BaseMaxSP,
            BaseHpRegen = fighterPart.BaseHpRegen,
            BaseSpRegen = fighterPart.BaseSpRegen,
            PartType = fighterPart.PartType,
            Equipped = fighterPart.Equipped
        };
    }

    public static void SetFromJson(FighterPartJson fighterPartJson, FighterPart fighterPart)
    {
        fighterPart.ID = fighterPartJson.ID;
        fighterPart.Description = fighterPartJson.Description;
        fighterPart.ItemType = fighterPartJson.ItemType;
        fighterPart.ItemMaterialType = fighterPartJson.ItemMaterialType;
        fighterPart.BaseMaxHP = fighterPartJson.BaseMaxHP;
        fighterPart.BaseMaxSP = fighterPartJson.BaseMaxSP;
        fighterPart.BaseHpRegen = fighterPartJson.BaseHpRegen;
        fighterPart.BaseSpRegen = fighterPartJson.BaseSpRegen;
        fighterPart.PartType = fighterPartJson.PartType;
        fighterPart.Equipped = fighterPartJson.Equipped;
        fighterPart.SetUpPart();
    }
}