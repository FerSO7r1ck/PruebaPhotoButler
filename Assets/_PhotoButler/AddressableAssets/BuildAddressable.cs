#if UNITY_EDITOR
using UnityEditor.AddressableAssets;
using UnityEngine;

public class BuildAddressable : MonoBehaviour
{

    public static void Build() {
        var path = Application.dataPath + "/AddressableAssetsData/Android/addressables_content_state.bin";
        AddressablesExtension.BuildOrUpdatePreviousBuild(path);
    }
}
#endif

