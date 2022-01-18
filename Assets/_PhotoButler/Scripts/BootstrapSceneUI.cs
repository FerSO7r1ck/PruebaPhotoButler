using System;
using UnityEngine;
using static GlobalDeclarations;

public class BootstrapSceneUI : MonoBehaviour
{
    public GameObject UIRoot;


    void Start()
    {
#if !UNITY_EDITOR
        UIRoot.SetActive(false);
        CallbackHandler.OnUnityLoaded();
#endif

        GC.Collect();
        Resources.UnloadUnusedAssets();

        //TEST
        if (EnableTestMode)
            UIRoot.SetActive(true);

        if (IsRunningAsAServer)
        {
            OnLoadVideoScene();
        }
            
    }


    public void OnLoadVideoScene()
    {
        NativeUnityInterface.Instance.LoadVideoScene();
    }

    public void OnLoadARVideoSCene()
    {
        NativeUnityInterface.Instance.LoadARScene();
    }
}
