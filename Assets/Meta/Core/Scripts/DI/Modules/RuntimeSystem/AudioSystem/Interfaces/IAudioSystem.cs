using UnityEngine;

namespace Core
{
    public interface IAudioSystem
    {
        bool IsSoundEnabled { get; }
        bool IsMusicEnabled { get; }

        void ToggleSound(bool isEnable);
        void ToggleMusic(bool isEnable);

        void PlayMusic(SoundType type);
        void StopMusic();
        
        AudioHandle Play2D(SoundType type);
        void Stop2D();
        
        AudioHandle Play3D(SoundType type, Vector3 position);
        AudioHandle Play3D(SoundType type, Vector3 position, AudioParams parameters);
    }
}