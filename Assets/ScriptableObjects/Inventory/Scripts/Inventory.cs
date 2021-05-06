using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory" )]
public class Inventory : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath;
    private ItemDatabase database;
    public List<InventorySlot> Container = new List<InventorySlot>();

    private void OnEnable()
    {
#if  UNITY_EDITOR
        database = (ItemDatabase)AssetDatabase.LoadAssetAtPath("Assets/Resources/Database.asset", typeof(ItemDatabase));
#else
        database = Resource.Load<ItemDatabase>("Database");
#endif
    }

    public void AddItem(Item _item, int _amount)
    {

        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item && Container[i].amount < 64)
            {
                Container[i].AddAmount(_amount);
                return;
            }
        }
            Container.Add(new InventorySlot(database.GetId[_item], _item, _amount));
    }

    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter BF = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        BF.Serialize(file, saveData);
        file.Close();
    }

    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter BF = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(BF.Deserialize(file).ToString(), this);
            file.Close();
        }
    }

    public void OnAfterDeserialize()
    {
       for (int i = 0; i < Container.Count; i++)
            Container[i].item = database.GetItem[Container[i].ID];
        
    }

    public void OnBeforeSerialize()
    {
        
    }
}

[System.Serializable]
public class InventorySlot
{
    public int ID;
    public Item item;
    public int amount;
    public InventorySlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}
