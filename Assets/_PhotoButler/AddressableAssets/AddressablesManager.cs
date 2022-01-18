using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.UI;
using UnityEngine.Video;

public class AddressablesManager : MonoBehaviour
{
    public static AddressablesManager Instance { get; private set ; }
    public  VideoClip VideoClip { get; private set; }
 
    private AssetLabelReference labelReference;

    private void Awake()
    {
        Caching.ClearCache();

        Instance = this;
    }

    public void LoadVideo(string labelName)
    {
        labelReference = new AssetLabelReference
        {
            labelString = labelName
        };

        //Load Addressable by Label name
        AsyncOperationHandle loadHandle = Addressables.LoadResourceLocationsAsync(labelReference.labelString);
        loadHandle.Completed += InitDialogWithImages;
    }

    private void InitDialogWithImages(AsyncOperationHandle obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            if (obj.Result != null)
            {
                IList<IResourceLocation> list = obj.Result as IList<IResourceLocation>;
                foreach (IResourceLocation irl in list)
                {
                    AsyncOperationHandle loadHandle = Addressables.LoadAssetAsync<VideoClip>(irl);
                    loadHandle.Completed += GetVideoFromAddressable;
                }
            }
        }
    }

    private void GetVideoFromAddressable(AsyncOperationHandle obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            if (obj.Result != null)
                VideoClip = obj.Result as VideoClip;
        }
    }

    public bool GetVideoClipStatus()
    {
        return VideoClip;
    }

    public void OnDestroy()
    {
        if(VideoClip != null)
            Addressables.Release(VideoClip);
    }
}

