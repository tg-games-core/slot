using System;
using System.Threading;
using Core.Highlighting;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public static class HighlightExtension
    {
        public static async UniTaskVoid Highlight<T>(T config, HighlightComposer composer, Action callback,
            CancellationToken token) where T : HighlightEffectConfig
        {
            await PlayFillAnimation(0f, 1f, config.InDuration, config.InCurve, composer, token);
            await PlayFillAnimation(1f, 0f, config.OutDuration, config.OutCurve, composer, token);

            if (!token.IsCancellationRequested)
            {
                callback?.Invoke();
            }
        }

        private static async UniTask PlayFillAnimation(float from, float to, float duration, AnimationCurve curve,
            HighlightComposer composer, CancellationToken token)
        {
            await UniTaskExtensions.Lerp(p =>
            {
                float progress = Mathf.Lerp(from, to, p);
                composer.ApplyEffects(progress);
            }, duration, curve, token);
        }
    }
}