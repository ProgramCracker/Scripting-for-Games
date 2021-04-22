using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowInventory : MonoBehaviour
{
    public Inventory inventory;

    public int inventoryStartX;
    public int inventoryStartY;
    public int spaceBetweenItemsOnX;
    public int spaceBetweenItemsOnY;

    public int amountOfColumns;

    Dictionary<InventorySlot, GameObject> itemsShown = new Dictionary<InventorySlot, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
       UpdateDisplay();
    }
    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (itemsShown.ContainsKey(inventory.Container[i]))
            {
                itemsShown[inventory.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(inventory.Container[i].item.prefabObject, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPos(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
                itemsShown.Add(inventory.Container[i], obj);
            }
        }
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            var obj = Instantiate(inventory.Container[i].item.prefabObject, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPos(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
            itemsShown.Add(inventory.Container[i], obj);
        }
    }

    public Vector3 GetPos(int i)
    {
        return new Vector3(inventoryStartX + (spaceBetweenItemsOnX *(i % amountOfColumns)),(inventoryStartY - (spaceBetweenItemsOnY * (i/amountOfColumns))), 0f);
    }
}
