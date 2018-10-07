using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class GroupColorManager : MonoBehaviour
{
    public GroupType type;
    public string name = "";
    public Color firstColor = Color.white;
    public Color lastColor = Color.black;


    void Start()
    {
        List<ColorChanger> changers = new List<ColorChanger>();

        foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>().ToList())
        {
            spriteRenderer.gameObject.AddComponent<SpriteColorChanger>();
            changers.Add(spriteRenderer.GetComponent<SpriteColorChanger>());
        }

        foreach (ParticleSystem particleSystem in GetComponentsInChildren<ParticleSystem>().ToList())
        {
            particleSystem.gameObject.AddComponent<ParticleColorChanger>();
            changers.Add(particleSystem.GetComponent<ParticleColorChanger>());
        }

        if (type == GroupType.CUSTOM)
            GroupColorChanger.get(name, firstColor, lastColor).addChangers(changers);
        else
            GroupColorChanger.get(type, firstColor, lastColor).addChangers(changers);

    }
}
