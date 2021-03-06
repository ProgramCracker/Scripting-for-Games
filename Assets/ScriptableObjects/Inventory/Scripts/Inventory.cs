using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory" )]
public class Inventory : ScriptableObject
{
    public string savePath;
    public ItemDatabase database;
    public InventoryFilter Container;

    public void AddItem(InventoryItem _item, int _amount)
    {
        

        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID == _item.ID && Container.Items[i].amount < 64)
            {
                Container.Items[i].AddAmount(_amount);
                return;
            }
        }
        setEmptySlot(_item, _amount);
           
        
    }

    public InventorySlot setEmptySlot(InventoryItem _item, int _amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if(Container.Items[i].ID <= -1)
            {
                Container.Items[i].UpdateSlot(_item.ID, _item, _amount);
                return Container.Items[i];
            }
        }

        return null;
    }

    public void MoveItem(InventorySlot item1, InventorySlot item2)
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            InventorySlot temp = new InventorySlot(item2.ID, item2.item, item2.amount);
            item2.UpdateSlot(item1.ID, item1.item, item1.amount);
            item1.UpdateSlot(temp.ID, temp.item, temp.amount);
        }
        else
        {

            InventorySlot temp = new InventorySlot(item2.ID, item2.item, item2.amount);
            item2.UpdateSlot(item1.ID, item1.item, item1.amount);
            item1.UpdateSlot(temp.ID, temp.item, temp.amount);
        }
    }

    public void RemoveItem(InventoryItem _item)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if(Container.Items[i].item == _item)
            {
                Container.Items[i].UpdateSlot(-1, null, 0);
            }
        }
    }
    
    [ContextMenu("Save")]
    public void Save()
    {

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            InventoryFilter newContainer = (InventoryFilter)formatter.Deserialize(stream);
            for (int i = 0; i < Container.Items.Length; i++)
            {
                Container.Items[i].UpdateSlot(newContainer.Items[i].ID, newContainer.Items[i].item, newContainer.Items[i].amount);
            }
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container = new InventoryFilter();
    }

}

[System.Serializable]
public class InventorySlot
{
    public int ID = -1;
    public InventoryItem item;
    public int amount;

    public InventorySlot()
    {
        ID = -1;
        item = null;
        amount = 0;
    }

    public InventorySlot(int _id, InventoryItem _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }

    public void UpdateSlot(int _id, InventoryItem _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }
}



[System.Serializable]
public class InventoryFilter
{
    public InventorySlot[] Items = new InventorySlot[27];
}
