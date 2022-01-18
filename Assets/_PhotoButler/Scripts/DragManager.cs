using HedgehogTeam.EasyTouch;
using UnityEngine;

public abstract class DragManager : MonoBehaviour
{
    protected void OnEnable()
    {
        EasyTouch.On_TouchDown += On_TouchDown;
        EasyTouch.On_TouchStart += On_TouchStart;
        EasyTouch.On_TouchUp += On_TouchUp;
    }

    protected void OnDisable()
    {
        EasyTouch.On_TouchDown -= On_TouchDown;
        EasyTouch.On_TouchStart -= On_TouchStart;
        EasyTouch.On_TouchUp -= On_TouchUp;
    }

    protected void OnDestroy()
    {
        EasyTouch.On_TouchDown -= On_TouchDown;
        EasyTouch.On_TouchStart -= On_TouchStart;
        EasyTouch.On_TouchUp -= On_TouchUp;
    }

    protected abstract void On_TouchStart(Gesture gesture);
    protected abstract void On_TouchDown(Gesture gesture);
    protected abstract void On_TouchUp(Gesture gesture);

}
