using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ValBar : MonoBehaviour
{
    public Player Player;
    public Image Bar;
    public Image LerpBar;
    public Text CurrVal;
    public Text MaxVal;

    public float BarSpeed = 10f;
    public int frameUpdate = 3;
    private float MaxBarWidth;
    protected float NewCurrVal;
    private bool start = false;
    
    protected virtual void Start()
    {
        MaxBarWidth = Bar.rectTransform.sizeDelta.x;
        Vector2 barSize = Bar.rectTransform.sizeDelta;
        barSize.x = MaxBarWidth;
        Bar.rectTransform.sizeDelta = barSize;
        LerpBar.rectTransform.sizeDelta = barSize;
        NewCurrVal = MaxBarWidth;
        start = true;
        StartCoroutine(UpdateBar());

    }

    private void Update()
    {
        
    }

    protected void SetVals(float currentVal, float maxVal)
    {
        SetCurrentVal(currentVal);
        MaxVal.text = ThousandsConverter.ToKs(maxVal);
        SetBar(currentVal, maxVal);
    }

    private void SetCurrentVal(float currentVal)
    {
        CurrVal.text = ThousandsConverter.ToKs(currentVal);
    }

    private void SetBar(float currentVal, float maxVal)
    {
        NewCurrVal = Mathf.Lerp(0f, MaxBarWidth, currentVal / maxVal);
        Vector2 barSize = Bar.rectTransform.sizeDelta;
        barSize.x = NewCurrVal;
        Bar.rectTransform.sizeDelta = barSize;
    }

    public IEnumerator UpdateBar()
    {
        while (true)
        {
            if (start && Math.Abs(LerpBar.rectTransform.sizeDelta.x - NewCurrVal) > 0.01)
            {
                Vector2 barSize = LerpBar.rectTransform.sizeDelta;
                barSize.x = Mathf.Lerp(LerpBar.rectTransform.sizeDelta.x, NewCurrVal, Time.deltaTime * BarSpeed);
                LerpBar.rectTransform.sizeDelta = barSize;
            }
            for (int i = frameUpdate - 1; i >= 0; i--)
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }

}