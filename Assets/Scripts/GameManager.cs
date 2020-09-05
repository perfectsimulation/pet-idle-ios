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

        // Give the user inventory data to the inventory content
        this.MenuManager.SetupInventory(this.User.Inventory);

        // Give the active biome a callback to update the user with new biome states
        this.ActiveBiome.SetupSaveUserCallback(this.SaveUserActiveBiomeState);

        // Set the active biome from the saved data
        this.ActiveBiome.SetupBiome(this.User.ActiveBiomeState.Biome);

        // Fill the active biome slots from the saved data
        this.ActiveBiome.LayoutSavedSlots(this.User.ActiveBiomeState.Slots);
    }

    // Delegate called in inventory content to slot the selected item
    public void SelectItemForSlotPlacement(Item item)
    {
        this.ActiveBiome.SelectItemForSlotPlacement(item);
    }

    // Delegate called in active biome to save user data with updated biome state
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
