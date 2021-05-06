using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory inventory;

    public Item Dirt;
    public Item CobbleStone;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Dirt)
            {
                inventory.AddItem(new InventoryItem(Dirt), 1);
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (CobbleStone)
            {
                inventory.AddItem(new InventoryItem(CobbleStone), 1);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            inventory.Load();
        }
    }

    private void OnApplicationQuit()
    {
       inventory.Container.Items = new InventorySlot[27];
    }
}
