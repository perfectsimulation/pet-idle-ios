using IOUtility;
using System.Collections.Generic;

public static class Persistence
{
    //   _   _
    //  | | | |
    //  | | | |___  ___ _ __
    //  | | | / __|/ _ \ '__|
    //  | |_| \__ \  __/ |
    //   \___/|___/\___|_|

    /* Serialize the user and save as a json file */
    public static void SaveUser(User user)
    {
        // Serialize the user
        SerializedUser serializedUser = new SerializedUser(user);

        // Save serialized user to local file
        Save.AsTxt(Paths.UserData, serializedUser);

        // TODO remove debug log
        UnityEngine.Debug.Log("Saved user");
    }

    /* Get deserialized user from local save data or create a new user */
    public static User LoadUser()
    {
        // Initialize a new user when no local save data exists
        if (!PathUtility.Exists(Paths.UserData))
        {
            UnityEngine.Debug.Log("No local user data. Created new user");
            return CreateUser();
        }

        // Load raw user data
        string userDataJson = Load.AsTxt(Paths.UserData);

        // TODO remove debug log
        UnityEngine.Debug.Log("Loaded user");

        // Construct the serialized user from the raw user data
        SerializedUser userData =
            StringUtility.FromJson<SerializedUser>(userDataJson);

        // Get a user from the serialized user
        User user = new User(userData);
        return user;
    }

    /* Create a new user when no local save data is found */
    private static User CreateUser()
    {
        // Create local save directories
        InitializePhotoDirectories();

        // Create a new user
        User newUser = new User();

        // Create new local save file for user data
        SaveUser(newUser);
        return newUser;
    }

    //  ______ _           _
    //  | ___ \ |         | |
    //  | |_/ / |__   ___ | |_ ___  ___
    //  |  __/| '_ \ / _ \| __/ _ \/ __|
    //  | |   | | | | (_) | || (_) \__ \
    //  \_|   |_| |_|\___/ \__\___/|___/

    /* Serialize the photo and save it locally to a png file */
    public static void SavePhoto(Guest guest, Photo photo)
    {
        // Get the path to the local photo file
        string photoPath = Paths.GuestPhotoFile(guest.Name, photo);

        // Do not continue if the photo has already been saved
        if (PathUtility.Exists(photoPath)) return;

        // Save photo to local file
        Save.AsBytes(photoPath, photo.Bytes);
    }

    /* Load and decode all saved photos */
    public static Photos[] LoadPhotos()
    {
        // Initialize a list of Photos
        List<Photos> photosList = new List<Photos>();
        photosList.Capacity = DataInitializer.AllGuests.Length;

        // Create a Photos element for each guest
        foreach (Guest guest in DataInitializer.AllGuests)
        {
            // Get the path to the photo directory of this guest
            string directory = Paths.GuestPhotoDirectory(guest.Name);

            // Get all the encoded image files in this photo directory
            List<byte[]> images = PathUtility.GetPngFilesInDirectory(directory);

            // Create a Photos with all the image files
            Photos photos = new Photos(guest.Name, images);

            // Add the Photos of this guest to the list of all Photos
            photosList.Add(photos);
        }

        // Return all the Photos for each guest as an array
        return photosList.ToArray();
    }

    /* Create a photo directory for each guest */
    private static void InitializePhotoDirectories()
    {
        // Create a directory for each guest where its photos are saved
        foreach (Guest guest in DataInitializer.AllGuests)
        {
            // Get the path to use for the new directory
            string photoDirectoryPath = Paths.GuestPhotoDirectory(guest.Name);

            // Create the new directory at this path
            PathUtility.CreateDirectory(photoDirectoryPath);
        }

    }

}
