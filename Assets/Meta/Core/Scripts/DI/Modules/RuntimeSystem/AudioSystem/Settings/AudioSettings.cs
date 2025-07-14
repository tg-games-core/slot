using System.Linq;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "AudioSettings", menuName = "Settings/Core/AudioSettings", order = 0)]
    public class AudioSettings : ScriptableObject
    {
        [SerializeField]
        private SoundConfig[] _configs;
        
        public AudioClip GetClip(SoundType type)
        {
            var setup = _configs.FirstOrDefault(s => s.AudioType == type);

            if (setup != null)
            {
                return setup.Clip;
            }

            return null;
        }
    }
}