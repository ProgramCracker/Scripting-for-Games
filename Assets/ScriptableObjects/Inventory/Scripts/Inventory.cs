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
    
    [ContextMenu("Save")]
    public void Save()
    {
        /*
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter BF = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        BF.Serialize(file, saveData);
        file.Close();
        */

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
            /*
            BinaryFormatter BF = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(BF.Deserialize(file).ToString(), this);
            file.Close();
            */

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Container = (InventoryFilter)formatter.Deserialize(stream);
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
