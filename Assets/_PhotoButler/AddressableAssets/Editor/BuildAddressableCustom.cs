using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(BuildAddressable))]
public class BuildAddressableCustom : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Label("BUILD OPTIONS");

        if (GUILayout.Button("Build"))
        {
            BuildAddressable.Build();
        }
    }
}
#endif

