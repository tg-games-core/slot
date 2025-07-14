using UnityEditor;

namespace Core.Editor.Tuner
{
    public class ReleaseSettingsTuner : SettingsTuner
    {
        public static readonly ReleaseSettings GooglePlaySettings =
            new(AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7);
        
        protected override string ApplyButtonName
        {
            get => "Setup Release Settings";
        }
        
        public override void Init() { }

        protected override void ApplySettings()
        {
            ApplyReleaseSettings();
        }

        private void ApplyReleaseSettings()
        {
            UnityEditor.PlayerSettings.Android.targetArchitectures = GooglePlaySettings.Architecture;
        }
    }
}