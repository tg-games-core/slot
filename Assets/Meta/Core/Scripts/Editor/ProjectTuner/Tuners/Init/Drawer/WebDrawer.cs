using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;

namespace Core.Editor.Tuner.Drawer
{
    public class WebDrawer : InitialDrawer
    {
        private int _selectedStrippingLevel;

        private ManagedStrippingLevel[] _strippingLevels;

        public override ManagedStrippingLevel SelectedStrippingLevel
        {
            get => _strippingLevels[_selectedStrippingLevel];
        }

        protected override NamedBuildTarget NamedBuildTarget
        {
            get
            {
                var activeTarget = EditorUserBuildSettings.activeBuildTarget;
                var activeTargetGroup = BuildPipeline.GetBuildTargetGroup(activeTarget);
                return NamedBuildTarget.FromBuildTargetGroup(activeTargetGroup);
            }
        }

        protected override void Initialize()
        {
            _strippingLevels = (ManagedStrippingLevel[])Enum.GetValues(typeof(ManagedStrippingLevel));
            _selectedStrippingLevel = Array.IndexOf(_strippingLevels, CurrentStrippingLevel);
        }

        public override void Draw()
        {
            var labels = _strippingLevels.Select(t => t.ToString()).ToArray();
            _selectedStrippingLevel = EditorGUILayout.Popup("Managed Stripping Level", _selectedStrippingLevel, labels);
        }
    }
}