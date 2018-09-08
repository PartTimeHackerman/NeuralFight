using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

class ColorShifter : MonoBehaviour
{
    public Color referenceColor;
    public static float currentDist = 0f;
    public static float maxDist = 1000f;
    private SpriteRenderer sprite;
    private bool firsColorDone = false;
    public float whiteBlackMax = 0f;
    public float firstColorHue = 0f;
    public float newHue;
    private Color newColor;

    void Start()
    {
        //referenceColor = HSBColor.ToColor(new HSBColor(Random.Range(0f, 1f), 1, 1));
        
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        whiteBlackMax = maxDist * 0.16666f;
        firstColorHue = HSBColor.FromColor(referenceColor).h;
        currentDist = Mathf.Clamp(currentDist, 0f, maxDist);
        if (currentDist <= whiteBlackMax)
        {
            newColor = Color.Lerp(Color.white, referenceColor, currentDist / whiteBlackMax);
        }

        else if (currentDist >= maxDist - whiteBlackMax)
        {
            newColor = Color.Lerp(referenceColor, Color.black, Mathf.Abs(((maxDist - currentDist) / whiteBlackMax) - 1f));
        }
        else
        {
            newHue = (currentDist - whiteBlackMax) / (maxDist - whiteBlackMax * 2f) + firstColorHue;
            newHue = newHue > 1 ? Mathf.Abs(1f - newHue) : newHue;
            newColor = HSBColor.ToColor(new HSBColor(newHue, 1, 1));
        }

        sprite.color = newColor;
    }

}
