using System;
using System.Collections;
using UnityEngine;

namespace Core
{
    public class PooledParticle : PooledBehaviour
    {
        private const int InitialBurstArraySize = 10;
        
        private ParticleSystem[] _particleSystems;
        private ParticleSystem.Burst[] _burstBuffer;

        private void Awake()
        {
            InitializeParticleSystems();
            InitializeBurstBuffer();
            ValidateAndConfigureParticleSystems();
        }
        
        private void InitializeParticleSystems()
        {
            _particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
        }

        private void InitializeBurstBuffer()
        {
            _burstBuffer = new ParticleSystem.Burst[InitialBurstArraySize];
        }

        private void ValidateAndConfigureParticleSystems()
        {
            foreach (var ps in _particleSystems)
            {
                ConfigureParticleSystem(ps);
                ps.Stop();
            }
        }
        
        private void ConfigureParticleSystem(ParticleSystem ps)
        {
            var main = ps.main;
            ValidateAndSetScalingMode(main, ps);
            ValidateAndSetSimulationSpace(main, ps);
        }

        private void ValidateAndSetScalingMode(ParticleSystem.MainModule main, ParticleSystem ps)
        {
            if (main.scalingMode == ParticleSystemScalingMode.Hierarchy) 
                return;
                
            main.scalingMode = ParticleSystemScalingMode.Hierarchy;
            LogConfigurationError(nameof(ParticleSystemScalingMode.Hierarchy), ps, "scaled mode");
        }

        private void ValidateAndSetSimulationSpace(ParticleSystem.MainModule main, ParticleSystem ps)
        {
            if (main.simulationSpace == ParticleSystemSimulationSpace.World) 
                return;
                
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            LogConfigurationError(nameof(ParticleSystemSimulationSpace.World), ps, "simulation space");
        }

        public void Emit(Vector3 targetPosition, Quaternion rotation = default, int forceCount = 0)
        {
            foreach (var ps in _particleSystems)
            {
                if (forceCount > 0)
                {
                    EmitParticles(ps, targetPosition, rotation, forceCount);
                }
                else
                {
                    ProcessBurstEmissions(ps, targetPosition, rotation);
                }
            }
        }
        
        private void ProcessBurstEmissions(ParticleSystem ps, Vector3 targetPosition, Quaternion rotation)
        {
            var burstCount = ps.emission.GetBursts(_burstBuffer);
            for (int i = 0; i < burstCount; i++)
            {
                StartCoroutine(ExecuteBurstEmission(_burstBuffer[i], 
                    count => EmitParticles(ps, targetPosition, rotation, count)));
            }
        }

        private void EmitParticles(ParticleSystem ps, Vector3 targetPosition, Quaternion rotation, int count)
        {
            var emitParams = CreateEmitParams(ps, targetPosition, rotation);
            ps.Emit(emitParams, count);
        }

        private IEnumerator ExecuteBurstEmission(ParticleSystem.Burst burst, Action<int> emitAction)
        {
            yield return new WaitForSeconds(burst.time);

            var intervalWaiter = new WaitForSeconds(burst.repeatInterval);
            for (int i = 0; i < burst.cycleCount; i++)
            {
                emitAction(burst.maxCount);
                yield return intervalWaiter;
            }
        }

        private ParticleSystem.EmitParams CreateEmitParams(ParticleSystem ps, Vector3 targetPosition,
            Quaternion rotation)
        {
            var worldPosition = targetPosition - transform.position + ps.transform.position;
            var combinedRotation = rotation.eulerAngles + ps.transform.rotation.eulerAngles;

            return new ParticleSystem.EmitParams
            {
                position = GetPositionInSimulationSpace(ps, worldPosition),
                rotation3D = combinedRotation,
                applyShapeToPosition = true
            };
        }

        private Vector3 GetPositionInSimulationSpace(ParticleSystem ps, Vector3 worldPosition)
        {
            var main = ps.main;
            return main.simulationSpace switch
            {
                ParticleSystemSimulationSpace.World => worldPosition,
                ParticleSystemSimulationSpace.Custom => main.customSimulationSpace.InverseTransformPoint(worldPosition),
                _ => ps.transform.InverseTransformPoint(worldPosition)
            };
        }
        
        private void LogConfigurationError(string requiredValue, ParticleSystem ps, string settingName)
        {
            DebugSafe.LogError(
                $"[{nameof(ParticlePoolSystem)}] prefab:{gameObject.name}; " +
                $"particle system: {ps.gameObject.name}. " +
                $"Must be {settingName} {requiredValue}");
        }
        
        public new void SpawnFromPool()
        {
            IsFree = true;
        }

        public new void Free() { }
    }
}