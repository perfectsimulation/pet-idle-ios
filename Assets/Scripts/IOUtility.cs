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
                    return Application.persistentDataPath + "/UserData.json";
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
                if (Application.isMobilePlatform)
                {
                    return Application.persistentDataPath + "/Photos";
                }

                // Debug purposes
                return "Persistence/Photos";
            }
        }

        /* Get the path to the directory containing all photos of this guest */
        public static string GuestPhotoDirectory(string guestName)
        {
            return Path.Combine(PhotosDirectory, guestName);
        }

        /* Get the path to the guest photo png file */
        public static string GuestPhotoFile(string guestName, Photo photo)
        {
            string fileName = photo.ID + ".png";
            return Path.Combine(GuestPhotoDirectory(guestName), fileName);
        }

        /* Get the path to the streaming asset */
        public static string StreamingAssetFile(string assetPath)
        {
            return Path.Combine(Application.streamingAssetsPath, assetPath);
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

        /* Get all the png files in the directory at this path */
        public static List<byte[]> GetPngFilesInDirectory(string path)
        {
            // Initialize a list of encoded image files
            List<byte[]> imageFiles = new List<byte[]>();

            // Return empty list if the directory does not exist
            if (!Exists(path)) return imageFiles;

            // Get the names of all files with a png extension in this directory
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

}
