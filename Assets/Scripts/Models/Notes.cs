using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

public class Notes
{
    // Keys = string of Guest name, Values = Note
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
            // Add a dictionary entry with a new note for this guest
            this.GuestNotes.Add(guest.Name, new Note(guest));
        }

    }

    /* Create Notes from save data */
    public Notes(SerializedNotes serializedNotes)
    {
        // Initialize the dictionary with known capacity of all guests
        int count = serializedNotes.GuestNotes.Length;
        this.GuestNotes = new OrderedDictionary(count);

        // Go through each serializedNote to deserialize the dictionary
        foreach (SerializedNote serializedNote in serializedNotes.GuestNotes)
        {
            // Create a new Note from the save data
            Note note = new Note(serializedNote);

            // Add a dictionary entry for the note of this guest
            this.GuestNotes.Add(serializedNote.Guest.Name, note);
        }

    }

    // Get the total number of guests
    public int Count { get { return this.GuestNotes.Count; } }

    // Custom indexing
    public Note this[string guestName]
    {
        get
        {
            return (Note)this.GuestNotes[guestName];
        }
    }

    // Assign photos after loading them in game manager
    public void HydratePhotos(Photos[] photosArray)
    {
        // Assign each Photos to the guest it belongs to
        foreach (Photos photos in photosArray)
        {
            // Get the note for this guest
            Note note = this[photos.GuestName];

            // Set the Photos of the note
            note.SetPhotos(photos);
        }

    }

    // Set photos for the note of this guest
    public void SetPhotos(string guestName, Photos photos)
    {
        // Get the note for this guest
        Note note = this[guestName];

        // Do not continue if there was an issue getting the note
        if (note == null) return;

        // Set photos of this note to new value
        note.SetPhotos(photos);
    }

    // Increment the visit count of this guest when it departs
    public void UpdateVisitCount(SlotGuest slotGuest)
    {
        // Get the note for this guest
        Note note = this[slotGuest.Guest.Name];

        // Do not continue if there was an issue getting the note
        if (note == null) return;

        // Check if this is the first encounter with this guest
        this.CheckForFirstEncounter(note, slotGuest);

        // Increase the visit count for this guest
        note.IncrementVisitCount();
    }

    // Update the friendship points of this guest
    public void UpdateFriendship(string guestName, int friendshipPoints)
    {
        // Get the note for the guest
        Note note = this[guestName];

        // Increase the friendship points for this guest
        note.AddFriendshipPoints(friendshipPoints);
    }

    // Add a photo to the note of this guest
    public void AddPhoto(string guestName, Photo photo)
    {
        // Get the note for the guest
        Note note = this[guestName];

        // Add the new photo to the photos of this guest note
        note.AddPhoto(photo);
    }

    // Get a list of names of all guests that have been seen in the active biome
    public List<string> GetSeenGuestNames()
    {
        // Initialize the list of guests
        List<string> seenGuestNames = new List<string>();

        // Get arrays of the keys and values of guest notes
        ICollection keys = this.GuestNotes.Keys;
        ICollection values = this.GuestNotes.Values;
        string[] guestNames = new string[this.GuestNotes.Count];
        Note[] notes = new Note[this.GuestNotes.Count];
        keys.CopyTo(guestNames, 0);
        values.CopyTo(notes, 0);

        for (int i = 0; i < this.GuestNotes.Count; i++)
        {
            // Check the note to see if the guest has been seen
            if (notes[i].HasBeenSeen)
            {
                // Add the seen guest to the list
                seenGuestNames.Add(guestNames[i]);
            }
        }

        return seenGuestNames;
    }

    // Record first encounter with the guest if it is seen for the first time
    private void CheckForFirstEncounter(Note note, SlotGuest slotGuest)
    {
        // Check if guest is currently in the active biome for the first time
        if (!note.HasBeenSeen && slotGuest.IsVisiting())
        {
            // Record the first encounter of this guest
            note.RecordFirstEncounter();
            note.SetImagePath(slotGuest.Guest.ImageAssetPath);
        }
    }

}

public class Note
{
    public Guest Guest { get; private set; }
    public string ImagePath { get; private set; }
    public bool HasBeenSeen { get; private set; }
    public int VisitCount { get; private set; }
    public int FriendshipPoints { get; private set; }
    public Photos Photos { get; private set; }

    /* Initialize a brand new Note */
    public Note(Guest guest)
    {
        this.Guest = guest;
        this.ImagePath = DataInitializer.UnseenGuestImageAsset;
        this.HasBeenSeen = false;
        this.VisitCount = 0;
        this.FriendshipPoints = 0;
        this.Photos = new Photos(guest.Name);
    }

    /* Create Note from save data */
    public Note(SerializedNote serializedNote)
    {
        this.Guest = serializedNote.Guest;
        this.ImagePath = serializedNote.ImagePath;
        this.HasBeenSeen = serializedNote.HasBeenSeen;
        this.VisitCount = serializedNote.VisitCount;
        this.FriendshipPoints = serializedNote.FriendshipPoints;
        this.Photos = new Photos(serializedNote.Guest.Name);
    }

    // Set the Photos for this note
    public void SetPhotos(Photos photos)
    {
        this.Photos = photos;
    }

    // Add a photo to Photos
    public void AddPhoto(Photo photo)
    {
        this.Photos.Add(photo);
    }

    // Set the image asset path used for this note
    public void SetImagePath(string imagePath)
    {
        this.ImagePath = imagePath;
    }

    // Indicate if guest has been seen in the active biome
    public void RecordFirstEncounter()
    {
        this.HasBeenSeen = true;
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
    public bool HasBeenSeen;
    public int VisitCount;
    public int FriendshipPoints;

    public SerializedNote() { }

    /* Create SerializedNote from guest and note */
    public SerializedNote(Guest guest, Note note)
    {
        this.Guest = guest;
        this.ImagePath = note.ImagePath;
        this.HasBeenSeen = note.HasBeenSeen;
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
            Note note = (Note)guestNote.Value;
            Guest guest = note.Guest;

            // Create a new serialized note
            SerializedNote serializedNote = new SerializedNote(guest, note);

            // Add the new serialized note to the list
            serializedNotes.Add(serializedNote);
        }

        // Convert serialized notes list to an array and assign it to this
        this.GuestNotes = Serializer.ListToArray(serializedNotes);
    }

}
