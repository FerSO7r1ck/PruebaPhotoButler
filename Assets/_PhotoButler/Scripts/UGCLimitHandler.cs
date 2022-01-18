using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static GlobalDeclarations;

public class UGCLimitHandler : MonoBehaviour
{
    public bool ApplyLimitPosition = false;

    private Vector3 _lastValidPosition;

    private void Start()
    {
        _lastValidPosition = transform.localPosition;
    }

    public void SetLastValidPosition(Vector3 lastValidPosition)
    {
        _lastValidPosition = lastValidPosition;
    }

    public Vector3 GetLastValidPosition()
    {
        return _lastValidPosition;
    }

    public void SetApplyLimitPositionState(bool state)
    {
        ApplyLimitPosition = state;
    }
   
}
