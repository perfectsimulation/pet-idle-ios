using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Menu manager
    public MenuManager MenuManager;

    // User
    private User User;

    // Load all game data
    void Awake()
    {
        this.User = Persistence.LoadUser();
        this.User.Notes.HydratePhotos(Persistence.LoadPhotos());
    }

    // Provide other scripts with game data and initialize their parameters
    void Start()
    {
        // Give the coin balance to the menu manager
        this.MenuManager.HydrateCoins(this.User.Coins);

        // Give the inventory to the inventory content
        this.MenuManager.HydrateInventory(this.User.Inventory);

        // Give the inventory to the market content
        this.MenuManager.HydrateMarket(this.User.Inventory);

        // Give the notes to the notes content
        this.MenuManager.HydrateNotes(this.User.Notes);

        // Give the gifts to the gifts content
        this.MenuManager.HydrateGifts(this.User.Gifts);

        // Give the menu manager a callback to save item purchases
        this.MenuManager.SetupPurchaseItemDelegate(this.SaveItemPurchase);

        // Give the active biome a callback to save updates to active biome
        this.MenuManager.SetupSaveBiomeDelegate(this.SaveActiveBiome);

        // Give the active biome slots a callback to save gifts
        this.MenuManager.SetupSaveVisitDelegate(this.SaveGuestVisit);

        // Give the active biome slots a callback to save gifts
        this.MenuManager.SetupSaveGiftDelegate(this.SaveGift);

        // Give the menu manager a callback to save claimed coins from gifts
        this.MenuManager.SetupClaimCoinsDelegate(this.SaveCoins);

        // Give the menu manager a callback to save claimed friendship points
        this.MenuManager.SetupClaimFriendshipDelegate(this.SaveFriendship);

        // Give the photo preview of photo capture a callback to save photos
        this.MenuManager.SetupSavePhotoDelegate(this.SavePhoto);

        // Give the photo detail of photos content a callback to delete photos
        this.MenuManager.SetupDeletePhotoDelegate(this.DeletePhoto);

        // Give the biome state of user data to the active biome
        this.MenuManager.RestoreBiomeState(this.User.BiomeState);
    }

    // Delegate called in market content to update user inventory and coins
    public void SaveItemPurchase(Item item)
    {
        // Add the newly purchased item to the user inventory
        this.User.Inventory.Add(item);

        // Subtract the newly purchased item price from the user coin balance
        this.User.Coins -= item.Price;

        // Give the updated coin balance to the menus that use it
        this.MenuManager.HydrateCoins(this.User.Coins);

        // Give the new inventory item to the inventory content
        this.MenuManager.AddInventoryItem(item);

        Persistence.SaveUser(this.User);
    }

    // Delegate called in active biome to update user biome state
    public void SaveActiveBiome(SerializedActiveBiome updatedBiomeState)
    {
        this.User.BiomeState = updatedBiomeState;

        Persistence.SaveUser(this.User);
    }

    // Delegate called when a departing guest leaves a coin award for the user
    public void SaveCoins(int coins)
    {
        // Add the coins to user balance
        this.User.Coins += coins;

        // Give the updated coin balance to the menus using it
        this.MenuManager.HydrateCoins(this.User.Coins);

        Persistence.SaveUser(this.User);
    }

    // Delegate called when guest visits the active biome
    public void SaveGuestVisit(SlotGuest slotGuest)
    {
        // Automatically update visit count when guest departs
        this.User.Notes.UpdateVisitCount(slotGuest);

        // Update notes in notes content
        this.MenuManager.UpdateNotes(slotGuest.Guest.Name, this.User.Notes);

        Persistence.SaveUser(this.User);
    }

    // Delegate called when guest departs to update notes and save the new gift
    public void SaveGift(Gift gift)
    {
        // Add the new gift to the user gifts
        this.User.Gifts.Add(gift);

        // Give the new gift to the gifts content
        this.MenuManager.AddGift(gift);

        // Update notes in notes content
        this.MenuManager.UpdateNotes(gift.Guest.Name, this.User.Notes);

        Persistence.SaveUser(this.User);
    }

    // Delegate called when a gift is claimed to update guest friendship
    public void SaveFriendship(string guestName, int friendshipPoints)
    {
        this.User.Notes.UpdateFriendship(guestName, friendshipPoints);

        // Update notes in notes content
        this.MenuManager.UpdateNotes(guestName, this.User.Notes);

        Persistence.SaveUser(this.User);
    }

    // Delegate called when a photo is saved to a guest note
    public void SavePhoto(string guestName, Photo photo)
    {
        this.User.Notes.AddPhoto(guestName, photo);

        // Update notes in notes content
        this.MenuManager.UpdateNotes(guestName, this.User.Notes);

        Persistence.SaveUser(this.User);
        Persistence.SavePhoto(guestName, photo);
    }

    // Delegate called when a photo is deleted from photo detail panel
    public void DeletePhoto(string guestName, Photo photo)
    {
        Persistence.DeletePhoto(guestName, photo);

        // Remove the photo from note in user data
        this.User.Notes[guestName].Photos.Remove(photo);

        // Update notes in notes content
        this.MenuManager.UpdateNotes(guestName, this.User.Notes);

        Persistence.SaveUser(this.User);
    }

    // Save the current user data before closing the application
    void OnApplicationQuit()
    {
        Persistence.SaveUser(this.User);
    }

}
