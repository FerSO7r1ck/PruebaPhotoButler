#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Video;
using Object = UnityEngine.Object;

public static class AddressablesExtension
{
    public static event Action<Warnings> OnWarningUser;

    private static AddressableAssetSettings settings;
    private static List<AddressableAssetEntry> entriesAdded;

    private const string REMOTE_BUILD_PATH_FIELD = "RemoteBuildPath";
    private const string REMOTE_LOAD_PATH_FIELD = "RemoteLoadPath";
    private const string BUILD_PATH_FIELD_ID = "03a4fa1e6551ebf40828c7cbfa564022";
    private const string LOAD_PATH_FIELD_ID = "d88790e891c8f1849a128aabefbc4070";
    private const string DEFAULT_PROFILE_NAME = "Default";
    private const string MY_DEFAULT_PROFILE = "My Default Profile";
    private const string MY_DEFAULT_GROUP = "My Default Group";

    #region SETTINGS

    public static void SetInitialSettings()
    {
        var profilename = MY_DEFAULT_PROFILE;

        // We are using default settings
        settings = AddressableAssetSettingsDefaultObject.Settings;

        //Build Remote Catalog Activation

        if (!settings.BuildRemoteCatalog)
        {
            settings.BuildRemoteCatalog = true;

            settings.RemoteCatalogBuildPath.SetVariableByName(settings, REMOTE_BUILD_PATH_FIELD);
            settings.RemoteCatalogBuildPath.SetVariableByName(settings, REMOTE_LOAD_PATH_FIELD);
        }

        //Profile Creation

        if (settings.profileSettings.GetProfileId(MY_DEFAULT_PROFILE) != settings.activeProfileId)
            CreateProfile(settings, profilename);
        else
            OnWarningUser?.Invoke(Warnings.SettingsAlreadyConfigured);
    }

    public static void CreateProfile(AddressableAssetSettings settings, string profileName)
    {
        var profileId = settings.profileSettings.GetProfileId(DEFAULT_PROFILE_NAME);
        settings.profileSettings.AddProfile(profileName, profileId);

        settings.SetDirty(AddressableAssetSettings.ModificationEvent.ProfileAdded, settings.activeProfileId, true, true);

        SetProfile(profileName);
    }

    public static void SetProfile(string profile)
    {
        string profileId = settings.profileSettings.GetProfileId(profile);

        if (String.IsNullOrEmpty(profileId))
            Debug.LogWarning($"Couldn't find a profile named, {profile}, " + $"using current profile instead.");

        else
            settings.activeProfileId = profileId;
    }

    public static bool CheckActiveProfile()
    {
        settings = AddressableAssetSettingsDefaultObject.Settings;
        return settings.profileSettings.GetProfileName(settings.activeProfileId) == MY_DEFAULT_PROFILE;
    }

    public static void SetProfileRemoteBuildPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            OnWarningUser?.Invoke(Warnings.EmptyPathField);
            return;
        }

        settings = AddressableAssetSettingsDefaultObject.Settings;
        settings.profileSettings.SetValue(settings.activeProfileId, REMOTE_BUILD_PATH_FIELD, path);

        settings.SetDirty(AddressableAssetSettings.ModificationEvent.ProfileModified, REMOTE_BUILD_PATH_FIELD + ": " + path , true, true);
    }

    public static void SetProfileRemoteLoadPath(string path)
    {
        if(string.IsNullOrEmpty(path))
        {
            OnWarningUser?.Invoke(Warnings.EmptyPathField);
            return;
        }

        settings = AddressableAssetSettingsDefaultObject.Settings;
        settings.profileSettings.SetValue(settings.activeProfileId, REMOTE_LOAD_PATH_FIELD, path);

        settings.SetDirty(AddressableAssetSettings.ModificationEvent.ProfileModified, REMOTE_LOAD_PATH_FIELD + ": " + path, true, true);
    }

    public static void SetAddressableGroup(string assetName, string labelName)
    {
        if (String.IsNullOrEmpty(assetName) || String.IsNullOrEmpty(labelName))
        {
            OnWarningUser?.Invoke(Warnings.EmptyNameOrLabelField);
            return;
        }

        Object obj;

        if (!Resources.Load<VideoClip>(assetName))
        {
            OnWarningUser?.Invoke(Warnings.ResourcesLoadingFail);
            return;
        }

        obj = Resources.Load<VideoClip>(assetName);

        var groupName = MY_DEFAULT_GROUP;

        var settings = AddressableAssetSettingsDefaultObject.Settings;

        if (settings)
        {
            var group = settings.FindGroup(groupName);
            if (!group)
            {
                group = settings.CreateGroup(groupName, false, false, true, null, typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));

               
                var shema = group.GetSchema<BundledAssetGroupSchema>();

                //Shema configuration
                shema.Compression = BundledAssetGroupSchema.BundleCompressionMode.Uncompressed;
                shema.BuildPath.SetVariableById(AddressableAssetSettingsDefaultObject.Settings, BUILD_PATH_FIELD_ID);
                shema.LoadPath.SetVariableById(AddressableAssetSettingsDefaultObject.Settings, LOAD_PATH_FIELD_ID);
            }

            var assetpath = AssetDatabase.GetAssetPath(obj);
            var guid = AssetDatabase.AssetPathToGUID(assetpath);

            var e = settings.CreateOrMoveEntry(guid, group, false, false);
            entriesAdded = new List<AddressableAssetEntry> { e };

            group.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, false, true);
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, true, false);

            SetLabel(obj.name, labelName);
        }
    }

    public static void SetLabel(string assetName, string labelName)
    {
        for (int i = 0; i < entriesAdded.Count; i++)
        {
            if (entriesAdded[i].MainAsset.name == assetName)
                entriesAdded[i].SetLabel(labelName, true, true);
        }
    }
    #endregion

    #region BUILD
    public static void BuildOrUpdatePreviousBuild(string contentStateDataPath)
    {
        var contentFileExist = File.Exists(contentStateDataPath);

        if (contentFileExist)
            BuildContentUpdate(contentStateDataPath);
        else
        {
            CleanBuildChache();
            AddressableAssetSettings.BuildPlayerContent();
        }
    }

    public static AddressablesPlayerBuildResult BuildContentUpdate(string contentStateDataPath)
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        ContentUpdateScript.BuildContentUpdate(settings, contentStateDataPath);

        return new AddressablesPlayerBuildResult();
    }

    private static void CleanBuildChache()
    {
        AddressableAssetSettings.CleanPlayerContent(AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);
    }
    #endregion

    #region EVENTS
    public enum Warnings
    {
        SettingsAlreadyConfigured,
        EmptyPathField,
        EmptyNameOrLabelField,
        ResourcesLoadingFail,
        VideoDownloadFail,
        VideoDownloadSucces,
    }
    #endregion
}

#endif