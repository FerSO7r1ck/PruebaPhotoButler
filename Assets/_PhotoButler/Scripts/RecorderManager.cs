using UnityEngine;
using UnityEngine.Rendering;
using System.IO;
using UnityEngine.Video;
using System;
using UnityEngine.UI;
using System.Collections;
#if (UNITY_IOS || UNITY_WEBGL) && !UNITY_EDITOR
using pmjo.NextGenRecorder;
#else
using NatSuite.Recorders;
using NatSuite.Recorders.Inputs;
using NatSuite.Recorders.Clocks;
#endif

using static GlobalDeclarations;

public class RecorderManager : MonoBehaviour
{
    public static RecorderManager Instance;

    public Camera MainCamera;

    public RecorderCamera RecorderCameraLandscape;
    public RecorderCamera RecorderCameraPortrait;
    public RecorderCamera RecorderCameraSquare;

    public RecorderCamera IOsRecorderCameraLandscape;
    public RecorderCamera IOsRecorderCameraPortrait;
    public RecorderCamera IOsRecorderCameraSquare;

    public Texture WatermarkTexture;

    public VideoPlayer PBStinger;
    public AudioSource AudioSource;

#if UNITY_ANDROID || UNITY_EDITOR || UNITY_STANDALONE
    CameraInput cameraInput;
    AudioInput audioInput;
#endif

    #region Record

    public bool RecordAudio = true;
    public bool RecordFromCamera = true;

    private RecorderCamera _currentCamera;
    private TemplateType _templateType;

#if UNITY_ANDROID || UNITY_EDITOR || UNITY_STANDALONE
    private MP4Recorder _recorder;
    private RealtimeClock _clock;
#endif

    private bool _asyncRecording = true;
    private bool _isRecording;
    private bool _isAborting;
    private bool _readyToStartRecording;
    private AudioListener _audioListener;
    private string _pathRequested;
    #endregion

    

    private void Awake()
    {
        Instance = this;
        _isRecording = false;

        _currentCamera = RecorderCameraLandscape;



#if (UNITY_IOS || UNITY_WEBGL) && !UNITY_EDITOR

            //Recorder.VerticalFlip = false;
            //Recorder.WatermarkTexture = WatermarkTexture;

            //var ratio = 720f / 1080;
            //Recorder.VideoScale = 1 - ratio;
            // Downscale huge resolutions 50%
            if (Mathf.Max(Screen.width, Screen.height) >= 1920)
            {
                Recorder.VideoScale = 0.5f;
            }
            else
            {
                Recorder.VideoScale = 1.0f;
            }


        _currentCamera = IOsRecorderCameraLandscape;
#else
        _audioListener = _currentCamera.TheAudio;
        _clock = new RealtimeClock();
#endif
    }

    private void Start()
    {
        if (PBStinger.gameObject.activeSelf) PBStinger.Prepare();
    }

    private void Update()
    {
        //This method is used only by NatCorder
#if UNITY_ANDROID || UNITY_EDITOR
        Record();
#endif
    }

    private void SetCurrentCameraBasedOnOrientation()
    {
        _currentCamera.gameObject.SetActive(true);
    }

    public void SetCurrentTemplateType(TemplateType templateType, Camera mainCamera, int width, int height)
    {
        _templateType = templateType;
        MainCamera = mainCamera;


        #if UNITY_ANDROID || UNITY_EDITOR
        
        switch (_templateType)
        {
            case TemplateType.Landscape:
                _currentCamera = RecorderCameraLandscape;
                break;
            case TemplateType.Portrait:
                _currentCamera = RecorderCameraPortrait;
                break;
            case TemplateType.Square:
                _currentCamera = RecorderCameraSquare;
                break;
        }

#endif

#if UNITY_IOS && !UNITY_EDITOR

        switch (_templateType)
        {
            case TemplateType.Landscape:
                _currentCamera = IOsRecorderCameraLandscape;
                break;
            case TemplateType.Portrait:
                _currentCamera = IOsRecorderCameraPortrait;
                break;
            case TemplateType.Square:
                _currentCamera = IOsRecorderCameraSquare;
                break;
        }

        var customRecorderiOS = _currentCamera.GetComponent<RecoderCustomiOS>();

        customRecorderiOS.SetTextureDimensions(width, height);

#endif
    }


