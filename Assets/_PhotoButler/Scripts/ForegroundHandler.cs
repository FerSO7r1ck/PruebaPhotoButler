using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Video;

public class ForegroundHandler : MonoBehaviour
{
    public static ForegroundHandler Instance;


    private Texture2D _texture2dGetFrame;

    [Header("VideoPlayer Configuration")]
    public VideoPlayer VideoPlayer;
    public RawImage VideoRender_Output;

    public AudioMixer AudioMixer;

    private int _currentFrame;

    private bool _isVideoLoaded;


    private void Awake()
    {
        Instance = this;
    }

    #region General Configuration

    public VideoPlayer GetVideoPlayer()
    {
        return VideoPlayer;
    }


    public void SetVideoPlayer(VideoPlayer videoPlayer, RawImage videoImage)
    {
        VideoPlayer = videoPlayer;

        VideoRender_Output = videoImage;

        VideoPlayer.errorReceived += VideoPlayerErrorReceived;
    }

    public IEnumerator PrepareVideo()
    {
        VideoPlayer.Prepare();

        while (!VideoPlayer.isPrepared)
        {
            yield return null;
        }
    }


    private void VideoPlayerErrorReceived(VideoPlayer source, string message)
    {
        ErrorHandler.Instance.DebugError(message, "VidePlayerErrorReceived", "");
    }
    public void Play()
    {
        try
        {
            if (VideoPlayer != null)
            {
                VideoPlayer.Play();
            }
        }
        catch (Exception ex)
        {
            ErrorHandler.Instance.DebugError(ex.Message, "Error in ForegroundHandler Play", ex.Message);
        }

    }

    public void Pause()
    {
        try
        {
            if (VideoPlayer != null)
            {
                VideoPlayer.Pause();
            }
        }
        catch (Exception ex)
        {
            ErrorHandler.Instance.DebugError(ex.Message, "Error in ForegroundHandler Pause", ex.Message);
        }
    }

    public void Stop()
    {
        try
        {
            if (VideoPlayer != null)
            {
                VideoPlayer.Stop();
                VideoPlayer.Prepare();
            }
        }
        catch (Exception ex)
        {
            ErrorHandler.Instance.DebugError(ex.Message, "Error in ForegroundHandler Stop", ex.Message);
        }
    }

    public void SetVideoVolume(float volume)
    {
#if (UNITY_IOS || UNITY_WEBGL) && !UNITY_EDITOR
                string OutputMixer = "Recording";
                    var recordingGroup = AudioMixer.FindMatchingGroups(OutputMixer)[0];
            
                    //The number 55 is just to ajust the volumen scale acording to the mixer values
                    var newVolume = (1f - volume) * -55;
            
                    AudioMixer.SetFloat("Volume", newVolume);
#elif UNITY_ANDROID || UNITY_EDITOR
        VideoPlayer.SetDirectAudioVolume(0, volume);
#endif
    }

    public float GetVideoVolume()
    {
        var volume = 1f;

#if (UNITY_IOS || UNITY_WEBGL) && !UNITY_EDITOR
                AudioMixer.GetFloat("Volume", out volume);
#elif UNITY_ANDROID || UNITY_EDITOR
        volume = VideoPlayer.GetDirectAudioVolume(0);
#endif

        return volume;
    }

    public void SetFrameVideoPlayer(int _videoFrameStart)
    {
        VideoPlayer.frame = _videoFrameStart;

        if (VideoRender_Output.texture == null)
            VideoRender_Output.texture = VideoPlayer.texture;
    }

    public void GetFrame(int intFrame)
    {
        CancelInvoke("AbortGetFrame");
        Invoke("AbortGetFrame", 5);

        VideoPlayer.frameReady += ReadyToGetFrame;
        VideoPlayer.frame = intFrame;
    }

    private void AbortGetFrame()
    {
#if !UNITY_EDITOR
                CallbackHandler.OnFrameReady("error");
#endif
    }

    private void ReadyToGetFrame(VideoPlayer vp, long frameIndex)
    {
        if (_texture2dGetFrame != null)
        {
            Destroy(_texture2dGetFrame);
        }

        _texture2dGetFrame = TextureToTexture2D(VideoPlayer.texture);

        byte[] imageData = _texture2dGetFrame.EncodeToPNG();
        var imageContents = Convert.ToBase64String(imageData);

        CancelInvoke("AbortGetFrame");

#if !UNITY_EDITOR
        CallbackHandler.OnFrameReady(imageContents);
#endif
        VideoPlayer.frameReady -= ReadyToGetFrame;
    }

    private Texture2D TextureToTexture2D(Texture texture)
    {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);
        return texture2D;
    }

    public int GetCurrentFrame()
    {
        return (int)VideoPlayer.frame;
    }

    public bool IsVideoPlaying()
    {
        return VideoPlayer.isPlaying;
    }

    public int GetVideoTotalFrames()
    {
        return (int)VideoPlayer.frameCount;
    }

    public void ShowRenderOutput()
    {
        VideoRender_Output.gameObject.SetActive(true);
    }

    public void HideRenderOutput()
    {
        VideoRender_Output.gameObject.SetActive(false);
    }

    public void SetVideoAsLoaded()
    {
        _isVideoLoaded = true;
    }

    public bool GetVideoState()
    {
        return _isVideoLoaded;
    }
    
    public void SetVideoRenderMaterial(Material material)
    {
        VideoRender_Output.material = material;
    }

    #endregion

}
