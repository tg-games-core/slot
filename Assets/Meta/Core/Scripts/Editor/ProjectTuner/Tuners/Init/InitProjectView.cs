using Core.Editor.Tuner.Drawer;
using Core.Editor.Tuner.MVC;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.Tuner
{
    public class InitProjectView : View
    {
        public InitialDrawer Drawer
        {
            get;
            private set;
        }

        public void ChangePlatform(BuildTargetGroup targetGroup)
        {
            switch (targetGroup)
            {
                case BuildTargetGroup.WebGL:
                    Drawer = new WebDrawer();
                    break;
                
                default:
                    Drawer = null;
                    break;
            }
        }

        public void Draw()
        {
            Drawer?.Draw();
        }
        
        public GUIStyle GetColoredStyle(BuildTargetGroup target)
        {
            var style = new GUIStyle(EditorStyles.boldLabel)
            {
                normal = { textColor = GetTargetColor(target) }
            };
            return style;
        }

        private Color GetTargetColor(BuildTargetGroup target)
        {
            return target switch
            {
                BuildTargetGroup.Android => new Color(0.2f, 0.8f, 0.3f),
                BuildTargetGroup.iOS => new Color(0.4f, 0.5f, 1f),
                BuildTargetGroup.WebGL => new Color(1f, 0.6f, 0.1f),
                _ => EditorStyles.label.normal.textColor
            };
        }

        public string GetPlatformIconName(BuildTargetGroup target)
        {
            return target switch
            {
                BuildTargetGroup.Android => "BuildSettings.Android.Small",
                BuildTargetGroup.iOS => "BuildSettings.iPhone.Small",
                BuildTargetGroup.WebGL => "BuildSettings.WebGL.Small",
                _ => "BuildSettings.Standalone.Small"
            };
        }

        public Texture2D GetPlatformIcon(BuildTargetGroup target)
        {
            return EditorGUIUtility.IconContent(GetPlatformIconName(target)).image as Texture2D;
        }

        public GUIStyle GetHighlightBoxStyle(Color color)
        {
            GUIStyle style = new GUIStyle(EditorStyles.helpBox)
            {
                normal =
                {
                    textColor = Color.black, background = MakeTex(2, 2, color)
                },
                alignment = TextAnchor.MiddleCenter
            };
            
            return style;
        }

        private Texture2D MakeColorTexture(Color color)
        {
            var tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, color);
            tex.Apply();
            return tex;
        }
    }
}