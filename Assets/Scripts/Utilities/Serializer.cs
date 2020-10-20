using System.Collections.Generic;

public static class Serializer
{
    /* Get an array with the contents of the provided list */
    public static T[] ListToArray<T>(List<T> list)
    {
        return list.ToArray();
    }

    /* Get a list with the contents of the provided array */
    public static List<T> ArrayToList<T>(T[] array)
    {
        List<T> list = new List<T>();

        foreach (T item in array)
        {
            list.Add(item);
        }

        return list;
    }
}
