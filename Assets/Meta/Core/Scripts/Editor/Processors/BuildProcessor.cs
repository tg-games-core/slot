using System;
using Core.Editor.Helpers;
using Core.Editor.Tuner;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Core.Editor
{
    public class BuildProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder
        {
            get => 0;
        }
        
        public void OnPreprocessBuild(BuildReport report)
        {
            if (PublisherHelper.GetTargetCompany() != PublisherType.None)
            {
                if (DefineHelper.IsDefineEnabled(CustomProjectTunerSettings.DebugMenuDefine))
                {
                    UnityEngine.Debug.LogException(
                        new Exception($"Debug Menu Enabled! Go to Release Project Tunner Tab and click 'Setup'"));
                }

                if (BuildHelper.GetBuildTargetGroup() == BuildTargetGroup.Android)
                {
                    if (UnityEditor.PlayerSettings.Android.targetArchitectures !=
                        ReleaseSettingsTuner.GooglePlaySettings.Architecture)
                    {
                        UnityEngine.Debug.LogException(new Exception(
                            $"Incorrect Target Architectures! Go to Release Project Tunner Tab and click 'Setup'"));
                    }
                }
            }
        }
    }
}