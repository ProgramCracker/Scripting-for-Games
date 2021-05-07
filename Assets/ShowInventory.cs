using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ShowInventory : MonoBehaviour
{
    public MouseItem mouseItem = new MouseItem();

    public GameObject inventoryPrefab;
    public Inventory inventory;

    public int inventoryStartX;
    public int inventoryStartY;
    public int spaceBetweenItemsOnX;
    public int spaceBetweenItemsOnY;

    public int amountOfColumns;

    Dictionary<GameObject, InventorySlot> itemsShown = new Dictionary<GameObject, InventorySlot> ();

    void Start()
    {
        CreateSlots();
    }

    void Update()
    {
        UpdateSlots();
    }

    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in itemsShown) // for every item currently in the inventory check...
        {
            if(_slot.Value.ID >= 0) // if there is no item in the slot, look like this
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[_slot.Value.item.ID].UIdisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            else // if there are items in the slot, look like this
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

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            itemsShown.Add(obj, inventory.Container.Items[i]);
        }
        
    }

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj) // when the player's cursor hovers over an inventory slot
    {
        mouseItem.hoverObj = obj;
        if (itemsShown.ContainsKey(obj))
        {
            mouseItem.hoverItem = itemsShown[obj];
        }
    }

    public void OnExit(GameObject obj) // when the player's cursor leaves an inventory slot
    {
        mouseItem.hoverObj = null;

        mouseItem.hoverItem = null;
        
    }

    public void OnDragStart(GameObject obj) // when the player starts to drag
    {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(36, 36);
        mouseObject.transform.SetParent(transform.parent);
        if(itemsShown[obj].ID >= 0) // identifies the item the user has clicked on
        {
            //retrieve the sprite from the target item
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventory.database.GetItem[itemsShown[obj].ID].UIdisplay;
            img.raycastTarget = false;
        }
        mouseItem.obj = mouseObject;
        mouseItem.item = itemsShown[obj];
    }

    public void OnDragEnd(GameObject obj) // when the player lets go of the item
    {
        if(mouseItem.hoverObj) // if there is an object in the target slot
        {
            inventory.MoveItem(itemsShown[obj], itemsShown[mouseItem.hoverObj]);
        }
        else //if the location is empty
        {
            inventory.RemoveItem(itemsShown[obj].item);
        }
        Destroy(mouseItem.obj);
        mouseItem.item = null;
    }

    public void OnDrag(GameObject obj) // while the player is dragging the item
    {
        if (mouseItem.obj != null)
            mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public Vector3 GetPos(int i)
    {
        return new Vector3(inventoryStartX + (spaceBetweenItemsOnX *(i % amountOfColumns)),(inventoryStartY - (spaceBetweenItemsOnY * (i/amountOfColumns))), 0f);
    }
    
}

public class MouseItem
{
    public GameObject obj;
    public InventorySlot item;
    public InventorySlot hoverItem;
    public GameObject hoverObj;
}
