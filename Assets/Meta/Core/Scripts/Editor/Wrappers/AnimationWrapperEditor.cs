using Core.Wrappers.Animations;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.Wrappers
{
    [CustomEditor(typeof(AnimationWrapper), true)]
    public class AnimationWrapperEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI() 
        {
            base.OnInspectorGUI();
        
            AnimationWrapper wrapper = (AnimationWrapper)target;
        
            if (GUILayout.Button("Play Animation")) 
            {
                wrapper.Play();
            }
        
            if (GUILayout.Button("Stop Animation")) 
            {
                wrapper.Stop();
            }
        }
    }
}