    public void SetRecordAudioState(bool state)
    {
        RecordAudio = state;
    }

    public void OnRecord(string finalPath = "")
    {
        if (_isRecording)
        {
            ResetRecorder();
        }

        AERuntimeLoaderThreeLayer.Instance.SetCoverState(true);

        AERuntimeLoaderThreeLayer.Instance.CloseTransitionEffect();

        //Set the volume to max
        NativeUnityInterface.Instance.SetVolume("1");

        AERuntimeLoaderThreeLayer.Instance.SetAudioState(true);

        _readyToStartRecording = true;

        StartCoroutine(ReadyToStartRecording(finalPath));
    }

    public void StopReadyToStartRecording()
    {
        _readyToStartRecording = false;
    }

    IEnumerator ReadyToStartRecording(string finalPath)
    {
        yield return Frames(20);

        if (_readyToStartRecording)
        {
            PlaybackEngine.Instance.GoToFrame(0, false);
            StartRecording(finalPath);
            PlaybackEngine.Instance.PlayPauseVideo();
        }
    }

    public static IEnumerator Frames(int frameCount)
    {
        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }
    }

    public void SetAsyncState(bool asyncRecording)
    {
        _asyncRecording = asyncRecording;
    }


    #region Gets

    public bool IsRecording()
    {
        return _isRecording;
    }

    public bool IsRecordAudioEnable()
    {
        return RecordAudio;
    }

    #endregion



