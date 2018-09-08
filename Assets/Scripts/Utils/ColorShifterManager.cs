using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

class ColorShifterManager : MonoBehaviour
{
    private List<ColorShifter> shifters;
    public int shiftersCount;
    public Color color;

    void OnEnable()
    {
        shifters = GetComponentsInChildren<ColorShifter>(true).ToList();
        shiftersCount = shifters.Count;
        color = HSBColor.ToColor(new HSBColor(Random.Range(0f, 1f), 1, 1));
        setColors(color);
    }

    public void setColors(Color color)
    {
        HSBColor hsbColor = HSBColor.FromColor(color);
        hsbColor.h += .1f;
        this.color = hsbColor.ToColor();
        foreach (ColorShifter colorShifter in shifters)
        {
            colorShifter.referenceColor = color;
        }
    }
}
