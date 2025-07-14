using Core.UI;
using UnityEditor;

namespace Core.Editor
{
    [CustomEditor(typeof(StickButton))]
    public class StickButtonEditor : UnityEditor.UI.ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            SerializedObject so = new SerializedObject(target);
            
            SerializedProperty stringsProperty = so.FindProperty("_selectedGroup");
            EditorGUILayout.PropertyField(stringsProperty, true);
            
            so.ApplyModifiedProperties();
        }
    }
}