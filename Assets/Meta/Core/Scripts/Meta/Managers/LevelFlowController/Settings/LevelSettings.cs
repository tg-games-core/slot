using System;
using System.Linq;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "Settings/Core/LevelSettings", order = 0)]
    public class LevelSettings : ScriptableObject
    {
        [Serializable]
        public class FinishCoinPreset
        {
            [field: SerializeField]
            public Range<int> LevelIndex
            {
                get;
                private set;
            }

            [field: SerializeField]
            public int Coin
            {
                get;
                private set;
            }
        }
        
#if UNITY_EDITOR
        [SerializeField, Header("Test Group")]
        private bool _isTestSceneEnabled;

        [SerializeField, EnabledIf(nameof(_isTestSceneEnabled), true, EnabledIfAttribute.HideMode.Invisible)]
        private string _testSceneName;
#endif
        
        [SerializeField, Header("Main Group")]
        private string _tutorialSceneName;

        [SerializeField]
        private string[] _levels;
        
        [SerializeField]
        private string[] _loopedLevels;

        [field: SerializeField]
        public float ResultDelay
        {
            get;
            private set;
        }
        
        [field: SerializeField]
        public float FailDelay
        {
            get;
            private set;
        }

        public string GetScene(int index)
        {
#if UNITY_EDITOR
            if (_isTestSceneEnabled)
            {
                return _testSceneName;
            }
#endif
            int levelIndex = index;

            if (levelIndex == 0)
            {
                return _tutorialSceneName;
            }
            else
            {
                // NOTE: учитываем туториал
                levelIndex -= 1;
            }

            if (levelIndex < _levels.Length)
            {
                return _levels[levelIndex % _levels.Length];
            }
            else
            {
                levelIndex -= _levels.Length;

                return _loopedLevels[levelIndex % _loopedLevels.Length];
            }
        }

#if UNITY_EDITOR
        public void Validate()
        {
            string sceneFormat = "{0}.unity";
            int defaultSceneCount = 2;
            
            var scenesGUIDs = UnityEditor.AssetDatabase.FindAssets("t:Scene");
            var scenesPaths = scenesGUIDs.Select(UnityEditor.AssetDatabase.GUIDToAssetPath);

            System.Collections.Generic.List<string> scenesToAdd = new System.Collections.Generic.List<string>();
            System.Collections.Generic.List<UnityEditor.EditorBuildSettingsScene> buildScenes =
                new System.Collections.Generic.List<UnityEditor.EditorBuildSettingsScene>();

            for (int i = 0; i < defaultSceneCount; i++)
            {
                buildScenes.Add(UnityEditor.EditorBuildSettings.scenes[i]);
            }

            void process(string[] levels)
            {
                foreach (var level in levels)
                {
                    foreach (var scene in scenesPaths)
                    {
                        if (scene.EndsWith(string.Format(sceneFormat, level)) && !scenesToAdd.Contains(scene))
                        {
                            scenesToAdd.Add(scene);
                        }
                    }
                }
                
                for (int i = 0; i < scenesToAdd.Count; i++)
                {
                    if (buildScenes.Select(bs => bs.path).All(bs => !bs.Contains(scenesToAdd[i])))
                    {
                        buildScenes.Add(new UnityEditor.EditorBuildSettingsScene(scenesToAdd[i], true));
                    }
                    else
                    {
                        int index = -1;

                        for (int j = 0; j < buildScenes.Count; j++)
                        {
                            if (buildScenes[j].path.Equals(scenesToAdd[i]))
                            {
                                index = j;
                            
                                break;
                            }
                        }

                        if (index != -1)
                        {
                            var editorScene = buildScenes[index];

                            if (!buildScenes.Contains(editorScene))
                            {
                                buildScenes.Add(editorScene);
                            }
                        }
                    }
                }
            }
            
            process(new []{ _tutorialSceneName });
            process(_levels);
            process(_loopedLevels);
            UnityEditor.EditorBuildSettings.scenes = buildScenes.ToArray();
        }
#endif
    }
}