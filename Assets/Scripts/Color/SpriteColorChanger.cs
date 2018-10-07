using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SpriteColorChanger : ColorChanger
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void changeColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public override Color getColor()
    {
        return spriteRenderer.color;
    }
}