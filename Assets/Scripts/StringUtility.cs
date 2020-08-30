using System;
using System.Collections.Generic;
using UnityEngine;

public static class StringUtility
{
    /* Get the object represented by the provided json */
    public static T FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.data[0];
    }

    /* Get an array of the objects represented by the provided json */
    public static T[] JsonToArray<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.data;
    }

    /* Get a list of the objects represented by the provided json */
    public static List<T> JsonToList<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        List<T> list = new List<T>();

        for (int i = 0; i < wrapper.data.Length; i++)
        {
            list.Add(wrapper.data[i]);
        }

        return list;
    }

    /* Get a json representing the provided object */
    public static string ToJson<T>(T item)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.data = new T[] { item };
        return JsonUtility.ToJson(wrapper, true);
    }

    /* Get a json representing the provided array of objects */
    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.data = array;
        return JsonUtility.ToJson(wrapper, true);
    }

    /* Get a json representing the provided list of objects */
    public static string ToJson<T>(List<T> list)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        T[] array = list.ToArray();
        wrapper.data = array;
        return JsonUtility.ToJson(wrapper, true);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] data;
    }

}
