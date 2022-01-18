using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static NativeGallery;
using static GlobalDeclarations;
using System.IO;
using System;

public class TestManager : MonoBehaviour
{
    public static TestManager Instance;

    public Dropdown TemplatesDropdown;
    public Dropdown UGCDropdown;  

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PrepareDropDowns();
    }

    public void PrepareDropDowns()
    {
        string[] enumValues = Enum.GetNames(typeof(TemplatesFolderNames));
        List<string> values = new List<string>(enumValues);

        TemplatesDropdown.AddOptions(values);

        string[] enumUGCValues = Enum.GetNames(typeof(UGCIndex));
        List<string> ugcValues = new List<string>(enumUGCValues);

        UGCDropdown.AddOptions(ugcValues);
    }


    public void SetTemplateName()
    {
        var templatePath = "C:/Trabajo/PBTemplates/AEJson/" + (TemplatesFolderNames)TemplatesDropdown.value;

#if UNITY_IOS && !UNITY_EDITOR
                    templatePath = "/Users/trick/downloads/Templates_Renamed/" + (TemplatesFolderNames)TemplatesDropdown.value;
                  
#endif


#if !UNITY_EDITOR

#if UNITY_ANDROID
                if (EnableTestMode)
                {
                    var rootFolder = new System.IO.DirectoryInfo(Application.persistentDataPath);

                    templatePath = rootFolder + "/Templates/" + (TemplatesFolderNames)TemplatesDropdown.value;
                }
#endif

#if UNITY_IOS

                if (EnableTestMode)
                {
                    templatePath = Application.persistentDataPath + "/" + (TemplatesFolderNames)TemplatesDropdown.value;
                }

#endif

#endif
        AERuntimeLoaderThreeLayer.Instance.SetTemplateNameOnTestMode(templatePath);

        AERuntimeLoaderThreeLayer.Instance.OnReadFile();
    }

    public void LoadImage()
    {
        NativeGallery.GetImageFromGallery(LoadImageCallback, "Image", "image/*");
    }

    private void LoadImageCallback(string path)
    {
        if (string.IsNullOrEmpty(path))
            return;

        string ugcName = "UGC " + ((UGCIndex)UGCDropdown.value).ToString().Substring(3);

        NativeUnityInterface.Instance.ReplaceUGCFromPath(ugcName + "_" + path);
    }

    public void LoadVideo()
    {
        NativeGallery.GetVideoFromGallery(LoadVideoCallback, "Video", "video/*");
    }

    private void LoadVideoCallback(string path)
    {
        if (string.IsNullOrEmpty(path))
            return;

        string ugcName = "UGC " + ((UGCIndex)UGCDropdown.value).ToString().Substring(3);

        NativeUnityInterface.Instance.ReplaceUGCVideoFromPath(ugcName + "_" + path);
    }


    public void LoadImage64()
    {
        NativeGallery.GetImageFromGallery(LoadImage64Callback, "Image", "image/*");
    }

    private void LoadImage64Callback(string path)
    {
        if (string.IsNullOrEmpty(path))
            return;

        byte[] fileData;
        fileData = File.ReadAllBytes(path);

        var tex = new Texture2D(2, 2);
        tex.LoadImage(fileData);

        if (path.Contains(".png"))
        {
            var imageBase64 = System.Convert.ToBase64String(tex.EncodeToPNG());

            string ugcName = "UGC " + ((UGCIndex)UGCDropdown.value).ToString().Substring(3);

            NativeUnityInterface.Instance.ReplaceUGC(ugcName + "_" + imageBase64);
        }
        else
        {
            var imageBase64 = System.Convert.ToBase64String(tex.EncodeToJPG());

            string ugcName = "UGC " + ((UGCIndex)UGCDropdown.value).ToString().Substring(3);

            NativeUnityInterface.Instance.ReplaceUGC(ugcName + "_" + imageBase64);
        }
        

        DestroyImmediate(tex);
    }
}
