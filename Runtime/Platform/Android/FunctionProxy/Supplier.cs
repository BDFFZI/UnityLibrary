using System;
using UnityEngine;

public class Supplier<T> : AndroidJavaProxy
{
    public Supplier(Func<T> supplier) : base("java.util.function.Supplier")
    {
        this.supplier = supplier;
    }

    readonly Func<T> supplier;

    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedMember.Local
    T get()
    {
        return supplier();
    }
}
