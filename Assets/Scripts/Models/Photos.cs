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
    public Photos(string guestName, List<byte[]> imageFiles)
    {
        this.GuestName = guestName;

        // Initialize a new photo list
        this.PhotoList = new List<Photo>();
        this.PhotoList.Capacity = imageFiles.Count;

        // Decode and and add each image to the photo list
        foreach (byte[] imageFile in imageFiles)
        {
            // Decode image file into a photo
            Photo photo = new Photo(imageFile);

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

    // Empty the photo list
    public void Clear()
    {
        this.PhotoList.Clear();
    }

}

public class Photo
{
    public Texture2D Texture;
    public string ID;
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
        this.Texture = texture;
        this.ID = this.GenerateID();
    }

    /* Create Photo from save data */
    public Photo(byte[] bytes)
    {
        // Initialize a new texture
        this.Texture = new Texture2D(2, 2);
        this.ID = this.GenerateID();

        // Replace texture contents with encoded image byte data
        ImageConversion.LoadImage(this.Texture, bytes);
    }

    // Generate filename for persisting this photo
    private string GenerateID()
    {
        // Return string value of the current datetime
        return System.DateTime.Now.ToString("ddMMyyyyHHmmss");
    }

}
