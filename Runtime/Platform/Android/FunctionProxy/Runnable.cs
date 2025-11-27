using System;
using UnityEngine;

public class Runnable : AndroidJavaProxy
{
    public Runnable(Action runnable) : base("java.lang.Runnable")
    {
        this.runnable = runnable;
    }

    readonly Action runnable;

    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedMember.Local
    void run()
    {
        runnable();
    }
}
