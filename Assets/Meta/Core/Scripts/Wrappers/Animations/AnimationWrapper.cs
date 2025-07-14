using UnityEngine;

namespace Core.Wrappers.Animations
{
    public abstract class AnimationWrapper : MonoBehaviour, IAnimation
    {
        public abstract float Duration { get; }

        public abstract void Play();
        public abstract void Stop();
    }
}