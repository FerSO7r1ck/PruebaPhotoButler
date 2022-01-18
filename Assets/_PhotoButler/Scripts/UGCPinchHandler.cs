using HedgehogTeam.EasyTouch;
using UnityEngine;
using UnityEngine.UI;

public class UGCPinchHandler : PinchManager
{
    private GameObject _goToScale;
    private MovieObject _movieObject;

    public void SetGameObjectToScale(GameObject go, MovieObject movieObject)
    {
        _goToScale = go;
        _movieObject = movieObject;
    }

    protected override void On_Pinch(Gesture gesture)
    {
        if (PlaybackEngine.Instance.IsPlaying()) return;
        if (gesture.isOverGui)
        {
            if (gesture.pickedUIElement == gameObject || gesture.pickedUIElement.transform.IsChildOf(transform) && _goToScale != null)
            {
                _movieObject.IsBeingScaled = true;

                AERuntimeLoaderThreeLayer.Instance.HideFingerSign();

                var goImage = _goToScale.GetComponent<Image>();

                //Dont allow ugc manipulation before replace the UGC
                if (goImage != null && goImage.type == Image.Type.Tiled)
                    return;

                AERuntimeLoaderThreeLayer.Instance.ScaleUGC(_goToScale.name, gesture.deltaPinch);
            }
        }
    }
}
