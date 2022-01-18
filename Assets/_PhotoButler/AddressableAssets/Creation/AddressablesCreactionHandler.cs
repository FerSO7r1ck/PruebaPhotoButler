#if UNITY_EDITOR
using UnityEngine;

public class AddressablesCreactionHandler : MonoBehaviour
{

    public void SetInitialSettings()
    {
        AddressablesExtension.SetInitialSettings();
    }

    public void SetProfileRemoteBuildPath(string path)
    {
        AddressablesExtension.SetProfileRemoteBuildPath(path);
    }

    public void SetProfileRemoteLoadPath(string path)
    {
        AddressablesExtension.SetProfileRemoteLoadPath(path);
    }

    public void SetAddressable(string assetName,string labelName)
    {
        AddressablesExtension.SetAddressableGroup(assetName,labelName);
    }
}
#endif
