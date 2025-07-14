using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using VContainer;
using VContainer.Unity;

namespace Core
{
    public class ShadowOptimizationSystem : IInitializable, ITickable
    {
        private const int TargetFps = 60;
        private const int MinAvailableFpsToTarget = 40;
        private const float AverageFPSTime = 5f;
        private const float TimeCheckShadowOff = 30f;
        private const float TimeCheckShadowOn = 15f;

        private bool _isShadowEnabled = true;
        
        private int _framesSinceLastUpdate;
        
        private float _fpsAccumulator;
        private float _fpsNextUpdate;
        
        //private UniversalRenderPipelineAsset _universalRenderPipelineAsset;
        
        private ShadowSettings _shadowSettings;

        private float ShadowDepthBias
        {
            get => _isShadowEnabled ? _shadowSettings.ShadowDepthBias : 0f;
        }
        
        private float ShadowDistance
        {
            get => _isShadowEnabled ? _shadowSettings.ShadowDistance : 0f;
        }

        [Inject]
        private void Construct(ShadowSettings shadowSettings)
        {
            _shadowSettings = shadowSettings;
        }
        
        void IInitializable.Initialize()
        {
            //_universalRenderPipelineAsset = (UniversalRenderPipelineAsset)GraphicsSettings.renderPipelineAsset;

            //_universalRenderPipelineAsset.shadowDepthBias = ShadowDepthBias;
            //_universalRenderPipelineAsset.shadowDistance = ShadowDistance;
            
            Application.targetFrameRate = TargetFps;
            _fpsNextUpdate = Time.time + AverageFPSTime; 
        }

        void ITickable.Tick()
        {
            _framesSinceLastUpdate++;
            _fpsAccumulator += Time.deltaTime;

            if (Time.time > _fpsNextUpdate)
            {
                var averageFPS = Mathf.RoundToInt(_framesSinceLastUpdate / _fpsAccumulator);

                _framesSinceLastUpdate = 0;
                _fpsAccumulator = 0f;
                _fpsNextUpdate = Time.time + AverageFPSTime;
                
                if (averageFPS >= MinAvailableFpsToTarget)
                {
                    ToggleShadow(true);
                }
                else
                {
                    ToggleShadow(false);
                }
            }
        }
        
        private void ToggleShadow(bool isEnabled)
        {
            if (_isShadowEnabled != isEnabled)
            {
                _isShadowEnabled = isEnabled;

                QualitySettings.shadows = _isShadowEnabled ? ShadowQuality.HardOnly : ShadowQuality.Disable;
                
                ToggleShadowAsync(ShadowDepthBias, ShadowDistance);
            }
        }

        private void ToggleShadowAsync(float shadowDepthBias, float shadowDistance)
        {
            // float startShadowDepthBias = _universalRenderPipelineAsset.shadowDepthBias;
            // float startShadowDistance = _universalRenderPipelineAsset.shadowDistance;
            //
            // UniTaskExtensions.Lerp(progress =>
            // {
            //     _universalRenderPipelineAsset.shadowDepthBias =
            //         Mathf.Lerp(startShadowDepthBias, shadowDepthBias, progress);
            //     _universalRenderPipelineAsset.shadowDistance =
            //         Mathf.Lerp(startShadowDistance, shadowDistance, progress);
            // }, 2.5f, AnimationCurve.Linear(0, 0, 1, 1)).Forget();
            //
            // _fpsNextUpdate = Time.time + (_isShadowEnabled ? TimeCheckShadowOn : TimeCheckShadowOff);
        }
    }
}