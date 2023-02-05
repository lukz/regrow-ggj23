using System.Collections.Generic;

public static class ListExtensions
{
    public static T Random<T>(this List<T> list)
    {
        if (list.Count == 0) return default(T);

        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static T RandomRemove<T>(this List<T> list)
    {
        if (list.Count == 0) return default;
        int at = UnityEngine.Random.Range(0, list.Count);
        T item = list[at];
        list.RemoveAt(at);
        return item;
    }

    public static void RemoveAll<T>(this List<T> list, List<T> other)
    {
        if (list.Count == 0 || list == other) return;
        foreach(var item in other)
        {
            list.Remove(item);
        }
    }

    public static void Shuffle<T>(this List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}