using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ShellSpawner.SpawnTimer))]
public class SpawnTimerDrawer : PropertyDrawer
{
    private const float TimerWidth = 60f;
    private const float Spacing = 10f; // расстояние между Shell и Timer

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Timer прижат к правому краю
        Rect timerRect = new Rect(
            position.xMax - TimerWidth,
            position.y,
            TimerWidth,
            position.height
        );

        // Shell занимает всё оставшееся место слева с учётом отступа
        float shellWidth = position.width - TimerWidth - Spacing;
        Rect shellRect = new Rect(
            position.x,
            position.y,
            shellWidth,
            position.height
        );

        SerializedProperty shellProp = property.FindPropertyRelative(nameof(ShellSpawner.SpawnTimer.Shell));
        SerializedProperty timerProp = property.FindPropertyRelative(nameof(ShellSpawner.SpawnTimer.Timer));

        EditorGUI.PropertyField(shellRect, shellProp, GUIContent.none);
        EditorGUI.PropertyField(timerRect, timerProp, GUIContent.none);

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}
