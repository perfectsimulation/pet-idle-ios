public class SlotItem
{
    public Item Item;

    public SlotItem() { }

    /* Initialize a brand new SlotItem */
    public SlotItem(Item item)
    {
        this.Item = item;
    }

    public void RemoveItem()
    {
        this.Item = null;
    }

}
