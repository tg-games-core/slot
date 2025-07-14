using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(AudioSource))]
    public class PooledAudio : PooledBehaviour
    {
        [SerializeField]
        private AudioSource _source;

        public AudioSource Source
        {
            get => _source;
        }
        
        protected override void BeforeReturnToPool()
        {
            base.BeforeReturnToPool();
            ResetState();
        }
        
        public void Configure(AudioClip clip, float pitch, bool isLooped, float volume = 1f)
        {
            _source.clip = clip;
            _source.pitch = pitch;
            _source.loop = isLooped;
            _source.volume = volume;
            
            FreeTimeout = isLooped ? 0f : clip.length;
            _source.Play();
        }
        
        private void ResetState()
        {
            _source.Stop();
            _source.clip = null;
            FreeTimeout = 0f;
        }
    }
}