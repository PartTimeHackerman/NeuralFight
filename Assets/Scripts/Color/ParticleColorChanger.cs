using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ParticleColorChanger : ColorChanger
{
    private ParticleSystem.MainModule particle;

    void Start()
    {
        particle = GetComponent<ParticleSystem>().main;
    }

    public override void changeColor(Color color)
    {
        particle.startColor = color;
    }

    public override Color getColor()
    {
        return particle.startColor.color;
    }
}
