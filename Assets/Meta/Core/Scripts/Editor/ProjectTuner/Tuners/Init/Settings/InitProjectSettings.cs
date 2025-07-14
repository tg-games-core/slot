using System.Collections.Generic;
using Core.Editor.Tuner.Configs;
using UnityEditor;
using UnityEditor.Build;

namespace Core.Editor.Tuner
{
    public class InitProjectSettings
    {
        public const string DebugResourcesName = "Settings";

        public static readonly string[] PackagePaths =
        {
            "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask",
            "https://github.com/Cysharp/R3.git?path=src/R3.Unity/Assets/R3.Unity",
        };

        public static readonly string[] InitialScenes =
        {
            "Scenes/Startup",
            "Scenes/UICommon",
            "Scenes/EmptyScene"
        };

        public static readonly Dictionary<BuildTargetGroup, BuildTargetConfig> BuildConfigs = new()
        {
            {
                BuildTargetGroup.Android,
                new(BuildTarget.Android, Il2CppCodeGeneration.OptimizeSpeed, ManagedStrippingLevel.Minimal)
            },
            {
                BuildTargetGroup.iOS,
                new(BuildTarget.iOS, Il2CppCodeGeneration.OptimizeSpeed, ManagedStrippingLevel.Minimal)
            },
            {
                BuildTargetGroup.WebGL,
                new(BuildTarget.WebGL, Il2CppCodeGeneration.OptimizeSize, ManagedStrippingLevel.High)
            },
        };
    }
}