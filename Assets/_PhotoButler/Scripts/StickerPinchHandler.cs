using HedgehogTeam.EasyTouch;

public class StickerPinchHandler : PinchManager
{
    protected override void On_Pinch(Gesture gesture)
    {
        if (PlaybackEngine.Instance.IsPlaying()) return;

        if (gesture.isOverGui)
        {
            if (gesture.pickedUIElement == gameObject || gesture.pickedUIElement.transform.IsChildOf(transform))
                AERuntimeLoaderThreeLayer.Instance.ScaleSticker(gameObject.name, gesture.deltaPinch);
        }
    }
}
