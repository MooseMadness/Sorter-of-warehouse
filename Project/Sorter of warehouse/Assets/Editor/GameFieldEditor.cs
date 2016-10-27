using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameFieldScript))]
public class GameFieldEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GameFieldScript script = (GameFieldScript)target;
        if(GUILayout.Button("Создать игровое поле"))
        {
            script.BuildGameField();
        }
    }
}
