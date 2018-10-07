using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

class ColorShifter : MonoBehaviour
{
    public Color referenceColor;
    public static float currentDist = 0f;
    public static float maxDist = 1000f;
    private SpriteRenderer sprite;
    private Image image;
    private bool firsColorDone = false;
    public float whiteBlackMax = 0f;
    public float firstColorHue = 0f;
    public float newHue;
    private Color newColor;

    private Color firstColor;
    private Color lastColor;

    void Start()
    {
        //referenceColor = HSBColor.ToColor(new HSBColor(Random.Range(0f, 1f), 1, 1));

        sprite = GetComponent<SpriteRenderer>();
        if (sprite == null)
            image = GetComponent<Image>();

        firstColor = referenceColor;
        lastColor = invertColor(referenceColor);
        StartCoroutine(updateColor(1f));

    }


    IEnumerator updateColor(float wait)
    {
        while (true)
        {
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

            if (sprite != null)
            {
                sprite.color = newColor;

            }
            else
            {
                image.color = newColor;
            }

            yield return new WaitForSeconds(wait);

        }
    }

    public Color invertColor(Color color)
    {
        return new Color(1.0f - color.r, 1.0f - color.g, 1.0f - color.b);
    }

}