#if (UNITY_IOS || UNITY_WEBGL) && !UNITY_EDITOR

    #region Next gen for Ios 

        void OnEnable()
        {
            Recorder.RecordingExported += RecordingExported;
        }

        void OnDisable()
        {
            Recorder.RecordingExported -= RecordingExported;
        }

        public void StartRecording(string finalPath = "")
        {
            try
            {
                SetCurrentCameraBasedOnOrientation();
        
                Recorder.RecordAudio = RecordAudio;
                Recorder.StartRecording();
                //CaptureFromCamera.OutputFolderPath = finalPath;
                _pathRequested = finalPath;

                if(finalPath == String.Empty)
                {
                    _pathRequested = Application.persistentDataPath;
                }

                _isRecording = true;
                _isAborting = false;

            }
            catch (Exception ex)
            {
                ErrorHandler.Instance.DebugError("Start recording failed", "StartRecording", ex.Message);
            }
        }

        private void ResetRecorder()
        {
            _isRecording = false;

            Recorder.StopRecording();
        }

        public void ResumeRecord()
        {
            _isRecording = true;

            Recorder.ResumeRecording();
        }

        public void PauseRecord()
        {
            _isRecording = false;

            Recorder.PauseRecording();
        }

        public void FinishRecord()
        {
            if (!_isRecording) return;

            if (AERuntimeLoaderThreeLayer.Instance.IsPBSingerEnable())
            {
                PBStinger.frame = 0;
                PBStinger.gameObject.SetActive(true);
                var videoLength = PBStinger.length;
                float vOut = Convert.ToSingle(videoLength);
                Invoke("StopRecord", vOut);
            }
            else
            {
                PBStinger.gameObject.SetActive(false);
                Invoke("StopRecord", 0f);
            }
        }

        public void StopRecord()
        {
            StopOrAbortInternal(false);
        }

        public void AbortRecord()
        {
            StopOrAbortInternal(true);
        }

        private void StopOrAbortInternal(bool isAbort)
        {
            if (!_isRecording) return;
            _isRecording = false;
            _isAborting = isAbort;

            PBStinger.gameObject.SetActive(false);

            Recorder.StopRecording();

            Invoke("StopRecordInternal", isAbort ? 0f : 1f);

            PlaybackEngine.Instance.StopAudio();
        }

        private void StopRecordInternal()
        {
            try
            {
                long lastSession = Recorder.GetLastRecordingSession();

                Recorder.ExportRecordingSession(lastSession);               

            }
            catch (Exception ex)
            {
                ErrorHandler.Instance.DebugError("File save/export failed", "StopRecordInternal", ex.Message);
                //Debug.Log("Exception saving mp4 file: " + ex.Message);
            }
        }

        void RecordingExported(long sessionId, string path, Recorder.ErrorCode errorCode)
        {
            if (errorCode == Recorder.ErrorCode.NoError)
            {
                AERuntimeLoaderThreeLayer.Instance.SetAudioState(false);

                //Debug.Log("Recording exported to " + path + ", session id " + sessionId);

                var destinationPath = string.Empty;
                string fileName = Path.GetFileName(path);

                try
                {
                    destinationPath = _pathRequested + fileName;
                    File.Move(path, destinationPath);
                }
                catch(Exception ex)
                {
                    destinationPath = path;

                    ErrorHandler.Instance.DebugError("File move failed", "RecordingExported", ex.Message);
                }

                PlaybackEngine.Instance.StopVideo();

                if (!_isAborting)
                {
                    Debug.Log("SaveMp4 saved successfully: " + destinationPath);

                    if(EnableTestMode)
                        NativeGallery.SaveVideoToGallery(destinationPath, "Poc", fileName);

#if !UNITY_EDITOR
                         CallbackHandler.OnSavedCompleted(destinationPath);
#endif

                    _isAborting = false;

                    _currentCamera.gameObject.SetActive(false);

                    GC.Collect();
                }                 
            }
            else
            {
                GC.Collect();

                if (errorCode != Recorder.ErrorCode.InvalidSession)
                    Recorder.RemoveRecordingSession(sessionId);

                var reason = "Failed to export recording, error code: " + errorCode;
                ErrorHandler.Instance.DebugError(reason, "RecordingExported", "");
                //Debug.Log("Failed to export recording, error code " + errorCode + ", session id " + sessionId);
            }

            AERuntimeLoaderThreeLayer.Instance.SetAudioState(false);

            //Set the video volume as was before start recording
            PlaybackEngine.Instance.SetLastVolume();

            if(GlobalDeclarations.IsRunningAsAServer)
                AERuntimeLoaderThreeLayer.Instance.CallbackJobDone();
        }

    #endregion

