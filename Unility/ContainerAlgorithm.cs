using System;
using System.Collections.Generic;
using System.Linq;

public static class ContainerAlgorithm
{
    public static TKey MinIndex<TKey, TValue, TComparable>(
        this Dictionary<TKey, TValue> source, Func<KeyValuePair<TKey, TValue>, TComparable> selector)
        where TComparable : IComparable<TComparable>
    {
        TKey minKey = default;
        TComparable minValue = default;
        foreach (KeyValuePair<TKey, TValue> pair in source)
        {
            TComparable comparable = selector(pair);
            if (comparable.CompareTo(minValue) < 0)
            {
                minKey = pair.Key;
                minValue = comparable;
            }
        }
        return minKey;
    }
    public static T MinElement<T, TComparable>(
        this IEnumerable<T> source, TComparable initValue, Func<T, TComparable> selector)
        where TComparable : IComparable<TComparable>
    {
        T minElement = default;
        TComparable minValue = initValue;
        foreach (T element in source)
        {
            TComparable comparable = selector(element);
            if (comparable.CompareTo(minValue) < 0)
            {
                minElement = element;
                minValue = comparable;
            }
        }
        return minElement;
    }
    public static T MinElement<T>(
        this IEnumerable<T> source, Func<T, float> selector)
    {
        T minElement = default;
        float minValue = float.MaxValue;
        foreach (T element in source)
        {
            float comparable = selector(element);
            if (comparable < minValue)
            {
                minElement = element;
                minValue = comparable;
            }
        }
        return minElement;
    }
}
