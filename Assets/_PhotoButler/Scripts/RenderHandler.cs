using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Video;
//using TMPro;
using static GlobalDeclarations;

public class RenderHandler : MonoBehaviour
{
    public static RenderHandler Instance;

    private TemplateType _templateType;

    private int _currentFrame = 0;
    private bool _updatedAfterPause;
    private bool _androidOrEditorPlatform;
    private bool _iOSPlatform;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DefinePlataformAndSynchronization();
    }

    private void Update()
    {
        PrepareInternalFrameUpdate();
    }

    public void SetTemplateType(TemplateType templateType)
    {
        _templateType = templateType;
    }


    private void DefinePlataformAndSynchronization()
    {
        #if UNITY_EDITOR || UNITY_ANDROID
                _androidOrEditorPlatform = true;
        #elif UNITY_IOS
                _iOSPlatform = true;
        #endif
    }

    public void PrepareInternalFrameUpdate()
    {
        if(_androidOrEditorPlatform)
        {
            PrepareFrameForAndroidOrEditor();
        }
        else if(_iOSPlatform)
        {
            PrepareFrameForiOS();
        }
    }

    private void PrepareFrameForAndroidOrEditor()
    {
        var currentVideoFrame = ForegroundHandler.Instance.GetCurrentFrame();
        var videoTotalFrames = ForegroundHandler.Instance.GetVideoTotalFrames() - 1;

        if (PlaybackEngine.Instance.IsPlaying())
        {
            _updatedAfterPause = false;

            if (currentVideoFrame >= videoTotalFrames)
            {
                AERuntimeLoaderThreeLayer.Instance.VideoFinishedCallback();
            }
            else
            {
                switch (_templateType)
                {
                    case TemplateType.Landscape:
                        if (currentVideoFrame < 0)
                            currentVideoFrame = 0;

                        AERuntimeLoaderThreeLayer.Instance.ReadyToSetNewUGCFrame(currentVideoFrame);
                        break;
                    case TemplateType.Portrait:
                        if (currentVideoFrame < 0)
                            currentVideoFrame = 1;

                        AERuntimeLoaderThreeLayer.Instance.ReadyToSetNewUGCFrame(currentVideoFrame - 1);
                        break;
                    case TemplateType.Square:
                        break;
                }
            }
        }
        else if(ForegroundHandler.Instance.GetVideoState() && !_updatedAfterPause)
        {
            _updatedAfterPause = true;

            AERuntimeLoaderThreeLayer.Instance.ReadyToSetNewUGCFrame(currentVideoFrame);
        }
    }

    private void PrepareFrameForiOS()
    {
        var currentVideoFrame = ForegroundHandler.Instance.GetCurrentFrame();
        var videoTotalFrames = ForegroundHandler.Instance.GetVideoTotalFrames() - 1;

        if (PlaybackEngine.Instance.IsPlaying())
        {
            _updatedAfterPause = false;

            var differenceBetweenFrames = currentVideoFrame - _currentFrame;

            if (differenceBetweenFrames > 0 || differenceBetweenFrames < -1)
            {
                _currentFrame = currentVideoFrame + 1;
            }

            if (currentVideoFrame >= videoTotalFrames)
            {
                AERuntimeLoaderThreeLayer.Instance.VideoFinishedCallback();
            }
            else
            {
                AERuntimeLoaderThreeLayer.Instance.ReadyToSetNewUGCFrame(_currentFrame - 1);
            }

            _currentFrame++;
        }
        else if (ForegroundHandler.Instance.GetVideoState() && !_updatedAfterPause)
        {
            _updatedAfterPause = true;

            AERuntimeLoaderThreeLayer.Instance.ReadyToSetNewUGCFrame(currentVideoFrame);
        }
    }

    public bool IsAndroidPlataform()
    {
        return _androidOrEditorPlatform;
    }
}
