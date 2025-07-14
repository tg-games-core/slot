using UnityEngine;

namespace Core.Wrappers.Animations
{
    public class LegacyAnimationWrapper : AnimationWrapper
    {
        [SerializeField]
        private Animation _animation;

        private AnimationClip AnimationClip
        {
            get => _animation.clip;
        }
        
        public override float Duration
        {
            get => _animation?.GetClip(AnimationClip.name).length ?? 0f;
        }

        public override void Play()
        {
            _animation.Play(AnimationClip.name);
        }

        public override void Stop()
        {
            _animation.Stop(AnimationClip.name);
        }
    }
}