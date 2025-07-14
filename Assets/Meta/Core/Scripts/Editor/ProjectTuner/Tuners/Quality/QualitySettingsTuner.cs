using UnityEngine;

namespace Core.Editor.Tuner
{
    public class QualitySettingsTuner : SettingsTuner
    {
        public override void Init()
        {
            
        }

        protected override string ApplyButtonName
        {
            get => "Apply Settings";
        }

        public override void DrawSettings()
        {
            //QualitySettings.antiAliasing = 0;

            base.DrawSettings();
        }

        protected override void ApplySettings()
        {
            QualitySettings.skinWeights = SkinWeights.OneBone;
            QualitySettings.vSyncCount = 0;
        }
    }
}