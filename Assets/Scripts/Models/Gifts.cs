using System.Collections.Generic;

public class Gifts
{
    public List<Gift> GiftList;

    /* Initialize a brand new Gifts */
    public Gifts()
    {
        this.GiftList = new List<Gift>();
    }

    /* Create Gifts from save data */
    public Gifts(SerializedGifts serializedGifts)
    {
        List<Gift> giftList = new List<Gift>();
        giftList.Capacity = serializedGifts.Length;

        // Create a gift for each serialized gift
        foreach (SerializedGift serializedGift in serializedGifts.GiftArray)
        {
            // Add the new gift to the gift list
            giftList.Add(new Gift(serializedGift));
        }

        this.GiftList = giftList;
    }

    // Get the total number of gifts
    public int Count { get { return this.GiftList.Count; } }

    // Custom indexing
    public Gift this[int index]
    {
        get
        {
            return this.ToArray()[index];
        }
    }

    // Get an array of all gifts
    public Gift[] ToArray()
    {
        return this.GiftList.ToArray();
    }

    // Create and add gifts from each visit
    public void Create(Visit[] visits)
    {
        // Cache a reference to reuse when generating each gift
        Gift gift;

        // Add new gift from each visit to this gift list
        foreach (Visit visit in visits)
        {
            // Generate the new gift from the guest and item of this visit
            gift = new Gift(visit.Guest, visit.Item);

            // Add the new gift to this gift list
            this.GiftList.Add(gift);
        }

    }

    // Add gifts to this gift list
    public void Add(Gift[] giftArray)
    {
        // Add each gift in the array
        foreach (Gift gift in giftArray)
        {
            this.GiftList.Add(gift);
        }

    }

    // Get all gifts of updated gift list that are not included in this list
    public List<Gift> GetLatestGifts(Gifts updatedGifts)
    {
        // Initilize list for gifts that have not yet been added to this list
        List<Gift> latestGifts = new List<Gift>();

        // Get the difference in gift count with the updated gift list
        int additionalGiftCount = updatedGifts.Count - this.Count;

        // Do not continue if there were fewer or equal gifts in updated list
        if (additionalGiftCount <= 0)
        {
            // Return the empty gift list
            return latestGifts;
        }

        // Add gifts from the updated list starting at index of first new gift
        for (int i = this.Count; i < updatedGifts.Count; i++)
        {
            // Add the latest gift to the list
            latestGifts.Add(updatedGifts[i]);
        }

        return latestGifts;
    }

    // Empty the gift list
    public void Clear()
    {
        this.GiftList.Clear();
    }

    // Return the total coin amount from all gifts
    public int GetTotalCoins()
    {
        int coins = 0;

        // Add coins from each gift
        foreach (Gift gift in this.GiftList)
        {
            coins += gift.Coins;
        }

        return coins;
    }

}

public class Gift
{
    public Guest Guest;
    public Item Item;
    public int Coins;
    public int FriendshipReward;

    /* Initialize a brand new Gift on guest departure */
    public Gift(Guest guest, Item item)
    {
        this.Guest = guest;
        this.Item = item;
        this.Coins = this.SelectGuestCoinDrop();
        this.FriendshipReward = this.SelectGuestFriendshipReward();
    }

    /* Create Gift from save data */
    public Gift(SerializedGift serializedGift)
    {
        this.Guest = new Guest(serializedGift.GuestName);
        this.Item = new Item(serializedGift.ItemName);
        this.Coins = serializedGift.Coins;
        this.FriendshipReward = serializedGift.FriendshipReward;
    }

    // Select a coin drop for a new gift
    private int SelectGuestCoinDrop()
    {
        // Randomly select a coin drop within the range allowed by the guest
        int min = this.Guest.MinimumCoinDrop;
        int max = this.Guest.MaximumCoinDrop;

        // Add one to the max since Random.Range has an exclusive max argument
        int coinDrop = UnityEngine.Random.Range(min, max + 1);
        return coinDrop;
    }

    // Select a friendship reward for a new gift
    private int SelectGuestFriendshipReward()
    {
        // Randomly select a coin drop within the range allowed by the guest
        int min = this.Guest.MinimumFriendshipReward;
        int max = this.Guest.MaximumFriendshipReward;

        // Add one to the max since Random.Range has an exclusive max argument
        int friendshipPoints = UnityEngine.Random.Range(min, max + 1);
        return friendshipPoints;
    }

}

[System.Serializable]
public class SerializedGift
{
    public string GuestName;
    public string ItemName;
    public int Coins;
    public int FriendshipReward;

    /* Serialize a gift */
    public SerializedGift(Gift gift)
    {
        this.GuestName = gift.Guest.Name;
        this.ItemName = gift.Item.Name;
        this.Coins = gift.Coins;
        this.FriendshipReward = gift.FriendshipReward;
    }

}

[System.Serializable]
public class SerializedGifts
{
    public SerializedGift[] GiftArray;

    /* Create SerializedGifts from Gifts */
    public SerializedGifts(Gifts gifts)
    {
        SerializedGift[] giftArray = new SerializedGift[gifts.Count];

        // Create a serialized gift for each gift
        for (int i = 0; i < gifts.Count; i++)
        {
            // Add the new serialized gift to the gift array
            giftArray[i] = new SerializedGift(gifts[i]);
        }

        this.GiftArray = giftArray;
    }

    // Get the total number of serialized gifts
    public int Length { get { return this.GiftArray.Length; } }

}
