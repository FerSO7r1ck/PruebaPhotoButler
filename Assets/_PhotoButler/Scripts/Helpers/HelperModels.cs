using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;
using UnityEngine.Video;
using static GlobalDeclarations;

[Preserve]
public class MovieObject
{
    public bool IsRoot;
    public string Name;
    public MovieObject Parent;
    public GameObject GameObject;
    public List<AnimationUnit> Animations;
    public Vector3 Position;
    public Vector3 Scale;
    public Vector3 EulerRotation;
    public Vector3 EulerOrientation;
    public string LayerType;
    public float LayerInPoint;
    public float LayerOutPoint;
    public int FrameInPoint;
    public int FrameOutPoint;
    public Dictionary<string, object> LayerSpecificData;
    public Vector3 LocalInitScale;
    public Vector3 LocalInitOrientation;
    public Image UIImage;
    public RawImage RawImage;
    public VideoPlayer VideoPlayer;
    public AudioSource VideoAudioSource;
    public float CameraZoom;
    public bool isWorldSpace;
    public Light Light;
    public UGCPositionType ugcPositionBasedOnVideo;
    public GameObject MaskGO;
    public GameObject BorderGO;
    public GameObject MovementRootGO;
    public bool HasMaskBeenApplied;
    public bool HasScaleBeenApplied;
    public bool HasBeenReplaced;
    public bool HasResizeToMatchFrameApplied;
    public Vector3 InitialScale = Vector3.one;
    public List<MovieObject> LinkedMovieObjects = new List<MovieObject>();
    public float PendingScaleDelta;
    public float TotalScaleModified;
    public Vector3 TotalMovementModified;
    public Vector3 LastMaskPosition;
    public bool HasUGCFamily;
    public UGCFamily UGCFamily;
    public Vector3 ScaleLimitMin;
    public Vector3 ScaleLimitMax;
    public Vector3 MovementRadiusLimit;
    public UGCLimitHandler UGCLimitHandler;
    public bool IsBeingScaled;
    public MovieObjectType MovieObjectType;
}

[Preserve]
public class Sticker
{
    public GameObject GameObject;
    public string Name;
    public Transform Parent;
    public RawImage RawImage;
    public Vector3 ScaleLimitMin;
    public Vector3 ScaleLimitMax;
}


[Preserve]
public class UGCFamily
{
    public int FramePreview;
    public Vector3 InitialScale;
    public float UserZoomDelta;
    public Vector3 UserMovement;
    public List<MovieObject> MovieObjects = new List<MovieObject>();
}


[Preserve]
public class AnimationFrame
{
    public int Frame;
    public MovieObject AnimatedObject;
    public AnimationType AnimationType;
    public Vector3 Value;
    public float Time;
}

[Preserve]
public class AnimationUnit
{
    public AnimationType AnimationType;
    public List<AnimationKeyFrame> AnimationSteps;
}

[Preserve]
public class AnimationKeyFrame
{
    public Vector3 Init;
    public Vector3 End;
    public float Time;
}

[Preserve]
public class AECamera
{
    public float Zoom;
    public Camera Camera;
    public float InPoint;
    public float OutPoint;
    public int CameraIndex;
}

[Preserve]
public class CameraInOut
{
    public float InPoint;
    public float OutPoint;
    public int CameraIndex;
}

[Preserve]
public class UGCObject
{
    public Image UGCImage;
    public Texture2D Tex;
    public Vector3 Position;
    public Vector3 Scale;
    public Vector3 PosDelta;
    public Vector3 ScaleDelta;
}

[Preserve]
public class UGCMetaObject
{
    public string id;
    public Vector3 position;
    public Size size;
    public string previewFrame;
}

[Preserve]
public class Size
{
    public string width;
    public string height;
}

[Preserve]
public class MetaObject
{
    public string themeId;
    public string themeTitle;
    public string themeJSONName;

    [JsonProperty(Required = Required.Default)]
    public string chromaKey;
    public bool usePBStinger;
    public bool useBackCover;
    [JsonProperty(Required = Required.Default)]
    public bool hideLogo;
    public List<UGCMetaObject> ugc;
}

[Preserve]
[Serializable]
public class JobMeta
{
    public string templateid;
    public List<UGCMetaObject> ugcs;
}

[Preserve]
public class JobRequest
{
    public string SQSMessage;
    public string SQSReceiptHandle;
}

[Preserve]
public class WebGLTemplate
{
    public string themeId;
    public string cover;
    public List<WebGLugc> ugc;
}

public class WebGLugc
{
    public string id;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 orientation;
    public Size size;
    public string previewImage;
}

public class WebGLTemplateNames
{
    public List<string> templateName;
}

public class AWSLambdaHeader
{
    public string authorizationToken = "allow";
}
