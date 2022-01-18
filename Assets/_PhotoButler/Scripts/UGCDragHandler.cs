using DG.Tweening;
using HedgehogTeam.EasyTouch;

using UnityEngine;
using UnityEngine.UI;

public class UGCDragHandler : DragManager
{

    private int fingerId = -1;
    private bool drag = true;

    private string _ugcId = "";

    public float SmoothTime = 0.15f;

    private GameObject _maskGO;

    private MovieObject _movieObject;
    private Image _movieObjectImage;
    private Vector2 _movementOffLimit;

    public void SetUGCId(string id)
    {
        _ugcId = id;
    }

    public void SetMaskGO(GameObject maskGO, MovieObject movieObject)
    {
        _maskGO = maskGO;

        _movieObject = movieObject;

        _movieObjectImage = _movieObject.GameObject.GetComponent<Image>();
    }

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

        if (fingerId == gesture.fingerIndex && drag && gameObject != null && !string.IsNullOrEmpty(_ugcId))
        {
            if (gesture.isOverGui)
            {
                if ((gesture.pickedUIElement == gameObject || gesture.pickedUIElement.transform.IsChildOf(transform)))
                {
                    AERuntimeLoaderThreeLayer.Instance.MoveUGC(_ugcId, gesture.deltaPosition);
                }
            }
        }
    }

    void ApplyLimitOffsetMovement()
    {
        if(_movieObject != null && _movieObject.UGCLimitHandler.ApplyLimitPosition)
        {
            //var positionDifference = _movieObject.GameObject.transform.localPosition - _movieObject.UGCLimitHandler.GetLastValidPosition();
            //var averageDifference = (Mathf.Abs(positionDifference.x) + Mathf.Abs(positionDifference.y)) / 2f;
            //var movementSpeed = 0.002f;

            //_movieObjectImage.raycastTarget = false;

            //_movieObject.GameObject.transform.DOLocalMove(_movieObject.UGCLimitHandler.GetLastValidPosition(), averageDifference * movementSpeed).OnComplete(() => RepositionCompleteCallback());

            _movieObject.GameObject.transform.DOLocalMove(_movieObject.UGCLimitHandler.GetLastValidPosition(), 0.3f).OnComplete(() => RepositionCompleteCallback());
        }
    }

    void RepositionCompleteCallback()
    {
        _movieObject.UGCLimitHandler.SetApplyLimitPositionState(false);

        AERuntimeLoaderThreeLayer.Instance.RecalculateUGCMovementOffset(_movieObject);

        _movieObjectImage.raycastTarget = true;

        AERuntimeLoaderThreeLayer.Instance.RecalculateUGCDeltaPinch(_movieObject);
    }

    protected override void On_TouchUp(Gesture gesture)
    {
        if (fingerId == gesture.fingerIndex)
        {
            fingerId = -1;

            ApplyLimitOffsetMovement();

            if(_movieObject != null && !_movieObject.UGCLimitHandler.ApplyLimitPosition)
            {
                AERuntimeLoaderThreeLayer.Instance.RecalculateUGCDeltaPinch(_movieObject);
            }
        }
    }

    public void ResetPosition()
    {
        transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
    }
}

