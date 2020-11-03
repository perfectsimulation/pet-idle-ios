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
            this.GuestNotes.Add(serializedNote.GuestName, note);
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

        set
        {
            this.GuestNotes[guestName] = value;
        }

    }

    // Get a note array of all notes from values of ordered dictionary
    public Note[] ToArray()
    {
        // Get values of ordered dictionary
        ICollection values = this.GuestNotes.Values;

        // Initialize a note array to fill with all note values
        Note[] notes = new Note[this.Count];

        // Copy all note values from ordered dictionary into the note array
        values.CopyTo(notes, 0);

        // Return array of all notes
        return notes;
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
    public void UpdateVisitCounts(Visit[] visits)
    {
        // Cache a reference to reuse when accessing the note for each visit
        Note note;

        // Update notes for each guest of these visits
        foreach (Visit visit in visits)
        {
            // Get the note for the guest of this visit
            note = this[visit.Guest.Name];

            // Skip this visit if there was an issue getting the note
            if (note == null) continue;

            // Check if this is the first encounter with this guest
            this.CheckForFirstEncounter(note, visit);

            // Increase the visit count for this guest
            note.IncrementVisitCount();
        }

    }

    // Update the friendships of these guests with gift rewards
    public void UpdateFriendships(Gifts gifts)
    {
        // Process each friendship reward of gifts
        foreach (Gift gift in gifts.GiftList)
        {
            // Get the note for the guest of this gift
            Note note = this[gift.Guest.Name];

            // Skip if there was an issue getting the note
            if (note == null) continue;

            // Claim the friendship reward and add the points to the note
            note.AddFriendshipPoints(gift.FriendshipReward);
        }

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

        // Get array with each note in ordered dictionary
        Note[] notes = this.ToArray();

        // Go through each note to check if its guest has been seen
        for (int i = 0; i < notes.Length; i++)
        {
            if (notes[i].HasBeenSeen)
            {
                // Add the seen guest to the list
                seenGuestNames.Add(notes[i].Guest.Name);
            }
        }

        return seenGuestNames;
    }

    // Record first encounter with the guest if it is seen for the first time
    private void CheckForFirstEncounter(Note note, Visit visit)
    {
        // Check if guest is currently in the active biome for the first time
        if (!note.HasBeenSeen && visit.IsActive())
        {
            // Record the first encounter of this guest
            note.RecordFirstEncounter();
        }
    }

}

public class Note
{
    public Guest Guest { get; private set; }
    public bool HasBeenSeen { get; private set; }
    public int VisitCount { get; private set; }
    public int FriendshipPoints { get; private set; }
    public Photos Photos { get; private set; }

    /* Initialize a brand new Note */
    public Note(Guest guest)
    {
        this.Guest = guest;
        this.HasBeenSeen = false;
        this.VisitCount = 0;
        this.FriendshipPoints = 0;
        this.Photos = new Photos(guest.Name);
    }

    /* Create Note from save data */
    public Note(SerializedNote serializedNote)
    {
        this.Guest = new Guest(serializedNote.GuestName);
        this.HasBeenSeen = serializedNote.HasBeenSeen;
        this.VisitCount = serializedNote.VisitCount;
        this.FriendshipPoints = serializedNote.FriendshipPoints;
        this.Photos = new Photos(serializedNote.GuestName);
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
    public string GuestName;
    public bool HasBeenSeen;
    public int VisitCount;
    public int FriendshipPoints;

    public SerializedNote() { }

    /* Create SerializedNote from guest and note */
    public SerializedNote(Guest guest, Note note)
    {
        this.GuestName = guest.Name;
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
