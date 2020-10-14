using System.Collections.Generic;
using UnityEngine;

public class Photos
{
    public List<Photo> PhotoList;

    /* Initialize a brand new Photos */
    public Photos()
    {
        this.PhotoList = new List<Photo>();
    }

    /* Create Photos from save data */
    public Photos(SerializedPhotos serializedPhotos)
    {
        List<Photo> photoList = new List<Photo>();
        photoList.Capacity = serializedPhotos.Length;

        // Create a gift for each serialized gift
        foreach (SerializedPhoto serializedPhoto in serializedPhotos.PhotoArray)
        {
            // Add the new gift to the gift list
            photoList.Add(new Photo(serializedPhoto));
        }

        this.PhotoList = photoList;
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

    /* Create Photo from texture */
    public Photo(Texture2D texture)
    {
        this.Texture = texture;
    }

    /* Create Photo from save data */
    public Photo(SerializedPhoto serializedPhoto)
    {
        // Initialize a new texture
        this.Texture = new Texture2D(2, 2);

        // Replace texture contents with serialized image byte data
        ImageConversion.LoadImage(this.Texture, serializedPhoto.Bytes);
    }

}

[System.Serializable]
public class SerializedPhoto
{
    public byte[] Bytes;

    /* Create SerializedPhoto from Photo */
    public SerializedPhoto(Photo photo)
    {
        // Encode the photo texture into a PNG byte array
        this.Bytes = photo.Texture.EncodeToPNG();
    }

}

[System.Serializable]
public class SerializedPhotos
{
    public SerializedPhoto[] PhotoArray;

    /* Create SerializedPhotos from Photos */
    public SerializedPhotos(Photos photos)
    {
        SerializedPhoto[] photoArray = new SerializedPhoto[photos.Count];

        // Create a serialized photo for each photo
        for (int i = 0; i < photos.Count; i++)
        {
            // Add the new serialized photo to the photo array
            photoArray[i] = new SerializedPhoto(photos[i]);
        }

        this.PhotoArray = photoArray;
    }

    // Get the total number of serialized photos
    public int Length { get { return this.PhotoArray.Length; } }

}
