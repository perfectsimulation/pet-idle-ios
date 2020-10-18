using System.Collections.Generic;
using UnityEngine;

public class Photos
{
    public string GuestName;
    public List<Photo> PhotoList;

    /* Initialize a brand new Photos */
    public Photos(string guestName)
    {
        this.GuestName = guestName;
        this.PhotoList = new List<Photo>();
    }

    /* Create Photos from save data */
    public Photos(string guestName, List<byte[]> images, string[] fileNames)
    {
        this.GuestName = guestName;

        // Initialize a new photo list
        this.PhotoList = new List<Photo>();
        this.PhotoList.Capacity = images.Count;

        // Decode and add each image to the photo list
        for (int i = 0; i < images.Count; i++)
        {
            // Decode image file into a photo
            Photo photo = new Photo(images[i], fileNames[i]);

            // Add photo to photo list
            this.PhotoList.Add(photo);
        }

    }

    // Get the total number of photos
    public int Count { get { return this.PhotoList.Count; } }

    // Custom indexing
    public Photo this[int index]
    {
        get
        {
            return this.ToArray()[index];
        }
    }

    // Get an array of all photos
    public Photo[] ToArray()
    {
        return this.PhotoList.ToArray();
    }

    // Add the photo to the photo list
    public void Add(Photo photo)
    {
        this.PhotoList.Add(photo);
    }

    // Remove the photo from the photo list
    public void Remove(Photo photo)
    {
        this.PhotoList.Remove(photo);
    }

    // Empty the photo list
    public void Clear()
    {
        this.PhotoList.Clear();
    }

}

public class Photo
{
    public string ID;
    public Texture2D Texture;
    public byte[] Bytes
    {
        get
        {
            if (this.Texture == null)
            {
                return new byte[] { };
            }

            return this.Texture.EncodeToPNG();
        }
    }

    /* Create Photo from texture */
    public Photo(Texture2D texture)
    {
        this.ID = this.GenerateFileName();
        this.Texture = texture;
    }

    /* Create Photo from save data */
    public Photo(byte[] bytes, string fileName)
    {
        // Use the file name for the ID
        this.ID = fileName;

        // Initialize a new texture
        this.Texture = new Texture2D(2, 2);

        // Replace texture contents with encoded image byte data
        ImageConversion.LoadImage(this.Texture, bytes);
    }

    // Generate file name for persisting this photo
    private string GenerateFileName()
    {
        // Append png extension to current datetime string
        string id = string.Format("{0}.png",
            System.DateTime.Now.ToString("ddMMyyyyHHmmss"));

        return id;
    }

}
