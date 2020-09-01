using UnityEditor;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public Item Item;
    public Texture2D ItemImage;

    public void SetItem(Item item)
    {
        this.Item = item;
        // TODO add a renderer to the Slot prefab to use this
        this.ItemImage = (Texture2D)AssetDatabase.LoadAssetAtPath(item.ImageAssetPathname, typeof(Texture2D));
    }

}