#else

    #region Record Natcoder - Android


    public void StartRecording(string finalPath = "")
    {
        try
        {
            Debug.Log("Record - Start Trigger");

            SetCurrentCameraBasedOnOrientation();

            _pathRequested = finalPath;

            if (finalPath == String.Empty)
            {
                _pathRequested = Application.persistentDataPath;
            }

            var recorderWidth = 1280;
            var recorderHeight = 720;

            if(_templateType == TemplateType.Portrait)
            {
                recorderWidth = 720;
                recorderHeight = 1280;
            }


            //if (RecordFromCamera)
            //{
            //    recorderWidth = _currentCamera.scaledPixelWidth;
            //    recorderHeight = _currentCamera.scaledPixelHeight;
            //}

            _clock = new RealtimeClock();

            if (RecordAudio)
            {
                //Debug.Log(AudioSettings.outputSampleRate);
                //Debug.Log(AudioSettings.speakerMode);
                //Set the new configuration to the recorder and texture with 48KHz stereo audio
                _recorder = new MP4Recorder(recorderWidth, recorderHeight, 30, AudioSettings.outputSampleRate, (int)AudioSettings.speakerMode);

                //audioInput = new AudioInput(_recorder, _clock, AudioSource, false);
                audioInput = new AudioInput(_recorder, _clock, MainCamera.GetComponent<AudioListener>());
            }
            else
            {
                //Set the new configuration to the recorder and texture without audio
                _recorder = new MP4Recorder(recorderWidth, recorderHeight, 30);

                audioInput = null;
            }

            if (RecordFromCamera)
            {
                // Using CameraInput to record the game camera
                cameraInput = new CameraInput(_recorder, _clock, _currentCamera.TheCamera);
            }



            AudioSource.mute = false;

            _isRecording = true;
            _isAborting = false;

        }
        catch (Exception ex)
        {
            ErrorHandler.Instance.DebugError("Start recording failed", "StartRecording", ex.Message);
        }
    }



    private void ResetRecorder()
    {
        _isRecording = false;

        if (cameraInput != null)
        {
            cameraInput.Dispose();

            cameraInput = null;
        }

        if (RecordAudio)
        {
            audioInput.Dispose();
            AudioSource.mute = true;
        }

        _recorder = null;
    }

    public void Record()
    {
        try
        {
            if (_isRecording && !RecordFromCamera)
            {
                if (_asyncRecording)
                {
                    //// Async with callback
                    //AsyncGPUReadback.Request(RenderTexture, 0, request =>
                    //{
                    //    // Once complete, access the data container
                    //    var nativeArray = request.GetData<byte>();
                    //    // And commit the pixel buffer
                    //    _recorder.CommitFrame(nativeArray.ToArray(), _clock.timestamp);
                    //});
                }
                else
                {
                    //This method doesn't support audio record

                    //// We can perform a synchronous readback using a `Texture2D`
                    //var readbackTexture = new Texture2D(RenderTexture.width, RenderTexture.height);
                    //RenderTexture.active = RenderTexture;
                    //readbackTexture.ReadPixels(new Rect(0, 0, RenderTexture.width, RenderTexture.height), 0, 0);
                    //RenderTexture.active = null;

                    //// Commit the pixel buffer
                    //_recorder.CommitFrame(readbackTexture.GetPixels32(), _clock.timestamp);
                    //Destroy(readbackTexture);//MEM OPT
                }
            }
        }
        catch (Exception ex)
        {
            ErrorHandler.Instance.DebugError("Error commiting frame", "Record", ex.Message);
        }
    }

    public void CommitAudioSample(float[] sampleBuffer, int channelCount)
    {
        //_recorder.CommitSamples(sampleBuffer, _clock.timestamp);
    }

    public void ResumeRecord()
    {
        _isRecording = true;
        _clock.paused = false;

        if (RecordFromCamera)
            cameraInput = new CameraInput(_recorder, _clock, _currentCamera.TheCamera);

        AudioSource.mute = false;
    }

    public void PauseRecord()
    {
        _isRecording = false;
        _clock.paused = true;

        if (RecordFromCamera)
            cameraInput.Dispose();

        audioInput.Dispose();
        AudioSource.mute = true;
    }


    public void FinishRecord()
    {
        if (!_isRecording) return;

        Debug.Log("Record - Finishing Trigger");

        if (AERuntimeLoaderThreeLayer.Instance.IsPBSingerEnable())
        {
            PBStinger.frame = 0;
            PBStinger.gameObject.SetActive(true);
            var videoLength = PBStinger.length;
            float vOut = Convert.ToSingle(videoLength);
            Invoke("StopRecord", vOut);
        }
        else
        {
            PBStinger.gameObject.SetActive(false);
            Invoke("StopRecord", 0f);
        }
    }

    public void StopRecord()
    {
        StopOrAbortInternal(false);
    }

    public void AbortRecord()
    {
        StopOrAbortInternal(true);
    }

    private void StopOrAbortInternal(bool isAbort)
    {
        if (!_isRecording) return;
        _isRecording = false;
        _isAborting = isAbort;

        PBStinger.gameObject.SetActive(false);

        if (RecordFromCamera)
        {
            // Stop sending frames to the recorder
            cameraInput.Dispose();
            cameraInput = null;
        }

        if (RecordAudio)
        {
            audioInput.Dispose();
            AudioSource.mute = false;
        }

        Invoke("StopRecordInternal", isAbort ? 0f : 1f);

        PlaybackEngine.Instance.StopAudio();
    }

    private async void StopRecordInternal()
    {
        try
        {
            Debug.Log("Record - Stop called and ready to export");

            AERuntimeLoaderThreeLayer.Instance.SetAudioState(false);

            // Finish writing
            var path = await _recorder.FinishWriting();
            var destinationPath = string.Empty;

            if (Application.platform == RuntimePlatform.Android || true)
            {
                string fileName = Path.GetFileName(path);
                if (EnableTestMode)
                {
                    fileName = UnityEngine.Random.Range(0, 5000).ToString() + "_testVideo.mp4";
                    destinationPath = Application.persistentDataPath + fileName;
                    AERuntimeLoaderThreeLayer.Instance.PathLabel.text = destinationPath;
                    try
                    {
                        File.Move(path, destinationPath);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.Instance.DebugError("File move failed", "StopRecordInternal", ex.Message);

                        destinationPath = path;
                    }

                    ////Save it in the album / media
                    NativeGallery.SaveVideoToGallery(destinationPath, "Poc", fileName);
                }
                else
                {
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        //Save it in the album / media
                        //NativeGallery.SaveVideoToGallery(path, "Poc", fileName);

                        //Move the file to where they requested
                        if (!string.IsNullOrEmpty(_pathRequested))
                        {
                            try
                            {
                                destinationPath = _pathRequested + fileName;
                                File.Move(path, destinationPath);

                                Debug.Log("Record - Export ready");
                            }
                            catch (Exception ex)
                            {
                                Debug.Log("Record - Export fail");

                                ErrorHandler.Instance.DebugError("File move failed", "StopRecordInternal", ex.Message);

                                destinationPath = path;
                            }
                        }

                        //Move the mp4 file to folder with more easy access
                        //var destinationPath = Application.persistentDataPath + fileName;
                        //File.Move(path, destinationPath); 

                        //NativeGallery.SaveVideoToGallery(destinationPath, "Poc", fileName);
                    }
                }
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                string fileName = Path.GetFileName(path);

                if (!string.IsNullOrEmpty(fileName))
                {
                    destinationPath = Application.dataPath + "/Resources/" + fileName;
                    File.Move(path, destinationPath);
                }
            }
            else if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                string fileName = Path.GetFileName(path);

                if (!string.IsNullOrEmpty(fileName))
                {
                    var rootFolder = new System.IO.DirectoryInfo(Application.dataPath).Parent.Parent.FullName;

                    destinationPath = rootFolder + "/Videos/" + fileName;

                    File.Move(path, destinationPath);
                }
            }


            PlaybackEngine.Instance.StopVideo();

            if (!_isAborting)
            {
                Debug.Log("SaveMp4 saved successfully: " + destinationPath);

#if !UNITY_EDITOR
                    CallbackHandler.OnSavedCompleted(destinationPath);
#endif

                _isAborting = false;
            }
            _currentCamera.gameObject.SetActive(false);

            _recorder = null;
            GC.Collect();

            AERuntimeLoaderThreeLayer.Instance.SetAudioState(false);

            //Set the video volume as was before start recording
            PlaybackEngine.Instance.SetLastVolume();

            if(GlobalDeclarations.IsRunningAsAServer)
                AERuntimeLoaderThreeLayer.Instance.CallbackJobDone();
        }
        catch (Exception ex)
        {
            ErrorHandler.Instance.DebugError("Could be path error, plugin exception or missing reference", "StopRecordInternal", ex.Message);
        }
    }
    #endregion
#endif
}

