using UnityEngine;
using UnityEngine.UI;
using static GlobalDeclarations;

public class PlaybackEngine : MonoBehaviour
{
    public static PlaybackEngine Instance;

    public bool PlayAudioFromSource;
    public AudioSource AudioSource;

    public GameObject PlaybackControls;
    public Image PlayPauseButton;
    public Sprite PauseIcon;
    public Sprite PlayIcon;

    private TemplateType _templateType;
    private bool _playing;
    private bool _paused;

    private float _lastVolumeSetted = 1f;

    public void Awake()
    {
        Instance = this;

        PlayPauseButton.sprite = PlayIcon;
        PlaybackControls.SetActive(false);
        _playing = false;
        _paused = false;
    }

    public void ActivatePlaybackControls() { PlaybackControls.SetActive(true); }
    public void DeactivatePlaybackControls() {  PlaybackControls.SetActive(false);}

    public bool IsPlaying() 
    {
        return _playing;
    }

    public void PlayAudio() 
    {
        if (!PlayAudioFromSource) return;

        if (!_paused)
        {
            AudioSource.Play();
        }
        else
        {
            UnPause();
        }
    }

    public void PauseAudio() 
    {
        if (!PlayAudioFromSource) return;

        if (AudioSource.isPlaying && !_paused)
        {
            AudioSource.Pause();

            _paused = true;
        }
    }

    private void UnPause()
    {
        if (!PlayAudioFromSource) return;

        AudioSource.UnPause();

        _paused = false;
    }

    public void StopAudio() 
    {
        if (!PlayAudioFromSource) return;

        if (!AudioSource.isPlaying && _paused)
        {
            UnPause();
        }

        AudioSource.Stop();

        SetAudioTime(0);

        _paused = false;
    }

    public void SetTemplateType(TemplateType templateType)
    {
        _templateType = templateType;
    }

    public void SetAudioTime(float time)
    {
        if (!PlayAudioFromSource) return;

        AudioSource.time = time;
    }

    public void SetAudioVolume(float volume)
    {
        //If its iOS we need to apply the new volume to the mixer audio, but in android is okey if it use audiosource.volume

#if (UNITY_IOS || UNITY_WEBGL) && !UNITY_EDITOR

                //_lastVolumeSetted = ForegroundHandler.Instance.GetVideoVolume();

                ForegroundHandler.Instance.SetVideoVolume(volume);

                _lastVolumeSetted = volume;

#else

             if (PlayAudioFromSource)
            {
                //_lastVolumeSetted = AudioSource.volume;

                var maxVolume = 0.5f;
                var newVolume = volume * maxVolume;

                AudioSource.volume = newVolume;

                _lastVolumeSetted = volume;
            }
            else
            {
                // = ForegroundHandler.Instance.GetVideoVolume();

                ForegroundHandler.Instance.SetVideoVolume(volume);

                _lastVolumeSetted = volume;
            }

        #endif
    }

    public void SetLastVolume()
    {
        SetAudioVolume(_lastVolumeSetted);
    }

    public float GetLastVolumeSetted()
    {
        return _lastVolumeSetted;
    }

    public void PlayPauseBtnCall()
    {
        PlayPauseVideo(CallbackSource.External);
    }

