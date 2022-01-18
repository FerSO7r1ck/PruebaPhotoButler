using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationStateHandler : MonoBehaviour
{
    private bool _resumeOnFocus;

    void OnApplicationFocus(bool hasFocus)
    {
        if(hasFocus)
        {
            if (!PlaybackEngine.Instance.IsPlaying() && _resumeOnFocus)
            {
                PlaybackEngine.Instance.PlayPauseVideo();
            }
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // action on going background
            if(RecorderManager.Instance.IsRecording())
            {
                RecorderManager.Instance.AbortRecord();

                PlaybackEngine.Instance.StopVideo();

                _resumeOnFocus = false;
            }
            else
            {
                if (PlaybackEngine.Instance.IsPlaying())
                {
                    PlaybackEngine.Instance.PlayPauseVideo();

                    _resumeOnFocus = true;
                }
                else
                {
                    _resumeOnFocus = false;
                }
            }
        }
        else
        {
            // action on coming from background
            if (!PlaybackEngine.Instance.IsPlaying() && _resumeOnFocus)
            {
                PlaybackEngine.Instance.PlayPauseVideo();
            }
        }
    }
}
