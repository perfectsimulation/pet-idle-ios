using System.IO;
using UnityEngine;

public static class Persistence
{
    private static string UserDataPath
    {
        get
        {
            if (Application.isMobilePlatform)
            {
                return Application.persistentDataPath + "/UserData.json";
            }

            return "UserData.json";
        }
    }

    /* Serialize the user and save data to local directory */
    public static void SaveUser(User user)
    {
        // Serialize the user
        string userDataJson = StringUtility.ToJson(new SerializedUser(user));

        // Write to local file
        File.WriteAllText(UserDataPath, userDataJson);
        Debug.Log("Saved user");
    }

    /* Get the user by deserializing local save data */
    public static User LoadUser()
    {
        // If there is local save data, load and deserialize the existing user
        if (DoesFileExistAtPath(UserDataPath))
        {
            // Read from local file
            string userDataJson = File.ReadAllText(UserDataPath);
            Debug.Log("Loaded user");

            // Get a serialized user from the contents of the local save file
            SerializedUser userData = StringUtility.FromJson<SerializedUser>(userDataJson);

            // Get a user from the serialized user
            User user = new User(userData);
            return user;
        }

        // Create a new user if no local save data is found
        Debug.Log("No local user data. Created new user");
        User newUser = new User();

        // Create local save file with new user
        SaveUser(newUser);
        return newUser;
    }

    /* Check if a local file exists at the provided path */
    private static bool DoesFileExistAtPath(string path)
    {
        return File.Exists(path);
    }

}
