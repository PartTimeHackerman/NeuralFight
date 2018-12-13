using System.Collections.Generic;
using UnityEngine;

public class FighterPartInstancer : MonoBehaviour
{
    public List<FighterPart> FighterPartsPrefs = new List<FighterPart>();
    private Dictionary<PartType, FighterPart> FighterPartsPrefabsDict = new Dictionary<PartType, FighterPart>();

    private void Awake()
    {
        foreach (FighterPart FighterPartsPref in FighterPartsPrefs)
        {
            FighterPartsPrefabsDict[FighterPartsPref.PartType] = FighterPartsPref;
        }
    }

    public FighterPart GetInstance(FighterPartJson FighterPartJson)
    {
        FighterPart FighterPart = Instantiate(FighterPartsPrefabsDict[FighterPartJson.PartType]);
        FighterPart.SetFromJson(FighterPartJson);
        return FighterPart;
    }
}