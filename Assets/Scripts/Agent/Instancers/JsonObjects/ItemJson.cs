using System;

[Serializable]
public class ItemJson
{
    public int ID;
    public string Name;
    public string Description;
    public ItemType ItemType;
    public ItemMaterialType ItemMaterialType;
    public int Level;
    public int Stars;
    public bool Equipped = false;

}