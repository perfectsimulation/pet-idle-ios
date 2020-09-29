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

    // Add the gift to the gift list
    public void Add(Gift gift)
    {
        this.GiftList.Add(gift);
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
    public int FriendshipPoints;
    public int Coins;

    /* Create a Gift with explicit properties */
    public Gift(Guest guest, Item item, int friendshipReward, int coins)
    {
        this.Guest = guest;
        this.Item = item;
        this.FriendshipPoints = friendshipReward;
        this.Coins = coins;
    }

    /* Create Gift from guest departure */
    public Gift(SlotGuest slotGuest, Item item)
    {
        this.Guest = slotGuest.Guest;
        this.Item = item;
        this.FriendshipPoints = slotGuest.FriendshipPointReward;
        this.Coins = slotGuest.CoinDrop;
    }

    /* Create Gift from save data */
    public Gift(SerializedGift serializedGift)
    {
        this.Guest = serializedGift.Guest;
        this.Item = new Item(serializedGift.Item);
        this.FriendshipPoints = serializedGift.FriendshipPoints;
        this.Coins = serializedGift.Coins;
    }

}

[System.Serializable]
public class SerializedGift
{
    public Guest Guest;
    public SerializedItem Item;
    public int FriendshipPoints;
    public int Coins;

    /* Create SerializedGift from Gift */
    public SerializedGift(Gift gift)
    {
        this.Guest = gift.Guest;
        this.Item = new SerializedItem(gift.Item);
        this.FriendshipPoints = gift.FriendshipPoints;
        this.Coins = gift.Coins;
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
