using HedgehogTeam.EasyTouch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGCPinch : MonoBehaviour
{

    public void OnEnable()
    {
        EasyTouch.On_Pinch += On_Pinch;
    }

    private void OnDisable()
    {
        EasyTouch.On_Pinch -= On_Pinch;
    }

    public void OnDestroy()
    {
        EasyTouch.On_Pinch -= On_Pinch;
    }


    void On_Pinch(Gesture gesture)
    {
        if (PlaybackEngine.Instance.IsPlaying()) return;
        if (gesture.isOverGui)
        {
            if (gesture.pickedUIElement == gameObject || gesture.pickedUIElement.transform.IsChildOf(transform))
            {
                AERuntimeLoaderThreeLayer.Instance.HideFingerSign();

                var childTransform = transform.GetChild(0);

                //Multiply by 0.05f to reduce the scale sensitive
                var newScale = new Vector3(childTransform.localScale.x + gesture.deltaPinch * 0.05f * Time.deltaTime, childTransform.localScale.y + gesture.deltaPinch * 0.05f * Time.deltaTime, childTransform.localScale.z);

                var scaleXClamped = Mathf.Clamp(newScale.x, 0.2f, 10f);
                var scaleYClamped = Mathf.Clamp(newScale.y, 0.2f, 10f);

                childTransform.localScale  = new Vector3(scaleXClamped, scaleYClamped, newScale.z);
            }
        }
    }
}
