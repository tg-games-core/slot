using Core.Services;
using UnityEditor;
using UnityEngine;

namespace Core.Editor
{
    [CustomEditor(typeof(InAppSettings))]
    public class InAppSettingsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var script = (InAppSettings)target;
            if (GUILayout.Button("Generate JSON", GUILayout.Height(40)))
            {
                script.ConvertConfigToJson();
            }
            
            if (GUILayout.Button("Convert JSON To Config", GUILayout.Height(40)))
            {
                script.ConvertJsonToConfig(script.FormattedJsonData);
            }
        }
    }
}