using UnityEngine;
using UnityEngine.Video;

public class VideoTest : MonoBehaviour
{
    [SerializeField] private RenderTexture texture;
    [SerializeField] private VideoPlayer videoPlayer;

    public void OnClickPlay()
    {
        var clip = AddressablesManager.Instance.VideoClip;
        videoPlayer.clip = clip;
        videoPlayer.Play();
    }

    private void OnDestroy()
    {
        texture.Release();
    }
}
