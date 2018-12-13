using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    public List<int> Indexes = new List<int>();

    public MeshRenderer Renderer;

    public void Awake()
    {
        Renderer = GetComponent<MeshRenderer>();
    }

    public void ChangeMaterial(ItemMaterial itemMaterial)
    {
        Renderer = GetComponent<MeshRenderer>();
        Material[] materials = Renderer.materials;
        for (int i = 0; i < Indexes.Count; i++)
        {
            materials[i] = itemMaterial.Materials[Indexes[i] - 1];
        }
        Renderer.materials = materials;
    }
}