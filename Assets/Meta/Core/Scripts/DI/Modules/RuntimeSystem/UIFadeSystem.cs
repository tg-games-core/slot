using System;
using Core.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public class UIFadeSystem : RuntimeComponent<UIFadeSystem>
    {
        [SerializeField, Header("Visual")]
        private CanvasGroup _canvasGroup;
        
        [SerializeField]
        private SlicedFilledImage _loadingProgress;

        [SerializeField, Header("Settings")]
        private float _inTime;

        [SerializeField]
        private AnimationCurve _inCurve;

        [SerializeField]
        private float _outTime;

        [SerializeField]
        private AnimationCurve _outCurve;

        [SerializeField, Header("Loading")]
        private float _minSceneLoadingTime;

        public override void Initialize()
        {
            base.Initialize();
            
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }

        public void Show(Func<UniTask> loadCallback)
        {
            _loadingProgress.fillAmount = 0f;
            _canvasGroup.blocksRaycasts = true;

            UniTaskExtensions.Lerp(progress =>
            {
                _canvasGroup.alpha = Mathf.Lerp(0f, 1f, progress);
            }, _inTime, _inCurve, callback: () => { LoadAsync(loadCallback).Forget(); }).Forget();
        }

        private async UniTaskVoid LoadAsync(Func<UniTask> callback)
        {
            var waiter = callback.Invoke();
            float time = 0f;
            
            while (waiter.Status == UniTaskStatus.Pending || time < _minSceneLoadingTime)
            {
                _loadingProgress.fillAmount = time / _minSceneLoadingTime * 0.8f;
                
                await UniTask.NextFrame();
                
                time += Time.deltaTime;
            }

            _loadingProgress.fillAmount = 1f;

            Hide();
        }

        private async void Hide()
        {
            UniTaskExtensions.Lerp(progress =>
            {
                _canvasGroup.alpha = Mathf.Lerp(1f, 0f, progress);
            }, _outTime, _outCurve, callback: () => { _canvasGroup.blocksRaycasts = false; }).Forget();
        }
    }
}