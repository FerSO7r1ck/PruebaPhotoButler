using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using static GlobalDeclarations;

public class CallbackHandler : MonoBehaviour
{

#if UNITY_IOS && !UNITY_EDITOR

    [DllImport("__Internal")]
	private static extern void _onUnityInit();

    public static void OnUnityInit()
    {
        _onUnityInit();
    }

    [DllImport("__Internal")]
    private static extern void _onSaving(int progress);

    public static void OnSaving(int progress)
    {
        _onSaving(progress);
    }

    [DllImport("__Internal")]
    private static extern void _onPlaying(int progress);

    public static void OnPlaying(int progress)
    {
        _onPlaying(progress);
    }

    [DllImport("__Internal")]
    private static extern void _onUnityReady(int totalFrames);

    public static void OnUnityReady(int totalFrames)
    {
        _onUnityReady(totalFrames);
    }

    [DllImport("__Internal")]
    private static extern void _onStartVideo();

    public static void OnStartVideo()
    {
        _onStartVideo();
    }

    [DllImport("__Internal")]
    private static extern void _onPauseVideo();

    public static void OnPauseVideo()
    {
        _onPauseVideo();
    }

    [DllImport("__Internal")]
    private static extern void _onSetFrameReady();

    public static void OnSetFrameReady()
    {
        _onSetFrameReady();
    }

    [DllImport("__Internal")]
    private static extern void _onPlayCompleted();

    public static void OnPlayCompleted()
    {
        _onPlayCompleted();
    }

    [DllImport("__Internal")]
    private static extern void _onSavedCompleted(string path);

    public static void OnSavedCompleted(string path)
    {
        _onSavedCompleted(path);
    }

    [DllImport("__Internal")]
    private static extern void _onFrameReady(string imageContents);

    public static void OnFrameReady(string imageContents)
    {
        _onFrameReady(imageContents);
    }

    [DllImport("__Internal")]
    private static extern void _onImageReplaced();

    public static void OnImageReplaced()
    {
        _onImageReplaced();
    }

    [DllImport("__Internal")]
    private static extern void _onVideoReplaced();

    public static void OnVideoReplaced()
    {
        _onVideoReplaced();
    }

    [DllImport("__Internal")]
    private static extern void _onSendMessage(string message);

    public static void OnSendMessage(string message)
    {
        _onSendMessage(message);
    }

    [DllImport("__Internal")]
    private static extern void _onVideoSceneLoaded();

    public static void OnVideoSceneLoaded()
    {
        _onVideoSceneLoaded();
    }

    [DllImport("__Internal")]
    private static extern void _onVideoSceneUnloaded();

    public static void OnVideoSceneUnloaded()
    {
        _onVideoSceneUnloaded();
    }

     [DllImport("__Internal")]
    private static extern void _onUnityLoaded();

    public static void OnUnityLoaded()
    {
        _onUnityLoaded();
    }

#endif

#if UNITY_ANDROID && !UNITY_EDITOR

    public static void OnUnityInit()
    {
        if (EnableTestMode) return;
        AndroidJavaObject jo = GetJavaObject();

        if (jo != null) jo.Call("onUnityInit");
    }
        
    public static void OnSaving(int progress)
    {
        if (EnableTestMode) return;
        AndroidJavaObject jo = GetJavaObject();

        if (jo != null) jo.Call("onSaving", progress);
    }

    public static void OnPlaying(int progress)
    {
        if (EnableTestMode) return;
        AndroidJavaObject jo = GetJavaObject();

        if (jo != null) jo.Call("onPlaying", progress);
    }

    public static void OnUnityReady(int totalFrames)
    {
        if (EnableTestMode) return;
        AndroidJavaObject jo = GetJavaObject();

        if (jo != null) jo.Call("onUnityReady", totalFrames);
    }

    public static void OnStartVideo()
    {
        if (EnableTestMode) return;
        AndroidJavaObject jo = GetJavaObject();

        if (jo != null) jo.Call("onStartVideo");
    }

    public static void OnPauseVideo()
    {
        if (EnableTestMode) return;
        AndroidJavaObject jo = GetJavaObject();

        if (jo != null) jo.Call("onPauseVideo");
    }

