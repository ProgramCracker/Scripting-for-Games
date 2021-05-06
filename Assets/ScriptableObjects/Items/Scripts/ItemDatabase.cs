using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDatabase : ScriptableObject, ISerializationCallbackReceiver
{

    public Item[] items;
    public Dictionary<Item, int> GetId = new Dictionary<Item, int>();
    public Dictionary<int, Item> GetItem = new Dictionary<int, Item>();

    public void OnAfterDeserialize()
    {
        GetId = new Dictionary<Item, int>();
        GetItem = new Dictionary<int, Item>();
        for (int i = 0; i < items.Length; i++)
        {
            GetId.Add(items[i], i);
            GetItem.Add(i, items[i]);
        }
    }

    public void OnBeforeSerialize()
    {

    }

}
