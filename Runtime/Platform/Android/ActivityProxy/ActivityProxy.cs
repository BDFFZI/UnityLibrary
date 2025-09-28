using System;
using UnityEngine;

#if UNITY_ANDROID && !UNITY_EDITOR
[DefaultExecutionOrder(-10)]
public static class ActivityProxy
{
    public static AndroidJavaObject CurrentActivity => currentActivity;
    public static Func<bool> VolumeDownHandler
    {
        set => currentActivity.Set("VolumeDownHandler", new Supplier<bool>(value));
    }
    public static Func<bool> VolumeUpHandler
    {
        set => currentActivity.Set("VolumeUpHandler", new Supplier<bool>(value));
    }

    static AndroidJavaClass unityPlayer;
    static AndroidJavaObject currentActivity;


    [RuntimeInitializeOnLoadMethod]
    public static void Initialize()
    {
        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    }
}
#endif