    public static void OnSetFrameReady()
    {
        if (EnableTestMode) return;
        AndroidJavaObject jo = GetJavaObject();

        if (jo != null) jo.Call("onSetFrameReady");
    }

    public static void OnPlayCompleted()
    {
        if (EnableTestMode) return;
        AndroidJavaObject jo = GetJavaObject();

        if (jo != null) jo.Call("onPlayCompleted");
    }

    public static void OnSavedCompleted(string path)
    {
        if (EnableTestMode) return;
        AndroidJavaObject jo = GetJavaObject();

        if (jo != null) jo.Call("onSaveCompleted", path);
    }

    public static void OnFrameReady(string imageContents)
    {
        if (EnableTestMode) return;
        AndroidJavaObject jo = GetJavaObject();

        if (jo != null) jo.Call("onFrameReady", imageContents);
    }

    public static void OnImageReplaced()
    {
        if (EnableTestMode) return;
        AndroidJavaObject jo = GetJavaObject();

        if (jo != null) jo.Call("onImageReplaced");
    }

    public static void OnVideoReplaced()
    {
        if (EnableTestMode) return;
        AndroidJavaObject jo = GetJavaObject();

        if (jo != null) jo.Call("onVideoReplaced");
    }

    public static void OnSendMessage(string message)
    {
        if (EnableTestMode) return;
        AndroidJavaObject jo = GetJavaObject();

        if (jo != null) jo.Call("onSendMessage", message);
    }

    public static void OnVideoSceneLoaded()
    {
        if (EnableTestMode) return;
        AndroidJavaObject jo = GetJavaObject();

        if (jo != null) jo.Call("onVideoSceneLoaded");
    }

    public static void OnVideoSceneUnloaded()
    {
        if (EnableTestMode) return;
        AndroidJavaObject jo = GetJavaObject();

        if (jo != null) jo.Call("onVideoSceneUnloaded");
    }

    public static void OnUnityLoaded()
    {
        if (EnableTestMode) return;
        AndroidJavaObject jo = GetJavaObject();

        if (jo != null) jo.Call("onUnityLoaded");
    }

    private static AndroidJavaObject GetJavaObject() 
    {
        try
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.photobutler.vs.VideoStoriesManager");

            if (jc != null)
                return jc.GetStatic<AndroidJavaObject>("instance");
        }
        catch(Exception ex)
        {
            ErrorHandler.Instance.DebugError("Fail to find android class VideoStoriesManager", "GetJavaObject - CallbackHandler", ex.Message);
        }

        return null;
    }

#endif

#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBGL

    public static void OnUnityInit()
    {
        //Debug.Log("Callback not supported in this platform");
    }

    public static void OnSaving(int progress)
    {
        //Debug.Log("Callback not supported in this platform");
    }

    public static void OnPlaying(int progress)
    {
        //Debug.Log("Callback not supported in this platform");
    }

    public static void OnUnityReady(int totalFrames)
    {
        //Debug.Log("Callback not supported in this platform");
    }

    public static void OnStartVideo()
    {
        //Debug.Log("Callback not supported in this platform");
    }

    public static void OnPauseVideo()
    {
        //Debug.Log("Callback not supported in this platform");
    }

    public static void OnSetFrameReady()
    {
        //Debug.Log("Callback not supported in this platform");
    }

    public static void OnPlayCompleted()
    {
        //Debug.Log("Callback not supported in this platform");
    }

    public static void OnSavedCompleted(string path)
    {
        //Debug.Log("Callback not supported in this platform");
    }

    public static void OnFrameReady(string imageContents)
    {
        //Debug.Log("Callback not supported in this platform");
    }

    public static void OnImageReplaced()
    {
        //Debug.Log("Callback not supported in this platform");
    }

    public static void OnVideoReplaced()
    {
        //Debug.Log("Callback not supported in this platform");
    }

    public static void OnSendMessage(string message)
    {
        //Debug.Log("Callback not supported in this platform");
    }

    public static void OnVideoSceneLoaded()
    {
        //Debug.Log("Callback not supported in this platform");
    }

    public static void OnVideoSceneUnloaded()
    {
        //Debug.Log("Callback not supported in this platform");
    }

    public static void OnUnityLoaded()
    {
        //Debug.Log("Callback not supported in this platform");
    }
#endif


}

