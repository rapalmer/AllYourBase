using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AutoVertSize))]
public class AutomaticVerticalSizeEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Calculate Size")) { ((AutoVertSize)target).AdjustSize(); }
    }
}
