using UnityEngine;

namespace Core
{
    public interface IParticleSystem
    {
        void Emit(ParticleType fxType, Vector3 position, Quaternion rotation = default);
    }
}