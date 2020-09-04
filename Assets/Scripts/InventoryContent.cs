using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryContent : MonoBehaviour
{
    // The item button prefab
    public GameObject Prefab;

    // The user inventory, set from the start() of the game manager
    private Inventory Inventory;

    // The number of items in the user inventory
    private int InventoryCount;

    // The rect transform of this inventory container
    private RectTransform RectTransform;

    void Start()
    {
        this.RectTransform = this.gameObject.GetComponent<RectTransform>();
    }

    public void SetInventory(Inventory inventory)
    {
        this.Inventory = inventory;
        this.InventoryCount = inventory.Count;

        // Set the height of the rect transform so the scroll view works properly
        // TODO finalize layout and change these values accordingly
        float height = (float)this.InventoryCount;
        height /= 3f; // three items per row
        height *= (760f / 3f); // each row has height of 760/3
        Vector2 oldSize = this.RectTransform.sizeDelta;
        this.RectTransform.sizeDelta = new Vector2(oldSize.x, height);

        // Fill the inventory menu with item buttons
        this.Populate();
    }

    // Create an item button prefab for each item in the inventory
    public void Populate()
    {
        GameObject prefabObject;

        // Position is set by the grid layout script attached to this gameobject
        for (int i = 0; i < this.InventoryCount; i++)
        {
            // Instantiate the prefab clone with this as the parent
            prefabObject = Instantiate(this.Prefab, this.transform);

            // TODO Set custom properties dependent on the item
            prefabObject.name = this.Inventory[i].Name;
        }

    }

}
