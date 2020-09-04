using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public Item Item;

    public void SetItem(Item item)
    {
        this.Item = item;
    }

    public void RemoveItem()
    {
        this.Item = null;
    }

}
