using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Core
{
    public class ParticlePoolSystem : RuntimeComponent<ParticlePoolSystem>, IParticleSystem
    {
        private readonly Dictionary<ParticleType, PooledParticle> _particles =
            new Dictionary<ParticleType, PooledParticle>();

        private ParticleSettings _particleSettings;
        
        [Inject]
        private void Construct(ParticleSettings particleSettings)
        {
            _particleSettings = particleSettings;
        }
        
        public override void Initialize()
        {
            base.Initialize();

            for (int i = 0; i < _particleSettings.ParticlePresets.Length; i++)
            {
                var preset = _particleSettings.ParticlePresets[i];
                
                Prepare(preset.Particle, preset.Type);
            }
        }

        private void Prepare(PooledParticle pooledParticle, ParticleType fxType)
        {
            if (!_particles.ContainsKey(fxType))
            {
                _particles.Add(fxType, Instantiate(pooledParticle, Vector3.zero, Quaternion.identity, transform));
            }
            else
            {
                DebugSafe.LogException(new Exception($"{nameof(ParticleType)} already exist in dictionary"));
            }
        }

        void IParticleSystem.Emit(ParticleType fxType, Vector3 position, Quaternion rotation = default)
        {
            if (_particles.TryGetValue(fxType, out var particle))
            {
                particle.Emit(position, rotation);
            }
            else
            {
                DebugSafe.LogError($"Not found particle for {nameof(ParticleType)}: {fxType}");
            }
        }
    }
}