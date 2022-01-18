using HedgehogTeam.EasyTouch;

public class StickerDragHandler : DragManager
{
    private int fingerId = -1;
    private bool drag = true;
    public float SmoothTime = 0.15f;

    protected override void On_TouchStart(Gesture gesture)
    {
        if (PlaybackEngine.Instance.IsPlaying()) return;

        if (gesture.isOverGui && drag && gameObject != null)
        {
            if ((gesture.pickedUIElement == gameObject || gesture.pickedUIElement.transform.IsChildOf(transform)) && fingerId == -1)
            {
                fingerId = gesture.fingerIndex;
                transform.SetAsLastSibling();

                AERuntimeLoaderThreeLayer.Instance.HideFingerSign();
            }
        }
    }

    protected override void On_TouchDown(Gesture gesture)
    {
        if (PlaybackEngine.Instance.IsPlaying()) return;

        if (fingerId == gesture.fingerIndex && drag && gameObject != null)
        {
            if (gesture.isOverGui)
            {
                if ((gesture.pickedUIElement == gameObject || gesture.pickedUIElement.transform.IsChildOf(transform)))
                    AERuntimeLoaderThreeLayer.Instance.MoveSticker(gameObject.name, gesture.deltaPosition);
            }
        }
    }

    protected override void On_TouchUp(Gesture gesture)
    {
        if (fingerId == gesture.fingerIndex)
            fingerId = -1;
    }
}


