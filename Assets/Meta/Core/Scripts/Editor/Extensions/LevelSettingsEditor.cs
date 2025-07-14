using UnityEditor;
using UnityEngine;

namespace Core.Editor
{
    [CustomEditor(typeof(LevelSettings))]
    public class LevelSettingsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var script = (LevelSettings)target;
            if (GUILayout.Button("Validate Scenes", GUILayout.Height(40)))
            {
                script.Validate();
            }
        }
    }
}