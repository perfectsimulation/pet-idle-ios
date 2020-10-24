using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace IOUtility
{
    /* Paths to local directories and files used for persisting game data */
    public static class Paths
    {
        /* User data json file location */
        public static string UserData
        {
            get
            {
                if (Application.isMobilePlatform)
                {
                    return Path.Combine(PersistentDataPath, "UserData.json");
                }

                // Debug purposes
                return "UserData.json";
            }
        }

        /* Photos root file directory location */
        public static string PhotosDirectory
        {
            get
            {
                return Path.Combine(PersistentDataPath, "Photos");
            }
        }

        /* Get the path to the directory containing all photos of this guest */
        public static string GuestPhotoDirectory(string guestName)
        {
            return Path.Combine(PhotosDirectory, guestName);
        }

        /* Get the path to the guest photo file */
        public static string GuestPhotoFile(string guestName, Photo photo)
        {
            return Path.Combine(GuestPhotoDirectory(guestName), photo.ID);
        }

        /* Get the path to the guest image streaming asset */
        public static string GuestImageFile(string guestName)
        {
            string fileName = string.Format("{0}.png", guestName.ToLower());
            return Path.Combine(StreamingGuestDirectory, fileName);
        }

        /* Get the path to the unknown guest image streaming asset */
        public static string GuestUnknownImageFile()
        {
            string fileName = string.Format("unknown.png");
            return Path.Combine(StreamingGuestDirectory, fileName);
        }

        /* Get the path to the item image streaming asset */
        public static string ItemImageFile(string itemName)
        {
            string fileName = string.Format("{0}.png", itemName.ToLower());
            return Path.Combine(StreamingItemDirectory, fileName);
        }

        /* Get the path to the interaction image asset */
        public static string InteractionImageFile(string guest, string item)
        {
            string fileName = string.Format(
                "{0}-{1}.png",
                guest.ToLower(),
                item.ToLower());

            return Path.Combine(StreamingInteractionDirectory, fileName);
        }

        /* Get the path to the heart level image asset */
        public static string HeartImageFile(string color)
        {
            string fileName = string.Format("heart-{0}.png", color.ToLower());
            return Path.Combine(StreamingFriendshipDirectory, fileName);
        }

        /* Get the path to the streaming asset */
        public static string StreamingAssetFile(string assetPath)
        {
            return Path.Combine(Application.streamingAssetsPath, assetPath);
        }

        /* Guest image streaming asset directory location */
        private static string StreamingGuestDirectory
        {
            get
            {
                return Path.Combine(StreamingAssetPath, "Images/Hamsters");
            }
        }

        /* Item image streaming asset directory location */
        private static string StreamingItemDirectory
        {
            get
            {
                return Path.Combine(StreamingAssetPath, "Images/Items");
            }
        }

        /* Interaction image streaming asset directory location */
        private static string StreamingInteractionDirectory
        {
            get
            {
                return Path.Combine(StreamingAssetPath, "Images/Interactions");
            }
        }

        /* Friendship image streaming asset directory location */
        private static string StreamingFriendshipDirectory
        {
            get
            {
                return Path.Combine(StreamingAssetPath, "Images/Friendship");
            }
        }

        /* Persistent data path */
        private static string PersistentDataPath
        {
            get
            {
                if (Application.isMobilePlatform)
                {
                    return Application.persistentDataPath;
                }

                // Debug purposes
                return "Persistence";
            }
        }

        /* Streaming asset path */
        private static string StreamingAssetPath
        {
            get
            {
                return Application.streamingAssetsPath;
            }
        }

    }

    /* File and directory utility functions */
    public static class PathUtility
    {
        /* Check if a local file or directory exists at this path */
        public static bool Exists(string path)
        {
            return Directory.Exists(path) || File.Exists(path);
        }

        /* Create a directory with this path */
        public static void CreateDirectory(string path)
        {
            // Create the new directory
            Directory.CreateDirectory(path);
        }

        /* Get all the file names in the directory at this path */
        public static string[] GetFileNamesInDirectory(string path)
        {
            // Initialize a string list for all the file names
            List<string> fileNames = new List<string>();

            // Return empty array if the directory does not exist
            if (!Exists(path)) return fileNames.ToArray();

            // Get all the file paths in this directory
            IEnumerable<string> filePaths = Directory.EnumerateFiles(path);

            // Get the file name and extension of each file path
            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileName(filePath);

                // Add the file name to the list of file names
                fileNames.Add(fileName);
            }

            // Return the array of file names
            return fileNames.ToArray();
        }

        /* Get all the png files in the directory at this path */
        public static List<byte[]> GetPngFilesInDirectory(string path)
        {
            // Initialize a list of encoded image files
            List<byte[]> imageFiles = new List<byte[]>();

            // Return empty list if the directory does not exist
            if (!Exists(path)) return imageFiles;

            // Get the paths of all files with a png extension in this directory
            string png = "*.png";
            IEnumerable<string> filePaths = Directory.EnumerateFiles(path, png);

            // Read each file and add its byte array to imageFiles list
            foreach (string filePath in filePaths)
            {
                // Get the image file at this path
                byte[] imageFile = Load.AsBytes(filePath);

                // Add the encoded image file to imageFiles
                imageFiles.Add(imageFile);
            }

            // Return list of encoded image files
            return imageFiles;
        }

    }

    /* Save/Write utility functions */
    public static class Save
    {
        /* Save the serializable object at the local path as a txt file */
        public static void AsTxt<T>(string path, T serializableObject)
        {
            // Generate json representation of the serializable object
            string contents = StringUtility.ToJson(serializableObject);

            // Write json to file
            File.WriteAllText(path, contents);
        }

        /* Save the byte array at the local path */
        public static void AsBytes(string path, byte[] bytes)
        {
            // Write bytes to file
            File.WriteAllBytes(path, bytes);
        }

    }

    /* Load/Read utility functions */
    public static class Load
    {
        /* Load the string contents of the text file located at this path */
        public static string AsTxt(string path)
        {
            // Return empty string if the file does not exist
            if (!PathUtility.Exists(path))
            {
                return string.Empty;
            }

            // Return string representation of the text content in this file
            return File.ReadAllText(path);
        }

        /* Load the byte array located at this path */
        public static byte[] AsBytes(string path)
        {
            // Return empty byte array if the file does not exist
            if (!PathUtility.Exists(path))
            {
                return new byte[] { };
            }

            // Return the byte array contained in this file
            return File.ReadAllBytes(path);
        }

    }

    /* Delete utility functions */
    public static class Delete
    {
        /* Delete the file at this path */
        public static void File(string path)
        {
            System.IO.File.Delete(path);
        }

    }

}
