using HedgehogTeam.EasyTouch;
using UnityEngine;

public class UGCDrag : MonoBehaviour
{
    private int fingerId = -1;
    private bool drag = true;

    private string _ugcId = "";

    private Vector3 _startPosition;
    private Vector3 _velocity;
    private bool _underInertia;
    private float _time = 0.0f;
    public float SmoothTime = 0.15f;

    private RectTransform _rectTransform;
    private float _horizontalLimit;
    private float _verticalLimit;

    private void Start()
    {
        _startPosition = transform.localPosition;

        _rectTransform = GetComponent<RectTransform>();

        _horizontalLimit = _rectTransform.sizeDelta.x / 2;
        _verticalLimit = _rectTransform.sizeDelta.y / 2;
    }

    private void Update()
    {
        Inertia();
    }

    void OnEnable()
    {
        EasyTouch.On_TouchDown += On_TouchDown;
        EasyTouch.On_TouchStart += On_TouchStart;
        EasyTouch.On_TouchUp += On_TouchUp;
        EasyTouch.On_TouchStart2Fingers += On_TouchStart2Fingers;
        EasyTouch.On_TouchDown2Fingers += On_TouchDown2Fingers;
        EasyTouch.On_TouchUp2Fingers += On_TouchUp2Fingers;
    }

    private void OnDisable()
    {
        EasyTouch.On_TouchDown -= On_TouchDown;
        EasyTouch.On_TouchStart -= On_TouchStart;
        EasyTouch.On_TouchUp -= On_TouchUp;
        EasyTouch.On_TouchStart2Fingers -= On_TouchStart2Fingers;
        EasyTouch.On_TouchDown2Fingers -= On_TouchDown2Fingers;
        EasyTouch.On_TouchUp2Fingers -= On_TouchUp2Fingers;
    }

    void OnDestroy()
    {
        EasyTouch.On_TouchDown -= On_TouchDown;
        EasyTouch.On_TouchStart -= On_TouchStart;
        EasyTouch.On_TouchUp -= On_TouchUp;
        EasyTouch.On_TouchStart2Fingers -= On_TouchStart2Fingers;
        EasyTouch.On_TouchDown2Fingers -= On_TouchDown2Fingers;
        EasyTouch.On_TouchUp2Fingers -= On_TouchUp2Fingers;
    }

    public void SetUgcId(string id)
    {
        _ugcId = id;
    }

    void On_TouchStart(Gesture gesture)
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

    void On_TouchDown(Gesture gesture)
    {
        if (PlaybackEngine.Instance.IsPlaying()) return;
        if (fingerId == gesture.fingerIndex && drag && gameObject != null)
        {
            if (gesture.isOverGui)
            {
                if ((gesture.pickedUIElement == gameObject || gesture.pickedUIElement.transform.IsChildOf(transform)))
                {
                    Vector3 _prevPosition = transform.GetChild(0).position;

                    AERuntimeLoaderThreeLayer.Instance.MoveUGCToNewPositionExternal(_ugcId, gesture.deltaPosition.x, gesture.deltaPosition.y, Screen.width / 2 , Screen.height / 2);

                    _velocity = (transform.GetChild(0).position + (Vector3)gesture.deltaPosition) - _prevPosition;
                }
            }
        }
    }

    void On_TouchUp(Gesture gesture)
    {
        if (fingerId == gesture.fingerIndex)
        {
            fingerId = -1;
            _underInertia = true;
        }
    }

    void Inertia()
    {
        if (_underInertia && _time <= SmoothTime)
        {
            AERuntimeLoaderThreeLayer.Instance.MoveUGCToNewPositionExternal(_ugcId, _velocity.x, _velocity.y, Screen.width / 2, Screen.height / 2);

            _velocity = Vector3.Lerp(_velocity, Vector3.zero, _time);
            _time += Time.smoothDeltaTime;
        }
        else
        {
            _underInertia = false;
            _time = 0.0f;
        }
    }


    void On_TouchStart2Fingers(Gesture gesture)
    {
        if (PlaybackEngine.Instance.IsPlaying()) return;
        if (gesture.isOverGui && drag && gameObject != null)
        {
            if ((gesture.pickedUIElement == gameObject || gesture.pickedUIElement.transform.IsChildOf(transform)) && fingerId == -1)
            {
                transform.SetAsLastSibling();
            }
        }
    }


    void On_TouchDown2Fingers(Gesture gesture)
    {
        if (PlaybackEngine.Instance.IsPlaying()) return;
        if (gesture.isOverGui && gameObject != null)
        {
            if (gesture.pickedUIElement == gameObject || gesture.pickedUIElement.transform.IsChildOf(transform))
            {
                if ((gesture.pickedUIElement == gameObject || gesture.pickedUIElement.transform.IsChildOf(transform)))
                {
                    transform.GetChild(0).position += (Vector3)gesture.deltaPosition;
                }
                drag = false;
            }
        }
    }

    void On_TouchUp2Fingers(Gesture gesture)
    {
        if (gesture.isOverGui)
        {
            if (gesture.pickedUIElement == gameObject || gesture.pickedUIElement.transform.IsChildOf(transform))
            {
                drag = true;
            }
        }
    }


    public void ResetPosition()
    {
        transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
    }

}
