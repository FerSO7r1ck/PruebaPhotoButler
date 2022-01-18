using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NativeUnityInterface : MonoBehaviour
{
    public static NativeUnityInterface Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Set/Change the current template path
    /// </summary>
    public void SetCurrentTemplate(string path)
    {
        Debug.Log("SetCurrentTemplate: " + path);

        AERuntimeLoaderThreeLayer.SetTemplate(path, false, false);
        AERuntimeLoaderThreeLayer.Instance.OnReadFile();
    }

    /// <summary>
    /// Set/Change the current template path
    /// </summary>
    public void SetCurrentTemplateDynamic(string path)
    {
        Debug.Log("SetCurrentTemplateDynamic: " + path);

        AERuntimeLoaderThreeLayer.SetTemplate(path, true, true);
        AERuntimeLoaderThreeLayer.Instance.OnReadFile();
    }

    /// <summary>
    /// Set/Change the current template path
    /// </summary>
    public void SetCurrentTemplateFullPath(string path)
    {
        Debug.Log("SetCurrentTemplateFullPath: " + path);

        AERuntimeLoaderThreeLayer.SetTemplate(path, true, false);
        AERuntimeLoaderThreeLayer.Instance.OnReadFile();
    }

    /// <summary>
    /// Check if the video is ready to play or loading
    /// </summary>
    public bool ReadyToPlay()
    {
        Debug.Log("Ready To Play invoked");
        return AERuntimeLoaderThreeLayer.Instance.ReadyToPlay();
    }

    /// <summary>
    /// Get the current frame of the animation
    /// </summary>
    public int GetCurrentFrame()
    {
        return AERuntimeLoaderThreeLayer.Instance.GetCurrentFrame();
    }

    /// <summary>
    /// Set/change the current frame of the animation
    /// </summary>
    public void SetCurrentFrame(int frame)
    {
        Debug.Log("Set Current Frame: " + frame);
        AERuntimeLoaderThreeLayer.Instance.EnableSetFrameReadyCallback();
        PlaybackEngine.Instance.GoToFrame(frame, true);
    }

    /// <summary>
    /// Set/change the current frame of the animation
    /// </summary>
    public void SetCurrentFrame(string frame)
    {
        var success = int.TryParse(frame, out int intFrame);
        if (success)
            SetCurrentFrame(intFrame);
        else
        {
            var reason = "Couldn't parse string frame as int: " + frame;
            ErrorHandler.Instance.DebugError(reason, "SetCurrentFrame", "");
        }
    }

    /// <summary>
    /// Set/change the current frame of the animation without stopping
    /// </summary>
    public void SetCurrentFrameAndPlay(int frame)
    {
        Debug.Log("Set Current Frame: " + frame);
        AERuntimeLoaderThreeLayer.Instance.EnableSetFrameReadyCallback();
        PlaybackEngine.Instance.GoToFrameWithoutStop(frame);
    }

    /// <summary>
    /// Set/change the current frame of the animation without stopping
    /// </summary>
    public void SetCurrentFrameAndPlay(string frame)
    {
        var success = int.TryParse(frame, out int intFrame);
        if (success)
            SetCurrentFrameAndPlay(intFrame);
        else
        {
            var reason = "Couldn't parse string frame as int: " + frame;
            ErrorHandler.Instance.DebugError(reason, "SetCurrentFrameAndPlay", "");
        }
    }

    /// <summary>
    /// Disable Finger sign
    /// </summary>
    public void DisableFingerSign()
    {
        AERuntimeLoaderThreeLayer.Instance.DisableFingerSignExternal();
    }

    /// <summary>
    /// This function gives us the size and position of the visible animation content
    /// </summary>
    public void GetActualRect()
    {
        Debug.Log("GetActualRect called successfully");
    }

    /// <summary>
    /// Replace the UGC based on the contentId;
    /// </summary>
    private void ReplaceUGCInternal(string imageId, string image)
    {
        AERuntimeLoaderThreeLayer.Instance.ReplaceUGCExternal(imageId, image);
    }

    /// <summary>
    /// Replace the UGC based on the contentId;
    /// </summary>
    public void ReplaceUGC(string imageContents)
    {
        var pieces = imageContents.Split(new[] { '_' }, 2);
        ReplaceUGCInternal(pieces[0], pieces[1]);
    }

    /// <summary>
    /// Replace the UGC based on the path received;
    /// </summary>
    private void ReplaceUGCFromPathInternal(string imageId, string path)
    {
        AERuntimeLoaderThreeLayer.Instance.ReplaceUGCFromPathExternal(imageId, path);
    }

    /// <summary>
    /// Replace the UGC based on the path received;
    /// </summary>
    public void ReplaceUGCFromPath(string imageContents)
    {
        var pieces = imageContents.Split(new[] { '_' }, 2);
        ReplaceUGCFromPathInternal(pieces[0], pieces[1]);
    }

    /// <summary>
    /// Replace the UGC Video based on the path received;
    /// </summary>
    private void ReplaceUGCVideoFromPathInternal(string imageId, string path)
    {
        AERuntimeLoaderThreeLayer.Instance.ReplaceUGCVideoFromPathExternal(imageId, path);
    }

    /// <summary>
    /// Replace the UGC Video based on the path received;
    /// </summary>
    public void ReplaceUGCVideoFromPath(string videoContents)
    {
        var pieces = videoContents.Split(new[] { '_' }, 2);
        ReplaceUGCVideoFromPathInternal(pieces[0], pieces[1]);
    }


    /// <summary>
    /// Mute or unmute the UGC video audio source
    /// </summary>
    public void SetUGCVideoAudioState(bool newState)
    {
        AERuntimeLoaderThreeLayer.Instance.SetAudioUGCVideoState(newState);
    }

    /// <summary>
    /// Insert and sticker based on the path received;
    /// Name will be use as a unique identifier
    /// </summary>
    public void InsertSticker(string stickerContents)
    {
        var pieces = stickerContents.Split(new[] { '_' }, 2);
        InsertStickerInternal(pieces[0], pieces[1]);
    }

    /// <summary>
    /// Insert and sticker based on the path received;
    /// Name will be use as a unique identifier
    /// </summary>
    private void InsertStickerInternal(string name, string path)
    {
        AERuntimeLoaderThreeLayer.Instance.InsertSticker(path, name);
    }

    /// <summary>
    /// Release the memory calling an empty scene
    /// </summary>
    [ContextMenu("ReleaseMemory")]
    public void ReleaseMemory()
    {
        StartCoroutine(LoadSceneInternal(0));
    }

    /// <summary>
    /// Load the main scene
    /// </summary>
    [ContextMenu("Load Video Scene")]
    public void LoadVideoScene()
    {
        StartCoroutine(LoadSceneInternal(1));
    }

    /// <summary>
    /// Load the main scene
    /// </summary>
    [ContextMenu("Load AR Scene")]
    public void LoadARScene()
    {
        StartCoroutine(LoadSceneInternal(2));
    }

    private IEnumerator LoadSceneInternal(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        yield return new WaitUntil(() => operation.isDone);

        switch (sceneIndex)
        {
            case 0:
                CallbackHandler.OnVideoSceneUnloaded();
                //Debug.Log("Total allocated memory on empty scene loaded: " + System.GC.GetTotalMemory(false) + " bytes");
                break;
            case 1:
                CallbackHandler.OnVideoSceneLoaded();
                //Debug.Log("Total allocated memory on video scene loaded: " + System.GC.GetTotalMemory(false) + " bytes");
                break;
            case 2:
                AudioListener.volume = 1;

                break;
        }
    }

    /// <summary>
    /// Set the hand state
    /// </summary>
    public void SetHandState(bool newState)
    {
        Debug.Log("Set Hand State: " + newState);
        AERuntimeLoaderThreeLayer.Instance.SetHandState(newState);
    }

    /// <summary>
    /// Set the hand state on
    /// </summary>
    public void SetHandOn()
    {
        Debug.Log("Set Hand State On");
        AERuntimeLoaderThreeLayer.Instance.SetHandState(true);
    }

    /// <summary>
    /// Set the hand state off
    /// </summary>
    public void SetHandOff()
    {
        Debug.Log("Set Hand State Off");
        AERuntimeLoaderThreeLayer.Instance.SetHandState(false);
    }

    /// Set the hand state on
    /// </summary>
    public void SetCoverOn()
    {
        Debug.Log("Set Cover State On");
        AERuntimeLoaderThreeLayer.Instance.SetCoverState(true);
    }

    /// <summary>
    /// Set the hand state off
    /// </summary>
    public void SetCoverOff()
    {
        Debug.Log("Set Cover State Off");
        AERuntimeLoaderThreeLayer.Instance.SetCoverState(false);
    }


    /// <summary>
    /// Starts/resumes playback
    /// </summary>
    public void Play()
    {
        Debug.Log("Play/Resume");
        PlaybackEngine.Instance.PlayPauseVideo(GlobalDeclarations.CallbackSource.External);
    }

    /// <summary>
    /// Pauses playback
    /// </summary>
    public void Pause()
    {
        Debug.Log("Paused");
        PlaybackEngine.Instance.PlayPauseVideo(GlobalDeclarations.CallbackSource.External);
    }

    /// <summary>
    /// Enable the playback callbacks
    /// </summary>
    public void EnablePlaybackCallback()
    {
        AERuntimeLoaderThreeLayer.Instance.EnablePlaybackCallback();
    }

    /// <summary>
    /// Disable the playback callbacks
    /// </summary>
    public void DisablePlaybackCallback()
    {
        AERuntimeLoaderThreeLayer.Instance.DisablePlaybackCallback();
    }
    


    /// <summary>
    /// Pauses playback
    /// </summary>
    /// Parameters are: (progress:Closure, success:Closure, failure:Closure)
    public void SaveMp4(bool asyncRecording = true)
    {
        RecorderManager.Instance.SetRecordAudioState(true);
        RecorderManager.Instance.OnRecord();
    }

    /// <summary>
    /// Saves async
    /// </summary>
    /// Parameters are: (progress:Closure, success:Closure, failure:Closure)
    public void SaveMp4Async(string path)
    {
        RecorderManager.Instance.SetRecordAudioState(true);
        RecorderManager.Instance.SetAsyncState(true);
        RecorderManager.Instance.OnRecord(path);
    }

    public void SaveMp4Sync(string path)
    {
        RecorderManager.Instance.SetRecordAudioState(true);
        RecorderManager.Instance.SetAsyncState(false);
        RecorderManager.Instance.OnRecord(path);
    }

    public void SaveMp4WithoutAudio(string path)
    {
        RecorderManager.Instance.SetRecordAudioState(false);
        RecorderManager.Instance.OnRecord(path);
    }

    /// <summary>
    /// This method resume the saving process
    /// </summary>
    public void ResumeSaveMP4()
    {
        PlaybackEngine.Instance.PlayPauseVideo();
        RecorderManager.Instance.ResumeRecord();
    }

    /// <summary>
    /// This method pause the saving process and remove from memory any objects used to save the MP4
    /// </summary>
    public void PauseSaveMP4()
    {
        PlaybackEngine.Instance.PlayPauseVideo();
        RecorderManager.Instance.PauseRecord();
    }

    /// <summary>
    /// This method stops all generation and completely deletes any partial progress that has been made
    /// </summary>
    public void StopSaveMp4()
    {
        RecorderManager.Instance.AbortRecord();
    }

    /// <summary>
    /// This method stops all generation and completely deletes any partial progress that has been made
    /// </summary>
    public void GetMaskForFrame(string frame)
    {
        var success = int.TryParse(frame, out int intFrame);
        if (success)
            AERuntimeLoaderThreeLayer.Instance.GetFrame(intFrame);
        else
        {
            var reason = "Couldn't parse string frame as int: " + frame;
            ErrorHandler.Instance.DebugError(reason, "GetMaskForFrame", "");
        }
    }

    /// <summary>
    /// Set the playback volume (0.0 to 1.0) only in the preview, the export will have the full volume
    /// </summary>
    public void SetVolume(string volume)
    {
        var success = float.TryParse(volume, out float floatVolume);

        if(success)
        {
            #if UNITY_ANDROID || UNITY_EDITOR
                        PlaybackEngine.Instance.SetAudioVolume(floatVolume);
            #endif

            #if UNITY_IOS && !UNITY_EDITOR
                        AERuntimeLoaderThreeLayer.Instance.SetGeneralVolume(floatVolume);
            #endif
        }
        else
        {
            var reason = "Couldn't parse string volume as float: " + volume;
            ErrorHandler.Instance.DebugError(reason, "SetVolume", "");
        }
    }

    /// <summary>
    /// Set the scale
    /// </summary>
    public void MoveUGC(string instructions)
    {
        var pieces = instructions.Split(new[] { '_' }, 3);
        var success = int.TryParse(pieces[1], out int x);

        if (!success)
        {
            var reason = "Couldn't parse string X as int: " + pieces[1];
            ErrorHandler.Instance.DebugError(reason, "ScaleUGC", "");
            return;
        }

        success = int.TryParse(pieces[2], out int y);
        if (!success)
        {
            var reason = "Couldn't parse string X as int: " + pieces[2];
            ErrorHandler.Instance.DebugError(reason, "ScaleUGC", "");
            return;
        }

        AERuntimeLoaderThreeLayer.Instance.MoveUGCExternal(pieces[0], x, y);
    }

    /// <summary>
    /// Set the scale
    /// </summary>
    public void ScaleUGC(string instructions)
    {
        var pieces = instructions.Split(new[] { '_' }, 3);
        var success = float.TryParse(pieces[1], out float x);

        if (!success)
        {
            var reason = "Couldn't parse string Y as float: " + pieces[1];
            ErrorHandler.Instance.DebugError(reason, "ScaleUGC", "");
            return;
        }

        success = float.TryParse(pieces[2], out float y);
        if (!success)
        {
            var reason = "Couldn't parse string Y as float: " + pieces[2];
            ErrorHandler.Instance.DebugError(reason, "ScaleUGC", "");
            return;
        }

        AERuntimeLoaderThreeLayer.Instance.ScaleUGCExternal(pieces[0], x, y);
    }

    /// <summary>
    /// Reset the scale and movement
    /// </summary>
    public void ResetUGC(string ugcId)
    {
        AERuntimeLoaderThreeLayer.Instance.ResetUGCExternal(ugcId);
    }


    /// <summary>
    /// Set the Hand's Outline Color
    /// </summary>
    public void SetHandOutlineColor(string colorStr)
    {
        var success = ColorUtility.TryParseHtmlString(colorStr, out Color newCol);
        if (success)
        {
            AERuntimeLoaderThreeLayer.Instance.SetHandOutlineColorInternal(newCol);
            Debug.Log("Outline Color Changed");
        }
        else
        {
            ErrorHandler.Instance.DebugError("Outline Color Failed to Parse", "SetHandOutlineColor", "");
        }
    }

    /// <summary>
    /// Set the Hand's Fill Color
    /// </summary>
    public void SetHandFillColor(string colorStr)
    {
        var success = ColorUtility.TryParseHtmlString(colorStr, out Color newCol);
        if (success)
        {
            AERuntimeLoaderThreeLayer.Instance.SetHandFillColorInternal(newCol);
            Debug.Log("Fill Color Changed");
        }
        else
        {
            ErrorHandler.Instance.DebugError("Fill Color Failed to Parse", "SetHandFillColor", "");
        }
    }

    /// <summary>
    /// Set the Background color on portrait
    /// </summary>
    public void SetBackgroundColor(string colorStr)
    {
        var success = ColorUtility.TryParseHtmlString(colorStr, out Color newCol);
        if (success)
        {
            AERuntimeLoaderThreeLayer.Instance.SetBackgroundColor(newCol);
            Debug.Log("Background Color Changed");
        }
        else
        {
            ErrorHandler.Instance.DebugError("Background Color Failed to Parse", "SetBackgroundColor", "");
        }
    }

    /// <summary>
    /// Reset the video state
    /// </summary>
    [ContextMenu("ResetVideo")]
    public void ResetVideo()
    {
        AERuntimeLoaderThreeLayer.Instance.ResetVideoAfterShare();
    }

    [ContextMenu("ShowConsole")]
    public void ShowConsole()
    {
        //SRDebug.Instance.ShowDebugPanel();
    }

    public void SetBackgroundMode()
    {
        if (RecorderManager.Instance.IsRecording())
        {
            RecorderManager.Instance.AbortRecord();

            PlaybackEngine.Instance.StopVideo();
        }
        else if(PlaybackEngine.Instance.IsPlaying())
        {
            PlaybackEngine.Instance.PlayPauseVideo();
        }
    }

    /// <summary>
    /// Set the record name
    /// </summary>
    [ContextMenu("Set Record Name")]
    public void SetCustomRecordName(string text)
    {
        AERuntimeLoaderThreeLayer.Instance.SetRecordName(text);
    }

    /// <summary>
    /// Load the soundtrack
    /// Path: location of the file ,Name: name of the file
    /// </summary>
    public void LoadSoundTrack(string path)
    {
        AERuntimeLoaderThreeLayer.Instance.LoadSoundtrack(path);
    }

    /// <summary>
    /// Mute or unmute the custom SoundTrack
    /// </summary>
    public void SetMuteStateOfSoundTrack(bool newState)
    {
        AERuntimeLoaderThreeLayer.Instance.SetMuteStateOfSoundTrack(newState);
    }

    /// <summary>
    /// Activate or deactivate addressabless
    /// </summary>
    public void SetAddressablesStatus(bool status)
    {
        AERuntimeLoaderThreeLayer.Instance.SetAddressablesState(status);
    }
}
