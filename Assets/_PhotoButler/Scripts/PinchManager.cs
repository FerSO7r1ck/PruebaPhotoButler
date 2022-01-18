using HedgehogTeam.EasyTouch;
using UnityEngine;

public abstract class PinchManager : MonoBehaviour
{
    protected void OnEnable()
    {
        EasyTouch.On_Pinch += On_Pinch;
    }

    protected void OnDisable()
    {
        EasyTouch.On_Pinch -= On_Pinch;
    }

    protected void OnDestroy()
    {
        EasyTouch.On_Pinch -= On_Pinch;
    }

    protected abstract void On_Pinch(Gesture gesture);
}
