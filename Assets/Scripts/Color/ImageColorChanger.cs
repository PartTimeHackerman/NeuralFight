using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ImageColorChanger : ColorChanger
{
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public override void changeColor(Color color)
    {
        image.color = color;
    }

    public override Color getColor()
    {
        return image.color;
    }
}
