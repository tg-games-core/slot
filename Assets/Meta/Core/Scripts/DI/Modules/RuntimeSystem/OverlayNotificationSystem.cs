using System;
using System.Threading;
using Core.Wrappers.Animations;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class OverlayNotificationSystem : RuntimeComponent<OverlayNotificationSystem>
    {
        private const int ScreenOffset = 80;

        [SerializeField]
        private TextMeshProUGUI _messageLabel;
        [SerializeField]
        private RectTransform _labelRect;
        [SerializeField]
        private LayoutElement _element;

        [SerializeField, Header("Animations")]
        private AnimationWrapper _appearAnimation;

        [SerializeField]
        private float _delay;

        [SerializeField]
        private AnimationWrapper _hideAnimation;

        private RectTransform _transform;
        
        private CancellationTokenSource _token;

        protected override void Awake()
        {
            base.Awake();

            _transform = (RectTransform)transform;
        }

        public override void Initialize()
        {
            base.Initialize();

            Hide();
        }

        public void Show(string text)
        {
            if (gameObject.activeSelf)
            {
                return;
            }
            
            _messageLabel.text = text;

            gameObject.SetActive(true);

            var token = UniTaskUtil.RefreshToken(ref _token);
            PlaySequence(_appearAnimation,
                () =>
                {
                    UniTaskExtensions.InvokeWithDelay(_delay, () =>
                    {
                        PlaySequence(_hideAnimation, Hide, token);
                    }, token).Forget();
                }, token);
            
            RebuildAsync().Forget();
        }

        private void PlaySequence(AnimationWrapper animationWrapper, Action callback, CancellationToken token)
        {
            animationWrapper?.Play();
            
            UniTaskExtensions.InvokeWithDelay(animationWrapper?.Duration ?? 0, callback, token).Forget();
        }
        
        private void Hide()
        {
            gameObject.SetActive(false);
        }
        
        private async UniTaskVoid RebuildAsync()
        {
            await UniTask.Yield();

            float screenWidth = _transform.rect.width - ScreenOffset;

            if (_labelRect.rect.width > screenWidth)
            {
                _element.enabled = true;
                _element.preferredWidth = screenWidth;
            }
        }
    }
}