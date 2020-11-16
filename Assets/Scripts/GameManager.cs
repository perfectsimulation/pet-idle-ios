using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MenuManager MenuManager;
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

        // Give the menu manager a callback to save food purchases
        this.MenuManager.DelegatePurchaseFood(this.SaveFoodPurchase);

        // Give the menu manager a callback to save item purchases
        this.MenuManager.DelegatePurchaseItem(this.SaveItemPurchase);

        // Give the active biome a callback to save updates to active biome
        this.MenuManager.DelegateSaveBiome(this.SaveActiveBiome);

        // Give the active biome slots a callback to save gifts
        this.MenuManager.DelegateSaveVisits(this.SaveVisits);

        // Give the active biome slots a callback to save gifts
        this.MenuManager.DelegateSaveGifts(this.SaveGifts);

        // Give the menu manager a callback to claim gifts
        this.MenuManager.DelegateClaimGifts(this.ClaimGifts);

        // Give the photo preview of photo capture a callback to save photos
        this.MenuManager.DelegateSavePhoto(this.SavePhoto);

        // Give the photo detail of photos content a callback to delete photos
        this.MenuManager.DelegateDeletePhoto(this.DeletePhoto);

        // Give the biome state of user data to the active biome
        this.MenuManager.RestoreBiomeState(this.User.BiomeState);
    }

    // Delegate called in food menu to refill meal in active biome
    private void SaveFoodPurchase(Food food)
    {
        // Subtract the newly purchased food price from the user coin balance
        this.User.Coins -= food.Price;

        // Give the updated coin balance to the menus that use it
        this.MenuManager.HydrateCoins(this.User.Coins);

        // Give the new food to the active biome
        this.MenuManager.AddFoodToBiome(food);

        Persistence.SaveUser(this.User);
    }

    // Delegate called in market content to update user inventory and coins
    private void SaveItemPurchase(Item item)
    {
        // Subtract the newly purchased item price from the user coin balance
        this.User.Coins -= item.Price;

        // Add the newly purchased item to the user inventory
        this.User.Inventory.Add(item);

        // Give the updated coin balance to the menus that use it
        this.MenuManager.HydrateCoins(this.User.Coins);

        // Give the new inventory item to the inventory content
        this.MenuManager.AddInventoryItem(item);

        Persistence.SaveUser(this.User);
    }

    // Delegate called in active biome to update user biome state
    private void SaveActiveBiome(SerializedActiveBiome updatedBiomeState)
    {
        this.User.BiomeState = updatedBiomeState;

        Persistence.SaveUser(this.User);
    }

    // Delegate called when started visits are processed in visit schedule
    private void SaveVisits(Visit[] visits)
    {
        // Update visit counts for each guest of a started visit
        this.User.Notes.UpdateVisitCounts(visits);

        // Update notes in notes content
        this.MenuManager.UpdateNotes(this.User.Notes);

        Persistence.SaveUser(this.User);
    }

    // Delegate called when ended visits are processed in visit schedule
    private void SaveGifts(Visit[] visits)
    {
        // Create a new gift from each ended visit
        this.User.Gifts.Create(visits);

        // Update gifts in gifts content with new gifts
        this.MenuManager.UpdateGifts();

        Persistence.SaveUser(this.User);
    }

    // Delegate called to save all rewards when gifts are claimed
    private void ClaimGifts(Gifts gifts)
    {
        // Add coin reward to user coin balance
        this.User.Coins += gifts.GetTotalCoins();

        // Add friendship rewards to user notes
        this.User.Notes.UpdateFriendships(gifts);

        // Clear all claimed gifts
        this.User.Gifts.Clear();

        // Update the updated coin balance in menus
        this.MenuManager.HydrateCoins(this.User.Coins);

        // Update notes in notes content
        this.MenuManager.UpdateNotes(this.User.Notes);

        // Update gifts in gifts content
        this.MenuManager.UpdateGifts();

        Persistence.SaveUser(this.User);
    }

    // Delegate called when a photo is saved to a guest note
    private void SavePhoto(string guestName, Photo photo)
    {
        // Add the photo to the note in user data
        this.User.Notes.AddPhoto(guestName, photo);

        // Update note in notes content
        this.MenuManager.UpdateNote(guestName, this.User.Notes);

        Persistence.SaveUser(this.User);
        Persistence.SavePhoto(guestName, photo);
    }

    // Delegate called when a photo is deleted from photo detail panel
    private void DeletePhoto(string guestName, Photo photo)
    {
        Persistence.DeletePhoto(guestName, photo);

        // Remove the photo from note in user data
        this.User.Notes[guestName].Photos.Remove(photo);

        // Update note in notes content
        this.MenuManager.UpdateNote(guestName, this.User.Notes);

        Persistence.SaveUser(this.User);
    }

    // Save the current user data before closing the application
    void OnApplicationQuit()
    {
        // Make any necessary visit schedule adjustments
        this.MenuManager.ProcessBiomeStateChanges();

        Persistence.SaveUser(this.User);
    }

}
