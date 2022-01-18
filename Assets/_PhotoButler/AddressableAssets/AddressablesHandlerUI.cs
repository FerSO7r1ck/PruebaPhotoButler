#if UNITY_EDITOR
using System.Collections;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.UI;

public class AddressablesHandlerUI : MonoBehaviour
{
    [SerializeField] private AddressablesCreactionHandler addressablesCreactionHandler;
    [SerializeField] private VideoDownloaderManager videoDownloaderManager;
    [Space]
    [SerializeField] private Text settingsStatusLabel = default;
    [SerializeField] private Text profilePathLabel = default;
    [SerializeField] private Text assetNameAndLabel = default;
    [SerializeField] private Text videoPathLabel = default;
    [Space]
    [SerializeField] private InputField addressablesPathInput = default;
    [SerializeField] private InputField assetNameInput = default;
    [SerializeField] private InputField labelNameInput = default;
    [Space]
    [SerializeField] private Button [] buttons = default;
   


    private const string SETTINGS_SUCCESS = "Success, You can continue";
    private const string SETTINGS_ALREADY_CONFIGURATED = "Settings already configured";
    private const string EMPTY_PROFILE_PATH_FIELD = "Field has to be completed";
    private const string EMPTY_NAME_LABEL_FIELD = "Both fields have to be completed";
    private const string VIDEO_LOADING_FAIL = "Video loding fail, check the name of the video in the input field";
    private const string VIDEO_DOWNLOAD_SUCCESS = "Video downloading successful , you can continue";
    private const string VIDEO_DOWNLOAD_FAIL = "Video downloading fail , check the path";

    private void OnEnable()
    {
        AddressableAssetSettings.OnModificationGlobal += OnSettingsChanged;
        AddressablesExtension.OnWarningUser += OnWarningUser;
        VideoDownloaderManager.OnVideoDowloadFail += OnWarningUser;
    }

    private void OnDisable()
    {
        AddressableAssetSettings.OnModificationGlobal -= OnSettingsChanged;
        AddressablesExtension.OnWarningUser -= OnWarningUser;
        VideoDownloaderManager.OnVideoDowloadFail -= OnWarningUser;
    }

    private void Start()
    {
        if (AddressablesExtension.CheckActiveProfile())
            ActivateButtons();
    }

    public void OnClickInitialSettings()
    {
        addressablesCreactionHandler.SetInitialSettings();
    }

    public void OnClickBuildPath()
    {
       addressablesCreactionHandler.SetProfileRemoteBuildPath(addressablesPathInput.text);
    }

    public void OnClickLoadPath()
    {
        addressablesCreactionHandler.SetProfileRemoteLoadPath(addressablesPathInput.text);
    }

    public void OnClickPackAsset()
    {
        addressablesCreactionHandler.SetAddressable(assetNameInput.text , labelNameInput.text);
    }

    public void OnClickDownloadVideo()
    {
        videoDownloaderManager.DownloadVideo();
    }
    

    private void OnSettingsChanged(AddressableAssetSettings settings, AddressableAssetSettings.ModificationEvent @event , object obj)
    {
        switch (@event)
        {
            case AddressableAssetSettings.ModificationEvent.ProfileAdded:
                SetText(settingsStatusLabel,SETTINGS_SUCCESS, Color.green);
                ActivateButtons();
                break;

            case AddressableAssetSettings.ModificationEvent.ProfileModified:
                SetText(profilePathLabel,obj.ToString(), Color.green);
                break;
        }
    }

    private void OnWarningUser(AddressablesExtension.Warnings warning)
    {
        switch (warning)
        {
            case AddressablesExtension.Warnings.SettingsAlreadyConfigured:
                SetText(settingsStatusLabel, SETTINGS_ALREADY_CONFIGURATED, Color.red);
                break;

            case AddressablesExtension.Warnings.EmptyPathField:
                SetText(profilePathLabel, EMPTY_PROFILE_PATH_FIELD, Color.red);
                break;

            case AddressablesExtension.Warnings.EmptyNameOrLabelField:
                SetText(assetNameAndLabel, EMPTY_NAME_LABEL_FIELD, Color.red);
                break;

            case AddressablesExtension.Warnings.ResourcesLoadingFail:
                SetText(assetNameAndLabel, VIDEO_LOADING_FAIL, Color.red);
                break;

            case AddressablesExtension.Warnings.VideoDownloadSucces:
                SetText(videoPathLabel, VIDEO_DOWNLOAD_SUCCESS, Color.green);
                break;

            case AddressablesExtension.Warnings.VideoDownloadFail:
                SetText(videoPathLabel, VIDEO_DOWNLOAD_FAIL, Color.red);
                break;
        }
    }

    private void SetText(Text textToModify , string text , Color color)
    {
        textToModify.gameObject.SetActive(true);
        textToModify.text = text;
        textToModify.color = color;
        StartCoroutine(DeactivateText(textToModify));
    }

    public void ActivateButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].interactable = true;
    }

    private IEnumerator DeactivateText(Text text)
    {
        yield return new WaitForSeconds(5f);
        text.gameObject.SetActive(false);
    }
}
#endif