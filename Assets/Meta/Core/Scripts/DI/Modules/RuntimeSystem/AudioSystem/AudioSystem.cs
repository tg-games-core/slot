using UnityEngine;
using VContainer;

namespace Core
{
    public class AudioSystem : RuntimeComponent<AudioSystem>, IAudioSystem
    {
        [SerializeField]
        private AudioSource _musicSource;

        [SerializeField]
        private AudioSource _soundSource;

        private AudioSettings _audioSettings;
        private IPoolSystem _poolSystem;
        private PoolSettings _poolSettings;
        private IAudioSystem _audioSystem;

        bool IAudioSystem.IsSoundEnabled
        {
            get => LocalConfig.IsSoundEnabled;
        }
        
        bool IAudioSystem.IsMusicEnabled
        {
            get => LocalConfig.IsMusicEnabled;
        }

        [Inject]
        public void Construct(IPoolSystem poolSystem, AudioSettings audioSettings, PoolSettings poolSettings)
        {
            _poolSettings = poolSettings;
            _poolSystem = poolSystem;
            _audioSettings = audioSettings;
        }

        public override void Initialize()
        {
            base.Initialize();

            _audioSystem = this;
        }

        void IAudioSystem.ToggleSound(bool isEnable)
        {
            LocalConfig.IsSoundEnabled = isEnable;
        }

        void IAudioSystem.ToggleMusic(bool isEnable)
        {
            LocalConfig.IsMusicEnabled = isEnable;
        }
        
        void IAudioSystem.PlayMusic(SoundType type)
        {
            if (!_audioSystem.IsMusicEnabled)
            {
                return;
            }

            var clip = _audioSettings.GetClip(type);
            if (clip == null)
            {
                return;
            }

            if (_musicSource.isPlaying)
            {
                _musicSource.Stop();
            }

            _musicSource.clip = clip;
            _musicSource.Play();
        }

        void IAudioSystem.StopMusic()
        {
            if (_musicSource)
            {
                _musicSource.Stop();
            }
        }

        AudioHandle IAudioSystem.Play2D(SoundType type)
        {
            if (!_audioSystem.IsSoundEnabled)
            {
                return default;
            }

            var clip = _audioSettings.GetClip(type);
            if (clip == null)
            {
                return default;
            }

            _soundSource.PlayOneShot(clip);
            return new AudioHandle(null, clip.length);
        }

        void IAudioSystem.Stop2D()
        {
            _soundSource.Stop();
        }
        
        AudioHandle IAudioSystem.Play3D(SoundType type, Vector3 position)
        {
            return _audioSystem.Play3D(type, position, AudioParams.Default);
        }

        AudioHandle IAudioSystem.Play3D(SoundType type, Vector3 position, AudioParams parameters)
        {
            if (!_audioSystem.IsSoundEnabled)
            {
                return default;
            }

            var clip = _audioSettings.GetClip(type);
            if (clip == null)
            {
                return default;
            }

            var audio = _poolSystem.Get<PooledAudio>(_poolSettings.PooledAudio, position, Quaternion.identity);

            audio.Configure(clip, parameters.Pitch, parameters.IsLooped, parameters.Volume);
            return new AudioHandle(audio, clip.length);
        }
    }
}