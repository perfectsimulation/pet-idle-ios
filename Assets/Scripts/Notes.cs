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

    // Update the guest notes with the visit details of the guest object
    public void UpdateNote(GuestObject guestObject)
    {
        // TODO create gift for the guest visit to show updated values visually
        // Get the note for this guest
        Note note = (Note)this.GuestNotes[(object)guestObject.Guest];

        // Do not continue if there was an issue getting the note
        if (note == null) return;

        // Increase the friendship points and visit count for this guest
        note.AddFriendshipPointReward(guestObject.FriendshipPointReward);
        note.IncrementVisitCount();

        // Reassign the note to the guest notes
        this.GuestNotes[guestObject.Guest] = note;
    }

}

public class Note
{
    public int FriendshipPoints { get;  private set; }
    public int VisitCount { get; private set; }

    /* Initialize a brand new Note */
    public Note()
    {
        this.FriendshipPoints = 0;
        this.VisitCount = 0;
    }

    /* Create Note from save data */
    public Note(SerializedNote serializedNote)
    {
        this.FriendshipPoints = serializedNote.FriendshipPoints;
        this.VisitCount = serializedNote.VisitCount;
    }

    // Increase friendship points by the rewarded points when guest departs
    public void AddFriendshipPointReward(int friendshipPointReward)
    {
        this.FriendshipPoints += friendshipPointReward;
    }

    // Increment visit count when guest departs
    public void IncrementVisitCount()
    {
        this.VisitCount++;
    }

}

[System.Serializable]
public class SerializedNote
{
    public Guest Guest;
    public int FriendshipPoints;
    public int VisitCount;

    public SerializedNote() { }

    /* Create SerializedNote from guest and note */
    public SerializedNote(Guest guest, Note note)
    {
        this.Guest = guest;
        this.FriendshipPoints = note.FriendshipPoints;
        this.VisitCount = note.VisitCount;
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
