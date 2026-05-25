using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IngredientStack))]
public class IngredientStackEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var modeProp = serializedObject.FindProperty("mode");
        EditorGUILayout.PropertyField(modeProp);

        bool isPushOnly = modeProp.enumValueIndex == (int)IngredientStack.StackMode.PushOnly;

        using (new EditorGUI.DisabledScope(isPushOnly))
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxSize"));
        }

        if (isPushOnly)
        {
            EditorGUILayout.HelpBox("Max Size is set automatically by ChefRat's Recipe Length.", MessageType.Info);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
