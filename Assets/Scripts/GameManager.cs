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
        // Give the user inventory data to the inventory content
        this.MenuManager.SetupInventory(this.User.Inventory);

        // Give the inventory and coin balance to the market content
        this.MenuManager.SetupMarket(this.User.Inventory, this.User.Coins);

        // Give the notes to the notes content
        this.MenuManager.SetupNotes(this.User.Notes);

        // Give the gifts to the gifts content
        this.MenuManager.SetupGifts(this.User.Gifts);

        // Give the menu manager a callback to save item purchases
        this.MenuManager.SetupPurchaseItemDelegate(this.SaveItemPurchase);

        // Give the active biome a callback to save updates to active biome
        this.MenuManager.SetupSaveBiomeDelegate(this.SaveActiveBiome);

        // Give the active biome slots a callback to save coins
        this.MenuManager.SetupSaveCoinsDelegate(this.SaveCoins);

        // Give the active biome slots a callback to save notes
        this.MenuManager.SetupSaveNotesDelegate(this.SaveNotes);

        // Set the active biome state from the saved data
        this.MenuManager.SetupBiome(this.User.ActiveBiome);
    }

    // Delegate called when a departing guest leaves a coin award for the user
    public void SaveCoins(int coins)
    {
        // Add the coins to user balance
        this.User.Coins += coins;

        Persistence.SaveUser(this.User);
    }

    // Delegate called in market content to update user inventory and coins
    public void SaveItemPurchase(Item item)
    {
        // Add the newly purchased item to the user inventory
        this.User.Inventory.Add(item);

        // Subtract the newly purchased item price from the user coin balance
        this.User.Coins -= item.Price;

        // Update inventory content with newly purchased item
        this.MenuManager.UpdateInventory(item);

        Persistence.SaveUser(this.User);
    }

    // Delegate called in active biome to update user active biome state
    public void SaveActiveBiome(SerializedBiomeObject updatedBiomeState)
    {
        this.User.ActiveBiome = updatedBiomeState;

        Persistence.SaveUser(this.User);
    }

    // Delegate called when a guest departs to update its entry in user notes
    public void SaveNotes(GuestObject guestObject)
    {
        this.User.Notes.UpdateVisitCount(guestObject);

        Persistence.SaveUser(this.User);
    }

    // Save the current user data before closing the application
    void OnApplicationQuit()
    {
        Persistence.SaveUser(this.User);
    }

    // Save the current user data if the application is suspended
    void OnApplicationPause(bool isPaused)
    {
        Persistence.SaveUser(this.User);
    }

}
