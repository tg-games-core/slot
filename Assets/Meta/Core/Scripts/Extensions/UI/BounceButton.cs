using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.UI
{
    public class BounceButton : Button
    {
        public event Action Pressed;
        public event Action Released;
        
        [SerializeField]
        private float _scaleReference;

        [SerializeField]
        private RectTransform[] _scaleObjects;
        
        [SerializeField, Header("Animation Settings")]
        private AnimationCurve _releaseCurve = new AnimationCurve(
            new Keyframe(0, 0, 0, 0),
            new Keyframe(0.3f, 1.2f),
            new Keyframe(0.6f, 0.9f),
            new Keyframe(1, 1)
        );

        private bool _isPressed;
        
        private CancellationTokenSource _token;
        
        protected override void OnDisable()
        {
            base.OnDisable();
            
            if (_isPressed)
            {
                Release();
                _isPressed = false;
            }
            
            UniTaskUtil.CancelToken(ref _token);
        }
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            
            Press();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            
            Release();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            
            Release();
        }
        
        private void Press()
        {
            _isPressed = true;
            Pressed?.Invoke();
            
            PlayScaleAnimation(_scaleReference, 0.1f, null, UniTaskUtil.RefreshToken(ref _token)).Forget();
        }
        
        private void Release()
        {
            _isPressed = false;
            Released?.Invoke();
            
            PlayScaleAnimation(1f, 0.4f, _releaseCurve, UniTaskUtil.RefreshToken(ref _token)).Forget();
        }

        private async UniTaskVoid PlayScaleAnimation(float targetScale, float duration, AnimationCurve curve,
            CancellationToken token)
        {
            Vector3[] startScales = new Vector3[_scaleObjects.Length];
            for (int i = 0; i < _scaleObjects.Length; i++)
            {
                startScales[i] = _scaleObjects[i].localScale;
            }

            await UniTaskExtensions.Lerp(progress =>
            {
                for (int i = 0; i < _scaleObjects.Length; i++)
                {
                    _scaleObjects[i].localScale =
                        Vector3.LerpUnclamped(startScales[i], Vector3.one * targetScale, progress);
                }

            }, duration, curve, token, () =>
            {
                for (int i = 0; i < _scaleObjects.Length; i++)
                {
                    _scaleObjects[i].localScale = Vector3.one * targetScale;
                }
            });
        }
    }
}