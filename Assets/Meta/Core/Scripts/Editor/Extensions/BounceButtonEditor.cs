using Core.UI;
using UnityEditor;

namespace Core.Editor
{
    [CustomEditor(typeof(BounceButton))]
    public class BounceButtonEditor : UnityEditor.UI.ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            SerializedObject so = new SerializedObject(target);
            
            SerializedProperty stringsProperty = so.FindProperty("_scaleReference");
            EditorGUILayout.PropertyField(stringsProperty, true);
            
            stringsProperty = so.FindProperty("_scaleObjects");
            EditorGUILayout.PropertyField(stringsProperty, true);
            
            stringsProperty = so.FindProperty("_releaseCurve");
            EditorGUILayout.PropertyField(stringsProperty, true);
            
            so.ApplyModifiedProperties();
        }
    }
}