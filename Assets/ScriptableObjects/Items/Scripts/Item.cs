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
    public GameObject prefabObject;
    public ItemType type;

    [TextArea(15, 20)]
    public string description;
}
