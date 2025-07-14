using UnityEditor;

namespace Core.Editor.Tuner
{
    public class ReleaseSettings
    {
        public AndroidArchitecture Architecture
        {
            get;
            private set;
        }

        public ReleaseSettings(AndroidArchitecture architecture)
        {
            Architecture = architecture;
        }
    }
}