    public void PlayPauseVideo(CallbackSource callbackSource = CallbackSource.Internal) 
    {   
        if (EnableTestMode)
            NativeUnityInterface.Instance.SetCoverOff();

        if (!_playing)
        {
            _playing = true;

            AERuntimeLoaderThreeLayer.Instance.SetAudioState(true);
            AERuntimeLoaderThreeLayer.Instance.SetStickersStatus(true);

            if(AERuntimeLoaderThreeLayer.Instance.IsUsingCustomSoundtrack())
                AERuntimeLoaderThreeLayer.Instance.PlaySoundtrack();

            //if(AERuntimeLoaderThreeLayer.Instance.GetCurrentFrame() == AERuntimeLoaderThreeLayer.Instance.GetTotalFrames())
            //{
            //    StopVideo();
            //    PlayPauseVideo();

            //    return;
            //}
            if (AERuntimeLoaderThreeLayer.Instance.GetCurrentFrame() > 0)
            {
                if(AERuntimeLoaderThreeLayer.Instance.GetCurrentFrame() >= AERuntimeLoaderThreeLayer.Instance.GetTotalFrames())
                {
                    StopVideo();
                }

                AERuntimeLoaderThreeLayer.Instance.OnResumeAnimation(callbackSource);
            }
            else 
            { 
                AERuntimeLoaderThreeLayer.Instance.OnPlayAnimation(callbackSource); 
            }
            PlayPauseButton.sprite = PauseIcon;
            PlayAudio();
        }
        else 
        {
            AERuntimeLoaderThreeLayer.Instance.SetAudioState(false);

            if (AERuntimeLoaderThreeLayer.Instance.IsUsingCustomSoundtrack())
                AERuntimeLoaderThreeLayer.Instance.PauseSoundtrack();

            _playing = false;
            PlayPauseButton.sprite = PlayIcon;
            AERuntimeLoaderThreeLayer.Instance.OnPauseAnimation(callbackSource);
            PauseAudio();
        }

        if (RenderHandler.Instance.IsAndroidPlataform() && !_playing)
        {
            var currentVideoFrame = ForegroundHandler.Instance.GetCurrentFrame();
            var currentFrame = AERuntimeLoaderThreeLayer.Instance.GetCurrentFrame();
            var frameBalance = currentVideoFrame - currentFrame;

            GoToFrame(currentVideoFrame + frameBalance - 1);
        }

        /*  if (RenderHandler.Instance.IsAndroidPlataform() && !_playing)
          {
              var currentFrame = AERuntimeLoaderThreeLayer.Instance.GetCurrentFrame();
              var currentVideoFrame = ForegroundHandler.Instance.GetCurrentFrame();

              var frameBalance = currentVideoFrame - currentFrame;

              switch(_templateType)
              {
                  case TemplateType.Landscape:
                      #region Landscape

                      if (currentFrame < currentVideoFrame)
                          GoToNextFrame();
                      else if (currentFrame > currentVideoFrame && currentFrame > 0)
                          GoToPreviousFrame();

                      #endregion
                      break;
                  case TemplateType.Portrait:
                      #region Portrait

                      if (currentFrame < currentVideoFrame || (currentFrame > currentVideoFrame && currentFrame > 0))
                          GoToFrame(currentVideoFrame + frameBalance);

                      #endregion
                      break;
                  case TemplateType.Square:
                      #region Square

                      #endregion
                      break;
              }
          }*/

    }

    public void StopVideo()
    {
        _playing = false;
        PlayPauseButton.sprite = PlayIcon;
        AERuntimeLoaderThreeLayer.Instance.OnStopAnimation();
        AERuntimeLoaderThreeLayer.Instance.SetStickersStatus(false);

        if (AERuntimeLoaderThreeLayer.Instance.IsUsingCustomSoundtrack())
            AERuntimeLoaderThreeLayer.Instance.StopSoundtrack();

        RecorderManager.Instance.StopReadyToStartRecording();

        //ForegroundHandler.Instance.CleanBuffer();
        //StartCoroutine(ForegroundHandler.Instance.InitBuffer(0));
        StopAudio();
    }

    public void GoToFrame(int frame, bool isTransition = false, bool isPreload = false)
    {
        _playing = false;
        PlayPauseButton.sprite = PlayIcon;
        AERuntimeLoaderThreeLayer.Instance.SkipToFrame(frame, isTransition, isPreload);
    }

    public void GoToFrameWithoutStop(int frame)
    {
        _playing = true;
        PlayPauseButton.sprite = PauseIcon;

        AERuntimeLoaderThreeLayer.Instance.SkipToFrameWithoutStop(frame);
    }

    public void GoToNextFrame()
    {
        _playing = false;
        PlayPauseButton.sprite = PlayIcon;
        AERuntimeLoaderThreeLayer.Instance.SkipToFrame(AERuntimeLoaderThreeLayer.Instance.GetCurrentFrame() + 1, false);
    }

    public void GoToPreviousFrame()
    {
        _playing = false;
        PlayPauseButton.sprite = PlayIcon;
        AERuntimeLoaderThreeLayer.Instance.SkipToFrame(AERuntimeLoaderThreeLayer.Instance.GetCurrentFrame() - 1, false);
    }
}
