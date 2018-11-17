using System;
using UnityEngine;

public static class ThousandsConverter
{

    public static string ToKs(float value)
    {
        value = value > 0 && value < .49 ? 1f : value;
        value = (int)Mathf.Round(value);
        float strVal = 0f;
        string strEnd = "";
        if (value < 1000)
        {
            strVal = value;
            return value.ToString();
        }

        if (value >= 1000 && value < 1000000)
        {
            
            strVal = (value / 1000f);
            strEnd = "k";
        }
        
        if (value >= 1000000)
        {
            strVal = (value / 1000000f);
            strEnd = "kk";
        }
        
        return OneDecimalPlace(strVal) + strEnd;
    }

    private static string OneDecimalPlace(float val)
    {
        return String.Format("{0:0.0}", val);
    }
}