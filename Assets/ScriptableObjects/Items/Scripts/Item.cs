using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Block,
    Tool
}

public abstract class Item : ScriptableObject
{
    public int Id; 
    public Sprite UIdisplay;
    public ItemType type;

    [TextArea(15, 20)]
    public string description;
}

[System.Serializable]
public class InventoryItem
{
    public string Name;
    public int ID;
    public InventoryItem(Item item)
    {
        Name = item.name;
        ID = item.Id;
    }
}
