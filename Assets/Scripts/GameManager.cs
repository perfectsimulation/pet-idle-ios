using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Menu manager
    public MenuManager MenuManager;

    // User
    private User User;

    // Load user data from local persistence
    void Awake()
    {
        this.User = Persistence.LoadUser();
    }

    // Provide other scripts with user data and initialize their parameters
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

        // Give the photo preview of photos content a callback to save photos
        this.MenuManager.SetupSavePhotoDelegate(this.SavePhoto);

        // Give the active biome state loaded from user data to the active biome
        this.MenuManager.SetupBiome(this.User.ActiveBiome);
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

    // Delegate called in active biome to update user active biome state
    public void SaveActiveBiome(SerializedBiomeObject updatedBiomeState)
    {
        this.User.ActiveBiome = updatedBiomeState;

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
        this.MenuManager.UpdateNotes(slotGuest.Guest, this.User.Notes);

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
        this.MenuManager.UpdateNotes(gift.Guest, this.User.Notes);

        Persistence.SaveUser(this.User);
    }

    // Delegate called when a gift is claimed to update guest friendship
    public void SaveFriendship(Guest guest, int friendshipPoints)
    {
        this.User.Notes.UpdateFriendship(guest, friendshipPoints);

        // Update notes in notes content
        this.MenuManager.UpdateNotes(guest, this.User.Notes);

        Persistence.SaveUser(this.User);
    }

    // Delegate called when a photo is saved to a guest note
    public void SavePhoto(Guest guest, Photo photo)
    {
        this.User.Notes.AddPhoto(guest, photo);

        // Update notes in notes content
        this.MenuManager.UpdateNotes(guest, this.User.Notes);

        Persistence.SaveUser(this.User);
        Persistence.SavePhoto(guest, photo);
    }

    // Save the current user data before closing the application
    void OnApplicationQuit()
    {
        Persistence.SaveUser(this.User);
    }

}
