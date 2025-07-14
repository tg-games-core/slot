using UnityEditor;
using UnityEngine;

namespace Core.Editor.Tuner.MVC
{
    public class View
    {
        public Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++) pix[i] = col;
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        public GUIContent GetStatusContent(bool value)
        {
            return value ? EditorGUIUtility.IconContent("TestPassed") : EditorGUIUtility.IconContent("TestFailed");
        }
    }
}