using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Editor.Helpers;
using Core.Editor.Tuner.MVC;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.Tuner
{
    public class CustomProjectTuner : SettingsTuner
    {
        private readonly Dictionary<int, PublisherType> _targetPublisherTypes = new();
        private readonly Dictionary<PublisherType, string[]> _manifestRequiredAssets =
            new()
            {
                { PublisherType.None, null },
            };
        
        private bool _isInAppEnabled;
        private bool _isDebugEnabled;

        private string _companyName = string.Empty;
        private string _productName = string.Empty;
        private string _packageName = string.Empty;

        private int _selectedPublisher;

        private View _view;

        protected override string ApplyButtonName
        {
            get => "Apply Settings";
        }

        public override void Init()
        {
            _companyName = UnityEditor.PlayerSettings.companyName;
            _productName = UnityEditor.PlayerSettings.productName;
            _packageName = Application.identifier;
            
            var publishers = (PublisherType[])Enum.GetValues(typeof(PublisherType));
            
            _targetPublisherTypes.Clear();
            for (int i = 0; i < publishers.Length; i++)
            {
                _targetPublisherTypes.Add(i, publishers[i]);
            }

            _isInAppEnabled = DefineHelper.IsDefineEnabled(CustomProjectTunerSettings.InAppEnableDefine);
            _isDebugEnabled = DefineHelper.IsDefineEnabled(CustomProjectTunerSettings.DebugMenuDefine);
            
            DetectTargetPublisher(DefineHelper.GetPlatformDefines());

            _view = new View();
        }

        public override void DrawSettings()
        {
            _companyName = EditorGUILayout.TextField("Company Name:", _companyName);
            _productName = EditorGUILayout.TextField("Product Name:", _productName);
            _packageName = EditorGUILayout.TextField("Package Name:", _packageName);
            
            _selectedPublisher = EditorGUILayout.Popup("Target Publisher", _selectedPublisher,
                _targetPublisherTypes.Values.Select(s => s.ToString()).ToArray());

            _isInAppEnabled = EditorGUILayout.Toggle("In App Enabled: ", _isInAppEnabled);
            
            DrawDebugSettings();

            base.DrawSettings();
        }

        private void DrawDebugSettings()
        {
            EditorGUILayout.Space(10);
            
            GUIStyle groupStyle = new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(10, 10, 0, 10),
                margin = new RectOffset(5, 5, 5, 5),
                normal = { background = _view?.MakeTex(2, 2, new Color(0.1f, 0.1f, 0.2f, 0.3f)) }
            };
            EditorGUILayout.BeginVertical(groupStyle);
            {
                EditorGUILayout.LabelField("Debug Settings", EditorStyles.boldLabel);
                EditorGUILayout.Space(5);

                _isDebugEnabled =
                    EditorGUILayout.Toggle(
                        new GUIContent(" Debug Enabled:", _view?.GetStatusContent(_isDebugEnabled).image),
                        _isDebugEnabled);

                if (_isDebugEnabled)
                {
                    EditorGUILayout.HelpBox("Should be disabled before build!", MessageType.Warning);
                }
            }
            EditorGUILayout.EndVertical();
        }

        protected override void ApplySettings()
        {
            UnityEditor.PlayerSettings.companyName = _companyName;
            UnityEditor.PlayerSettings.productName = _productName;
            
            UnityEditor.PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, _packageName);
            UnityEditor.PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, _packageName);

            var targetStore = _targetPublisherTypes[_selectedPublisher];
            
            SetupDefineSymbols(targetStore);
            VerifyManifest(targetStore);
        }
        
        private void DetectTargetPublisher(string[] defines)
        {
            if (defines != null && defines.Length > 0)
            {
                var publisherTypes = (PublisherType[])Enum.GetValues(typeof(PublisherType));

                for (int i = 0; i < publisherTypes.Length; i++)
                {
                    if (defines.Contains(PublisherHelper.GetDefineByPublisher(publisherTypes[i])))
                    {
                        _selectedPublisher = i;

                        break;
                    }
                }
            }
        }

        private void SetupDefineSymbols(PublisherType publisher)
        {
            var defines = DefineHelper.GetPlatformDefines().ToList();
            
            SetupPublisherDefine(ref defines, publisher);
            SetupDefine(ref defines, _isInAppEnabled, CustomProjectTunerSettings.InAppEnableDefine);
            SetupDefine(ref defines, _isDebugEnabled, CustomProjectTunerSettings.DebugMenuDefine);

            UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android,
                defines.ToArray());
            UnityEditor.PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS,
                defines.ToArray());
        }

        private void SetupPublisherDefine(ref List<string> defines, PublisherType publisher)
        {
            var publisherDefines = PublisherHelper.GetPublisherDefines().ToList();

            foreach (var define in publisherDefines)
            {
                if (defines.Contains(define))
                {
                    defines.Remove(define);
                }
            }

            defines.Add(PublisherHelper.GetDefineByPublisher(publisher));
        }

        private void SetupDefine(ref List<string> defines, bool isEnabled, string define)
        {
            if (isEnabled)
            {
                if (!DefineHelper.IsDefineEnabled(define))
                {
                    defines.Add(define);
                }
            }
            else
            {
                defines.Remove(define);
            }
        }

        private void VerifyManifest(PublisherType publisher)
        {
            if (_manifestRequiredAssets.TryGetValue(publisher, out var requiredAssets))
            {
                if (requiredAssets != null && requiredAssets.Length > 0)
                {
                    var cachedRequiredAssets = requiredAssets.ToList();

                    int removeIndex = Application.dataPath.IndexOf("Assets", StringComparison.Ordinal);
                    var root = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
                    var path = Path.Combine(root, "Packages", "manifest.json");

                    if (File.Exists(path))
                    {
                        using (StreamReader sr = File.OpenText(path))
                        {
                            string s = string.Empty;

                            while ((s = sr.ReadLine()) != null)
                            {
                                for (int i = 0; i < requiredAssets.Length; i++)
                                {
                                    if (s.Contains(requiredAssets[i]))
                                    {
                                        cachedRequiredAssets.Remove(requiredAssets[i]);
                                    }
                                }
                            }

                            if (cachedRequiredAssets.Count > 0)
                            {
                                UnityEngine.Debug.LogException(new Exception($"Not added assets into manifest.json: "));

                                for (int i = 0; i < cachedRequiredAssets.Count; i++)
                                {
                                    UnityEngine.Debug.LogException(new Exception($"{cachedRequiredAssets[i]}"));
                                }
                            }
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.LogException(new Exception($"Not found manifest.json on path - {path}"));
                    }
                }
            }
        }
    }
}