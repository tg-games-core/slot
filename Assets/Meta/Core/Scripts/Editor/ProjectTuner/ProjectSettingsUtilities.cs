using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Core.Editor.Tuner
{
    public class ProjectSettingsUtilities : EditorWindow
    {
        private const string ProjectSettingsWindowName = "Jungle Tavern";
        private const string SelectedTabKey = "ProjectTuner_SelectedTab";

        private static readonly Dictionary<int, ProjectTunerTabs> ProjectTunerTabPairs = new();
        private static readonly Dictionary<ProjectTunerTabs, SettingsTuner> SettingsTuners = new();
        
        private int _selectedTab;

        private void OnEnable()
        {
            Init();
        }

        private void OnDisable()
        {
            EditorPrefs.SetInt(SelectedTabKey, _selectedTab);
        }

        [MenuItem("Tools/Project Tuner")]
        private static void ShowProjectSettingsTuner()
        {
            ShowProjectTunerWindow();
        }

        private void Init()
        {
            SettingsTuners.Clear();
            ProjectTunerTabPairs.Clear();

            var tabTypes = (ProjectTunerTabs[])Enum.GetValues(typeof(ProjectTunerTabs));
            _selectedTab = EditorPrefs.GetInt(SelectedTabKey, 0);

            foreach (var tabType in tabTypes)
            {
                ProjectTunerTabPairs.Add((int)tabType, tabType);
            }
            
            var tabs = ProjectTunerTabPairs.Values.ToArray();

            foreach (var tab in tabs)
            {
                SettingsTuners.Add(tab, SetupSettingsTuner(tab));
            }
        }

        private static SettingsTuner SetupSettingsTuner(ProjectTunerTabs tab)
        {
            SettingsTuner settingsTuner = null;
            
            switch (tab)
            {
                case ProjectTunerTabs.InitProjectTuner:
                    settingsTuner = new InitProjectTuner();
                    break;
                
                case ProjectTunerTabs.CustomProjectTuner:
                    settingsTuner = new CustomProjectTuner();
                    break;
                
                case ProjectTunerTabs.QualitySettingsTuner:
                    settingsTuner = new QualitySettingsTuner();
                    break;
                
                case ProjectTunerTabs.ReleaseSettingsTuner:
                    settingsTuner = new ReleaseSettingsTuner();
                    break;
                
                default:
                    UnityEngine.Debug.LogError($"Not found {nameof(ProjectTunerTabs)} SettingsTuner for tab: {tab}");
                    break;
            }
            
            settingsTuner?.Init();
            
            return settingsTuner;
        }

        private static void ShowProjectTunerWindow()
        {
            var window = GetWindow(typeof(ProjectSettingsUtilities), true, ProjectSettingsWindowName, true);
            window.minSize = new Vector2(400, 400);
        }
        
        private void OnGUI()
        {
            var values = ProjectTunerTabPairs.Values.Select(t => SplitCamelCase(t.ToString())).ToArray();
            
            EditorGUILayout.BeginHorizontal();
            _selectedTab = GUILayout.Toolbar(_selectedTab, values);
            EditorGUILayout.EndHorizontal();
            
            if (ProjectTunerTabPairs.TryGetValue(_selectedTab, out var tab))
            {
                if (SettingsTuners.TryGetValue(tab, out var tuner))
                {
                    tuner.DrawSettings();
                }
            }
        }
        
        private string SplitCamelCase(string source) 
        {
            return string.Join(" ", Regex.Split(source, @"(?<!^)(?=[A-Z](?![A-Z]|$))"));
        }
    }
}