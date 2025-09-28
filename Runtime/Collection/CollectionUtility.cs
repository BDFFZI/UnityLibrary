using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;

public static class CollectionUtility
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
    public static int MinIndex<TDataType>(this NativeArray<TDataType> array)
        where TDataType : struct, IComparable<TDataType>
    {
        return MaxIndex(array, -1);
    }

    public static int MaxIndex<TDataType>(this NativeArray<TDataType> array)
        where TDataType : struct, IComparable<TDataType>
    {
        return MaxIndex(array, 1);
    }

    static int MaxIndex<TDataType>(NativeArray<TDataType> array, int coefficient)
        where TDataType : struct, IComparable<TDataType>
    {
        int index = 0;
        TDataType minValue = array[0];

        for (int i = 1; i < array.Length; i++)
        {
            TDataType value = array[i];
            if (value.CompareTo(minValue) * coefficient > 0)
            {
                index = i;
                minValue = value;
            }
        }

        return index;
    }
}
