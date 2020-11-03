﻿using UnityEngine;

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

        // Update the updated coin balance in menus
        this.MenuManager.HydrateCoins(this.User.Coins);

        Persistence.SaveUser(this.User);
    }

    // Delegate called when started visits are processed in visit schedule
    public void SaveVisits(Visit[] visits)
    {
        // Update visit counts for each guest of a started visit
        this.User.Notes.UpdateVisitCounts(visits);

        // Update notes in notes content
        this.MenuManager.UpdateNotes(this.User.Notes);

        Persistence.SaveUser(this.User);
    }

    // Delegate called when ended visits are processed in visit schedule
    public void SaveGifts(Visit[] visits)
    {
        // Create a new gift from each ended visit
        this.User.Gifts.Create(visits);

        // Update gifts in gifts content with new gifts
        this.MenuManager.UpdateGifts(this.User.Gifts);

        Persistence.SaveUser(this.User);
    }

    // Delegate called to save all rewards when gifts are claimed
    public void ClaimGifts(Gifts gifts)
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
        this.MenuManager.HydrateGifts(this.User.Gifts);

        Persistence.SaveUser(this.User);
    }

    // Delegate called when a photo is saved to a guest note
    public void SavePhoto(string guestName, Photo photo)
    {
        // Add the photo to the note in user data
        this.User.Notes.AddPhoto(guestName, photo);

        // Update note in notes content
        this.MenuManager.UpdateNote(guestName, this.User.Notes);

        Persistence.SaveUser(this.User);
        Persistence.SavePhoto(guestName, photo);
    }

    // Delegate called when a photo is deleted from photo detail panel
    public void DeletePhoto(string guestName, Photo photo)
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
        Persistence.SaveUser(this.User);
    }

}
