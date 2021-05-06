using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShowInventory : MonoBehaviour
{
    public GameObject inventoryPrefab;
    public Inventory inventory;

    public int inventoryStartX;
    public int inventoryStartY;
    public int spaceBetweenItemsOnX;
    public int spaceBetweenItemsOnY;

    public int amountOfColumns;

    Dictionary<GameObject, InventorySlot> itemsShown = new Dictionary<GameObject, InventorySlot> ();
    // Start is called before the first frame update
    void Start()
    {
        CreateSlots();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlots();
    }

    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in itemsShown)
        {
            if(_slot.Value.ID >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[_slot.Value.item.ID].UIdisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    public void CreateSlots()
    {
        itemsShown = new Dictionary<GameObject, InventorySlot>();
        for (int  i = 0;  i < inventory.Container.Items.Length;  i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPos(i);

            itemsShown.Add(obj, inventory.Container.Items[i]);
        }
        
    }

    public Vector3 GetPos(int i)
    {
        return new Vector3(inventoryStartX + (spaceBetweenItemsOnX *(i % amountOfColumns)),(inventoryStartY - (spaceBetweenItemsOnY * (i/amountOfColumns))), 0f);
    }
    
}
