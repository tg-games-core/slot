using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Core.Editor.Tuner
{
    public class InitProjectTuner : SettingsTuner
    {
        private const BuildTargetGroup TargetPlatformGroup = BuildTargetGroup.WebGL;
        
        private bool _isDeveloperSettingsEnabled;
        private BuildTargetGroup _selectedBuildTarget;

        private InitProjectView _view;

        protected override string ApplyButtonName 
        { 
            get => "Init Project"; 
        }

        public override void Init()
        {
            BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
            _selectedBuildTarget = BuildPipeline.GetBuildTargetGroup(buildTarget);

            _view = new InitProjectView();
        }
        
        private void DrawTargetColumn(string label, BuildTargetGroup group, InitProjectView view)
        {
            var icon  = view?.GetPlatformIcon(group);
            var style = view?.GetColoredStyle(group);

            using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
            {
                GUILayout.Label(new GUIContent(icon), GUILayout.Width(20), GUILayout.Height(20));
                using (new EditorGUILayout.VerticalScope())
                {
                    EditorGUILayout.LabelField(label, group.ToString(), style);
                }
            }
        }
        
        public override void DrawSettings()
        {
            var activeTarget = EditorUserBuildSettings.activeBuildTarget;
            var activeTargetGroup = BuildPipeline.GetBuildTargetGroup(activeTarget);
            bool isDifferent = TargetPlatformGroup != activeTargetGroup;

            GUIStyle centeredStyle = new GUIStyle(GUI.skin.box)
            {
                alignment = TextAnchor.MiddleCenter,
                stretchHeight = true
            };

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                using (new EditorGUILayout.HorizontalScope(GUI.skin.box, GUILayout.Height(40)))
                {
                    using (new EditorGUILayout.VerticalScope(centeredStyle))
                    {
                        DrawTargetColumn("Target Build Target:", TargetPlatformGroup, _view);
                    }

                    using (new EditorGUILayout.VerticalScope(centeredStyle))
                    {
                        GUILayout.Box(GUIContent.none, GUILayout.Width(1), GUILayout.ExpandHeight(true));
                    }
                    
                    GUIStyle activeBoxStyle =
                        isDifferent ? _view.GetHighlightBoxStyle(new Color(1f, 0f, 0f, 0.5f)) : GUI.skin.box;
                    using (new EditorGUILayout.VerticalScope(activeBoxStyle, GUILayout.ExpandHeight(true)))
                    {
                        DrawTargetColumn("Active Build Target:", activeTargetGroup, _view);
                    }
                }

                if (isDifferent)
                {
                    EditorGUILayout.HelpBox("Should be the same platforms!\nClick on 'Init Project' button!",
                        MessageType.Error);
                }
            }

            HandleStateChanging(ref _isDeveloperSettingsEnabled,
                EditorGUILayout.Toggle(
                    new GUIContent(" Developer Settings", _view?.GetStatusContent(_isDeveloperSettingsEnabled).image),
                    _isDeveloperSettingsEnabled), () =>
                {
                    _view.ChangePlatform(_isDeveloperSettingsEnabled ? _selectedBuildTarget : activeTargetGroup);
                });

            if (_isDeveloperSettingsEnabled)
            {
                var targets = InitProjectSettings.BuildConfigs.Keys.ToArray();
                var labels = targets.Select(t => t.ToString()).ToArray();
                var currentIndex = System.Array.IndexOf(targets, _selectedBuildTarget);
                HandleStateChanging(ref _selectedBuildTarget,
                    targets[EditorGUILayout.Popup("Target Store", currentIndex, labels)], () =>
                    {
                        _view.ChangePlatform(_isDeveloperSettingsEnabled ? _selectedBuildTarget : activeTargetGroup);
                    });
                
                _view?.Draw();
            }
            
            base.DrawSettings();
        }
        
        private void HandleStateChanging<T>(ref T currentValue, T newValue, Action callback)
        {
            if (!EqualityComparer<T>.Default.Equals(currentValue, newValue))
            {
                currentValue = newValue;
                
                callback?.Invoke();
            }
        }

        protected override void ApplySettings()
        {
            var buildTargetGroup = _isDeveloperSettingsEnabled ? _selectedBuildTarget : TargetPlatformGroup;
            var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(buildTargetGroup);
            
            SwitchBuildTargetPlatform(buildTargetGroup);
            ApplyPlatformSettings(buildTargetGroup, namedBuildTarget);
            SetupPackageManager();
            SetupSpritePackerMode();
            SetupScriptingBackend(namedBuildTarget);
            SetupDebugMenu();
            SetupEditorBuildSettingsScene();
        }

        private void SwitchBuildTargetPlatform(BuildTargetGroup buildTargetGroup)
        {
            if (BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget) != buildTargetGroup)
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup,
                    InitProjectSettings.BuildConfigs[buildTargetGroup].BuildTarget);
            }
        }        
        
        private void ApplyPlatformSettings(BuildTargetGroup buildTargetGroup, NamedBuildTarget namedBuildTarget)
        {
            var config = InitProjectSettings.BuildConfigs[buildTargetGroup];
            
            PlayerSettings.SetIl2CppCodeGeneration(namedBuildTarget, config.CodeGeneration);
            PlayerSettings.SetManagedStrippingLevel(namedBuildTarget,
                _view.Drawer?.SelectedStrippingLevel ?? config.StrippingLevel);
        }

        private void SetupPackageManager()
        {
            foreach (var package in InitProjectSettings.PackagePaths)
            {
                Client.Add(package);
            }
        }

        private void SetupSpritePackerMode()
        {
            EditorSettings.spritePackerMode = SpritePackerMode.AlwaysOnAtlas;
        }

        private void SetupScriptingBackend(NamedBuildTarget namedBuildTarget)
        {
            UnityEditor.PlayerSettings.SetScriptingBackend(namedBuildTarget, ScriptingImplementation.IL2CPP);
        }

        private void SetupEditorBuildSettingsScene()
        {
            var scenesGUIDs = AssetDatabase.FindAssets("t:Scene");
            var scenesPaths = scenesGUIDs.Select(AssetDatabase.GUIDToAssetPath);

            List<string> foundedScenes = new List<string>();

            List<EditorBuildSettingsScene> buildScenes = EditorBuildSettings.scenes.ToList();
            foreach (var initialScene in InitProjectSettings.InitialScenes)
            {
                foreach (var scene in scenesPaths)
                {
                    if (scene.Contains(initialScene))
                    {
                        foundedScenes.Add(scene);
                    }
                }
            }

            for (int i = 0; i < foundedScenes.Count; i++)
            {
                if (buildScenes.Select(bs => bs.path).All(bs => !bs.Contains(foundedScenes[i])))
                {
                    buildScenes.Insert(i, new EditorBuildSettingsScene(foundedScenes[i], true));
                }
                else
                {
                    int index = -1;

                    for (int j = 0; j < buildScenes.Count; j++)
                    {
                        if (buildScenes[j].path.Equals(foundedScenes[i]))
                        {
                            index = j;
                            
                            break;
                        }
                    }

                    var editorScene = buildScenes[index];
                    
                    buildScenes.Remove(editorScene);
                    buildScenes.Insert(i, editorScene);
                }
            }
            
            foreach (var scenePath in foundedScenes)
            {
                if (buildScenes.Select(bs => bs.path).All(bs => !bs.Contains(scenePath)))
                {
                    buildScenes.Add(new EditorBuildSettingsScene(scenePath, true));
                }
            }
            
            EditorBuildSettings.scenes = buildScenes.ToArray();
        }

        private void SetupDebugMenu()
        {
            var settings = Resources.Load<SRDebugger.Settings>("SRDebugger/" + InitProjectSettings.DebugResourcesName);
            settings.IsEnabled = false;
        }
    }
}