using Nexweron.Core.MSK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class TemplateData : MonoBehaviour
{
# if UNITY_WEBGL
    public List<GameObject> videoFrames;
#endif

    public Camera MainCamera;
    public VideoPlayer ForegroundVideo;
    public RawImage ForegroundVideoRender;
    public Material ChromaMaterial;
    public Material AlphaMaterial;

    public Image PlaceholderImage;
    public Image BackPlaceholderImage;
    public Image LogoImage;

    public Transform VideoPlayersTransform;
    public GameObject FingerSign;
    public Image FingerOutline;
    public Image FingerFill;
    public Image BackgroundTop;
    public Image BackgroundBottom;

    public RectTransform UGCRootBack;
    public RectTransform UGCRootFront;
    public RectTransform StickerRoot;
    public RectTransform ScreenSpaceUIRoot;

    public UGCDragHandler UGCDragBackHandler;
    public UGCPinchHandler UGCPinchBackHandler;

    public UGCDragHandler UGCDragFrontHandler;
    public UGCPinchHandler UGCPinchFrontHandler;

    public RectTransform Canvas;

    [HideInInspector]
    public int Width;
    [HideInInspector]
    public int Height;

}
