using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Controller))]
public class ControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Controller controller = (Controller) target;
        if(GUILayout.Button("Clear"))
            controller.Clear();

        if(GUILayout.Button("Generate Grid"))
            controller.SpawnGrid();

        if(GUILayout.Button("Generate Pendant"))
            controller.SpawnPendant();
    }
}
