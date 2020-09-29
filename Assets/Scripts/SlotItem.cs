public class SlotItem
{
    public Item Item;

    public SlotItem() { }

    /* Initialize a brand new SlotItem */
    public SlotItem(Item item)
    {
        this.Item = item;
    }

    /* Create SlotItem from save data */
    public SlotItem(SerializedItem serializedItem)
    {
        this.Item = new Item(serializedItem);
    }

    public void RemoveItem()
    {
        this.Item = null;
    }

}
