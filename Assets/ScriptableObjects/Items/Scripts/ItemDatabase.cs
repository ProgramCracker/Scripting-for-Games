using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDatabase : ScriptableObject, ISerializationCallbackReceiver
{

    public Item[] items;
    public Dictionary<int, Item> GetItem = new Dictionary<int, Item>();

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].Id = i;
            GetItem.Add(i, items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        GetItem = new Dictionary<int, Item>();
    }

}
