using UnityEngine;
using UnityEngine.Video;
using Nexweron.Core.MSK;

public class MSKBridgeVideoPlayer : MonoBehaviour
{
	public VideoPlayer videoPlayer;
	public MSKController mskController;

	private Texture _texture;

    private void Start()
    {
        //Invoke("TriggerSetTexture", 4f);
    }

    private void TriggerSetTexture()
    {
        mskController.SetSourceTexture(_texture);
    }

    private void UpdateProperties() {
		if (videoPlayer != null) {
			if (mskController != null) {
				if (_texture != videoPlayer.texture) {
					_texture = videoPlayer.texture;
					mskController.SetSourceTexture(_texture);
				}
			} else {
				Debug.LogError("MSKBridgeVideoPlayer | mskController = null");
			}
		} else {
			Debug.LogError("MSKBridgeVideoPlayer | videoPlayer = null");
		}
	}

	private void OnEnable() {
		if (videoPlayer != null) {
			if (mskController != null) {
				videoPlayer.prepareCompleted += VideoPlayerPrepareCompleted;
				if (videoPlayer.isPrepared) {
					UpdateProperties();
				}
				videoPlayer.frameReady += VideoPlayerFrameReady;
				videoPlayer.sendFrameReadyEvents = true;
			} else {
				Debug.LogError("MSKBridgeVideoPlayer | mskController = null");
			}
		} else {
			Debug.LogError("MSKBridgeVideoPlayer | videoPlayer = null");
		}
	}

	private void OnDisable() {
		if (videoPlayer != null) {
			videoPlayer.prepareCompleted -= VideoPlayerPrepareCompleted;
			videoPlayer.frameReady -= VideoPlayerFrameReady;
		}
	}

	private void VideoPlayerPrepareCompleted(VideoPlayer source) {
		UpdateProperties();
	}

	private void VideoPlayerFrameReady(VideoPlayer source, long frameIdx) {

        if(mskController != null && videoPlayer != null && videoPlayer.texture != null)
        {
            mskController.RenderOut(videoPlayer.texture as RenderTexture);
        }
	}	
}
