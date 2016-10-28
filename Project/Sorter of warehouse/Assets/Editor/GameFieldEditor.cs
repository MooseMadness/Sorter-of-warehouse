using UnityEngine;
using UnityEditor;

//Класс реализующий итерфейс редактора для компонента GameFieldScript
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
