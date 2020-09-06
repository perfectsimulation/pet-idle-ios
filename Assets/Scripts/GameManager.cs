using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Menu manager
    public MenuManager MenuManager;

    // Active biome
    public BiomeObject ActiveBiome;

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
        // Give the menu manager a callback to select an item to place in a slot
        this.MenuManager.SetupItemPlacementCallback(this.SelectItemForSlotPlacement);

        // Give the user inventory data to the inventory content in the menu manager
        this.MenuManager.SetupInventory(this.User.Inventory);

        // Give the inventory and currency to the market content in the menu manager
        this.MenuManager.SetupMarket(this.User.Inventory, this.User.Coins);

        // Give the menu manager a callback to buy and add an item to user inventory
        this.MenuManager.SetupItemPurchaseCallback(this.SaveUserPurchase);

        // Give the active biome a callback to update the user with new biome states
        this.ActiveBiome.SetupSaveUserCallback(this.SaveUserActiveBiomeState);

        // Set the active biome from the saved data
        this.ActiveBiome.SetupBiome(this.User.ActiveBiomeState.Biome);

        // Fill the active biome slots from the saved data
        this.ActiveBiome.LayoutSavedSlots(this.User.ActiveBiomeState.Slots);
    }

    // Delegate called in inventory content to enable placement of selected item
    public void SelectItemForSlotPlacement(Item item)
    {
        this.ActiveBiome.SelectItemForSlotPlacement(item);
    }

    // Delegate called in market content to update user inventory and coins
    public void SaveUserPurchase(Item item)
    {
        // Add the newly purchased item to the user inventory
        this.User.Inventory.Add(item);

        // Subtract the newly purchased item price from the user coin balance
        this.User.Coins -= item.Price;

        // Update menus that need inventory data TODO optimize this
        this.MenuManager.SetupInventory(this.User.Inventory);
        this.MenuManager.SetupMarket(this.User.Inventory, this.User.Coins);

        Persistence.SaveUser(this.User);
    }

    // Delegate called in active biome to update user active biome state
    public void SaveUserActiveBiomeState(SerializedBiomeObject updatedBiomeState)
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
