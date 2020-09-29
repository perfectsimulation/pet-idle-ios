using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

public class Notes
{
    // Keys = Guest, Values = Note
    public OrderedDictionary GuestNotes;

    /* Initialize a brand new Notes */
    public Notes()
    {
        // Initialize the dictionary with known capacity of all guests
        int count = DataInitializer.AllGuests.Length;
        this.GuestNotes = new OrderedDictionary(count);

        // Construct the dictionary with guests as keys and new notes as values
        foreach (Guest guest in DataInitializer.AllGuests)
        {
            this.GuestNotes.Add(guest, new Note());
        }

    }

    /* Create Notes from save data */
    public Notes(SerializedNotes serializedNotes)
    {
        // Initialize the dictionary with known capacity of all guests
        int count = serializedNotes.GuestNotes.Length;
        this.GuestNotes = new OrderedDictionary(count);

        // Go through each serializedNote to reconstruct the dictionary
        foreach (SerializedNote serializedNote in serializedNotes.GuestNotes)
        {
            // Create a new Note from the save data
            Note note = new Note(serializedNote);

            // Add a dictionary entry with the guest and note
            this.GuestNotes.Add(serializedNote.Guest, note);
        }

    }

    // Get the total number of guests
    public int Count { get { return this.GuestNotes.Count; } }

    // Custom indexing
    public Note this[Guest guest]
    {
        get
        {
            return (Note)this.GuestNotes[(object)guest];
        }
    }

    // Increment the visit count of this guest when it departs
    public void UpdateVisitCount(SlotGuest slotGuest)
    {
        // Get the note for this guest
        Note note = (Note)this.GuestNotes[(object)slotGuest.Guest];

        // Do not continue if there was an issue getting the note
        if (note == null) return;

        // If this was the first visit, change the image asset for this note
        if (note.VisitCount == 0)
        {
            note.SetImagePath("Images/Hamsters/unknown.png");
        }

        // Check if this is the first sighting for this guest
        this.CheckForFirstSighting(note, slotGuest);

        // Increase the visit count for this guest
        note.IncrementVisitCount();
    }

    // Update the friendship points of this guest
    public void UpdateFriendship(Guest guest, int friendshipPoints)
    {
        // Get the note for the guest who gave the gift
        Note note = (Note)this.GuestNotes[(object)guest];

        // Increase the friendship points for this guest
        note.AddFriendshipPoints(friendshipPoints);
    }

    // Record the sighting of the guest if it is visiting for first time
    private void CheckForFirstSighting(Note note, SlotGuest slotGuest)
    {
        // Check if guest is currently in the active biome for the first time
        if (!note.HasBeenSighted && slotGuest.IsVisiting())
        {
            // Record the first sighting of this guest
            note.RecordFirstSighting();
            note.SetImagePath(slotGuest.Guest.ImageAssetPath);
        }
    }

}

public class Note
{
    public string ImagePath { get; private set; }
    public bool HasBeenSighted { get; private set; }
    public int VisitCount { get; private set; }
    public int FriendshipPoints { get; private set; }

    /* Initialize a brand new Note */
    public Note()
    {
        this.ImagePath = "";
        this.HasBeenSighted = false;
        this.VisitCount = 0;
        this.FriendshipPoints = 0;
    }

    /* Create Note from save data */
    public Note(SerializedNote serializedNote)
    {
        this.ImagePath = serializedNote.ImagePath;
        this.HasBeenSighted = serializedNote.HasBeenSighted;
        this.VisitCount = serializedNote.VisitCount;
        this.FriendshipPoints = serializedNote.FriendshipPoints;
    }

    // Change the image asset used for this note in notes content
    public void SetImagePath(string imagePath)
    {
        this.ImagePath = imagePath;
    }

    // Indicate if guest has been seen in the active biome
    public void RecordFirstSighting()
    {
        this.HasBeenSighted = true;
    }

    // Increment visit count automatically
    public void IncrementVisitCount()
    {
        this.VisitCount++;
    }

    // Increase friendship points by the rewarded points when user claims gift
    public void AddFriendshipPoints(int friendshipPointReward)
    {
        this.FriendshipPoints += friendshipPointReward;
    }

}

[System.Serializable]
public class SerializedNote
{
    public Guest Guest;
    public string ImagePath;
    public bool HasBeenSighted;
    public int VisitCount;
    public int FriendshipPoints;

    public SerializedNote() { }

    /* Create SerializedNote from guest and note */
    public SerializedNote(Guest guest, Note note)
    {
        this.Guest = guest;
        this.ImagePath = note.ImagePath;
        this.HasBeenSighted = note.HasBeenSighted;
        this.VisitCount = note.VisitCount;
        this.FriendshipPoints = note.FriendshipPoints;
    }

}

[System.Serializable]
public class SerializedNotes
{
    public SerializedNote[] GuestNotes;

    /* Create SerializedNotes from Notes */
    public SerializedNotes(Notes notes)
    {
        // Initialize serialized note list with known capacity
        int count = notes.GuestNotes.Count;
        List<SerializedNote> serializedNotes = new List<SerializedNote>(count);

        // Go through each dictionary entry in notes to serialize the data
        foreach (DictionaryEntry guestNote in notes.GuestNotes)
        {
            // Get guest and note of this entry
            Guest guest = (Guest)guestNote.Key;
            Note note = (Note)guestNote.Value;

            // Create a new serialized note
            SerializedNote serializedNote = new SerializedNote(guest, note);

            // Add the new serialized note to the list
            serializedNotes.Add(serializedNote);
        }

        // Convert serialized notes list to an array and assign it to this
        this.GuestNotes = Serializer.ListToArray(serializedNotes);
    }

}
