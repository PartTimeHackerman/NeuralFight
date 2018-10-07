using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class GroupColorChanger : MonoBehaviour
{

    public static float currentDist = 0f;
    private float maxDist = 1000f;
    private bool firsColorDone = false;
    private float whiteBlackMax = 0f;
    private float firstColorHue = 0f;
    private float newHue;
    private Color newColor;

    public Color referenceColor;
    public Color firstColor = Color.white;
    public Color lastColor = Color.black;

    private List<ColorChanger> colorChangers;
    public GroupType type = GroupType.CUSTOM;

    public static GroupColorChanger get(string name, Color firstColor, Color lastColor)
    {
        if (!ColorChangerHelper.colorChangersDict.ContainsKey(name))
        {
            var go = new GameObject("ColorChanger_" + name);
            go.AddComponent<GroupColorChanger>();
            GroupColorChanger colorChanger = go.GetComponent<GroupColorChanger>();
            colorChanger.firstColor = firstColor;
            colorChanger.lastColor = lastColor;
            //colorChanger.referenceColor = refColor;
            colorChanger.Init();
            ColorChangerHelper.colorChangersDict[name] = colorChanger;
        }
        return ColorChangerHelper.colorChangersDict[name];
    }
    public static GroupColorChanger get(GroupType type, Color firstColor, Color lastColor)
    {
        string name = type.ToString("G");
        if (!ColorChangerHelper.colorChangersDict.ContainsKey(name))
        {
            var go = new GameObject("ColorChanger_" + name);
            go.AddComponent<GroupColorChanger>();
            GroupColorChanger colorChanger = go.GetComponent<GroupColorChanger>();
            colorChanger.firstColor = firstColor;
            colorChanger.lastColor = lastColor;
            colorChanger.type = type;
            colorChanger.Init();
            ColorChangerHelper.colorChangersDict[name] = colorChanger;
        }
        return ColorChangerHelper.colorChangersDict[name];
    }

    public void Init()
    {
        colorChangers = new List<ColorChanger>();
        //referenceColor = new HSBColor(Random.Range(0f, 1f), 1f, 1f).ToColor();
        referenceColor = GroupColors.get().getGroupColor(type);
        StartCoroutine(updateColor(.1f));
    }

    IEnumerator updateColor(float wait)
    {
        while (true)
        {
            currentDist = Mathf.Clamp(currentDist, 0f, maxDist);
            whiteBlackMax = maxDist * 0.16666f;
            firstColorHue = HSBColor.FromColor(referenceColor).h;
            currentDist = Mathf.Clamp(currentDist, 0f, maxDist);
            if (currentDist <= whiteBlackMax)
            {
                newColor = Color.Lerp(firstColor, referenceColor, currentDist / whiteBlackMax);
            }

            else if (currentDist >= maxDist - whiteBlackMax)
            {
                newColor = Color.Lerp(referenceColor, lastColor, Mathf.Abs(((maxDist - currentDist) / whiteBlackMax) - 1f));
            }
            else
            {
                newHue = (currentDist - whiteBlackMax) / (maxDist - whiteBlackMax * 2f) + firstColorHue;
                newHue = newHue > 1 ? Mathf.Abs(1f - newHue) : newHue;
                newColor = HSBColor.ToColor(new HSBColor(newHue, 1, 1));
            }

            foreach (ColorChanger colorChanger in colorChangers)
            {
                colorChanger.changeColor(newColor);
            }

            yield return new WaitForSeconds(wait);

        }
    }

    public void addChangers(List<ColorChanger> changers)
    {
        colorChangers.AddRange(changers);
    }

    public void addChanger(ColorChanger changer)
    {
        colorChangers.Add(changer);
    }

}

public class ColorChangerHelper
{
    public static Dictionary<string, GroupColorChanger> colorChangersDict =
         new Dictionary<string, GroupColorChanger>();
}

public class GroupColors : MonoBehaviour
{
    public Color[] colors = new Color[6];
    private static GroupColors groupColors;

    public static GroupColors get()
    {
        if (groupColors == null)
        {
            var go = new GameObject("GroupColors");
            groupColors = go.AddComponent<GroupColors>();
            groupColors.Init();
        }

        return groupColors;
    }

    public void Init()
    {
        float random = Random.Range(0f, 1f);
        float current = random;
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = new HSBColor(current, 1f, 1f).ToColor();
            current += .1667f;
            current = current > 1f ? current - 1f : current;
        }

    }

    public Color getGroupColor(GroupType type)
    {
        switch (type)
        {
            case GroupType.CUSTOM: return new HSBColor(Random.Range(0f, 1f), 1f, 1f).ToColor();
            case GroupType.OBSTACLES: return colors[0];
            case GroupType.OUTLINES: return colors[2];
            case GroupType.PLAYER: return colors[1];
            case GroupType.BG_BACK: return colors[3];
            case GroupType.BG_FRONT: return colors[4];
            case GroupType.BG_PARTICLES: return colors[5];
            default: return new HSBColor(Random.Range(0f, 1f), 1f, 1f).ToColor();
        }
    }

}

public enum GroupType
{
    CUSTOM,
    OBSTACLES,
    OUTLINES,
    PLAYER,
    BG_BACK,
    BG_FRONT,
    BG_PARTICLES

}
