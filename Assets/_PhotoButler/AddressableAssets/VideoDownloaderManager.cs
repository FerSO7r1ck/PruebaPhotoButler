#if UNITY_EDITOR
using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class VideoDownloaderManager : MonoBehaviour
{
    private int uniqueIdentifier;

    private void Awake()
    {
        uniqueIdentifier = PlayerPrefs.GetInt("ID");
    }

    public static event Action<AddressablesExtension.Warnings> OnVideoDowloadFail;

    public void DownloadVideo()
    {
        NativeGallery.GetVideoFromGallery((url) => {

            if (string.IsNullOrEmpty(url))
                return;

            StartCoroutine(GetVideoClip(url)); }, "Video", "video/*");
    }

        IEnumerator GetVideoClip(string filePath)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(filePath))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError)
                    OnVideoDowloadFail?.Invoke(AddressablesExtension.Warnings.VideoDownloadFail);
                else
                {
                    string[] newData = filePath.Split(new char[] { '.' });
                    var name = newData[0];
                    var extension = newData[1];

                    if (www.downloadHandler.data.Length > 0)
                    {
                        File.WriteAllBytes(Application.dataPath + "/Resources/" + "Video" + uniqueIdentifier + "." + extension, www.downloadHandler.data);
                        uniqueIdentifier++;
                        PlayerPrefs.SetInt("ID", uniqueIdentifier);

                        OnVideoDowloadFail?.Invoke(AddressablesExtension.Warnings.VideoDownloadSucces);
                    }
                    else
                        OnVideoDowloadFail?.Invoke(AddressablesExtension.Warnings.VideoDownloadFail);
                }
            }

            AssetDatabase.Refresh();
        }
    }


#endif
