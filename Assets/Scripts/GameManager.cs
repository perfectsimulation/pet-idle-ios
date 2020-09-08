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

        // Give the inventory and currency to the market content
        this.MenuManager.SetupMarket(this.User.Inventory, this.User.Coins);

        // Give the menu manager a callback to save item purchases
        this.MenuManager.SetupItemPurchaseCallback(this.SaveItemPurchase);

        // Give the active biome a callback to save updates to active biome
        this.MenuManager.SetupSaveBiomeCallback(this.SaveActiveBiomeState);

        // Set the active biome state from the saved data
        this.MenuManager.SetupBiome(this.User.ActiveBiomeState);
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
    public void SaveActiveBiomeState(SerializedBiomeObject updatedBiomeState)
    {
        this.User.ActiveBiomeState = updatedBiomeState;
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
