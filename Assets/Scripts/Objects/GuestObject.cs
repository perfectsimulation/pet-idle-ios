using UnityEditor;
using UnityEngine;

public class GuestObject : MonoBehaviour
{
    public Guest Guest;
    public Texture2D GuestImage;

    public void SetGuest(Guest guest)
    {
        this.Guest = guest;
        // TODO add a renderer to the Slot prefab to use this
        this.GuestImage = (Texture2D)AssetDatabase.LoadAssetAtPath(guest.ImageAssetPathname, typeof(Texture2D));
    }

}
