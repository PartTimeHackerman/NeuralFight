using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    public List<int> Indexes = new List<int>();

    public MeshRenderer Renderer;

    private Dictionary<Material, Color> MaterialColors = new Dictionary<Material, Color>();

    public void Awake()
    {
        Renderer = GetComponent<MeshRenderer>();
    }

    public void ChangeMaterial(ItemMaterial itemMaterial)
    {
        Renderer = GetComponent<MeshRenderer>();
        Material[] materials = Renderer.materials;
        MaterialColors.Clear();
        for (int i = 0; i < Indexes.Count; i++)
        {
            Material material = Instantiate(itemMaterial.Materials[Indexes[i] - 1]);
            Destroy(materials[i]);
            materials[i] = material;
            MaterialColors[materials[i]] = materials[i].color;
        }
        Renderer.materials = materials;
    }

    public void ChangeDamagedColor(float damage)
    {
        float dimm = .5f + (damage * .5f);
        foreach (KeyValuePair<Material,Color> materialColor in MaterialColors)
        {
            Color baseColor = materialColor.Value;
            Material material = materialColor.Key;
            HSBColor hsbColor = new HSBColor(baseColor);
            hsbColor.b = Mathf.Lerp(.5f, hsbColor.b, damage);
            material.color = hsbColor.ToColor();

        }
    }
}