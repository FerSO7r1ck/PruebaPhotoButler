using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorHandler : MonoBehaviour
{
    public static ErrorHandler Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DebugError(string errorReason, string errorOrigin, string errorText)
    {
        Debug.Log("Error Reason: " + errorReason + " - Error Origin: " + errorOrigin + " - ErrorText: " + errorText);
    }

}
