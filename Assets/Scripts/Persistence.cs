using System.IO;
using UnityEngine;

public static class Persistence
{
    //   _   _
    //  | | | |
    //  | | | |___  ___ _ __
    //  | | | / __|/ _ \ '__|
    //  | |_| \__ \  __/ |
    //   \___/|___/\___|_|

    /* User data file location */
    private static string UserDataPath
    {
        get
        {
            if (Application.isMobilePlatform)
            {
                return Application.persistentDataPath + "/UserData.json";
            }

            // Debug purposes
            return "UserData.json";
        }
    }

    /* Serialize the user and save data locally */
    public static void SaveUser(User user)
    {
        // Serialize the user
        string userDataJson = StringUtility.ToJson(new SerializedUser(user));

        // Write to local file
        File.WriteAllText(UserDataPath, userDataJson);
        Debug.Log("Saved user");
    }

    /* Get deserialized user from local save data or create a new user */
    public static User LoadUser()
    {
        // Initialize a new user when no local save data exists
        if (!DoesFileExistAtPath(UserDataPath))
        {
            Debug.Log("No local user data. Created new user");
            return CreateUser();
        }

        Debug.Log("Loaded user");

        // Read contents of existing local user data json
        string userDataJson = File.ReadAllText(UserDataPath);

        // Get a serialized user from the user data json
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

    /* Photos root file directory */
    private static string PhotosRootPath
    {
        get
        {
            if (Application.isMobilePlatform)
            {
                return Application.persistentDataPath + "/Photos";
            }

            // Debug purposes
            return "Persistence/Photos";
        }
    }

    /* Serialize the photo and save data locally */
    public static void SavePhoto(Guest guest, Photo photo)
    {
        // Get the pathname for the local photo file
        string photoPath = GetGuestPhotoFilePath(guest, photo);

        // Do not continue if the photo has already been saved
        if (DoesFileExistAtPath(photoPath)) return;

        // Encode the photo as a PNG byte array
        SerializedPhoto serializedPhoto = new SerializedPhoto(photo);

        // Write to local file
        File.WriteAllBytes(photoPath, serializedPhoto.Bytes);
    }

    /* Create a photo directory for each guest */
    private static void InitializePhotoDirectories()
    {
        // Create a directory for each guest in which its photos are saved
        foreach (Guest guest in DataInitializer.AllGuests)
        {
            // Get the path to use for the new directory
            string path = GetGuestPhotoDirectory(guest);

            // Get the directory info for this path
            DirectoryInfo directory = new DirectoryInfo(path);

            // Create the new directory
            directory.Create();
        }

    }

    //   _   _ _   _ _ _ _   _
    //  | | | | | (_) (_) | (_)
    //  | | | | |_ _| |_| |_ _  ___  ___
    //  | | | | __| | | | __| |/ _ \/ __|
    //  | |_| | |_| | | | |_| |  __/\__ \
    //   \___/ \__|_|_|_|\__|_|\___||___/

    /* Check if a local file exists at the provided path */
    public static bool DoesFileExistAtPath(string path)
    {
        return File.Exists(path);
    }

    /* Get the full path to the streaming asset */
    public static string GetAbsoluteAssetPath(string assetPath)
    {
        return Path.Combine(Application.streamingAssetsPath, assetPath);
    }

    /* Get the path to the directory containing all photos of this guest */
    private static string GetGuestPhotoDirectory(Guest guest)
    {
        return Path.Combine(PhotosRootPath, guest.Name);
    }

    /* Get the full path to the guest photo */
    private static string GetGuestPhotoFilePath(Guest guest, Photo photo)
    {
        string fileName = photo.ID + ".png";
        return Path.Combine(PhotosRootPath, guest.Name, fileName);
    }

}
