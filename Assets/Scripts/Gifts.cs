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

        foreach (SerializedGift serializedGift in serializedGifts.GiftArray)
        {
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

}

public class Gift
{
    public Guest Guest;
    public Item Item;
    public int FriendshipPointReward;
    public int CoinDrop;

    /* Create a new Gift */
    public Gift(Guest guest, Item item, int friendshipReward, int coinDrop)
    {
        this.Guest = guest;
        this.Item = item;
        this.FriendshipPointReward = friendshipReward;
        this.CoinDrop = coinDrop;
    }

    /* Create Gift from save data */
    public Gift(SerializedGift serializedGift)
    {
        this.Guest = serializedGift.Guest;
        this.Item = new Item(serializedGift.Item);
        this.FriendshipPointReward = serializedGift.FriendshipPointReward;
        this.CoinDrop = serializedGift.CoinDrop;
    }

}

[System.Serializable]
public class SerializedGift
{
    public Guest Guest;
    public SerializedItem Item;
    public int FriendshipPointReward;
    public int CoinDrop;

    /* Create SerializedGift from Gift */
    public SerializedGift(Gift gift)
    {
        this.Guest = gift.Guest;
        this.Item = new SerializedItem(gift.Item);
        this.FriendshipPointReward = gift.FriendshipPointReward;
        this.CoinDrop = gift.CoinDrop;
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

        for (int i = 0; i < gifts.Count; i++)
        {
            giftArray[i] = new SerializedGift(gifts[i]);
        }

        this.GiftArray = giftArray;
    }

    // Get the total number of serialized gifts
    public int Length { get { return this.GiftArray.Length; } }

}
