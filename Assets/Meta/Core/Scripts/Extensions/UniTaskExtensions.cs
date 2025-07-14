using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public static class UniTaskExtensions
    {
        public static async UniTask Lerp(Action<float> action, float executionTime, AnimationCurve curve = null,
            CancellationToken token = default, Action callback = null,
            PlayerLoopTiming playerLoopTiming = PlayerLoopTiming.FixedUpdate)
        {
            float evaluate(float progress)
            {
                if (curve != null)
                {
                    progress = curve.Evaluate(progress);
                }

                return progress;
            }

            float time = 0f;
            float progress = 0f;

            while (time < executionTime)
            {
                await UniTask.Yield(playerLoopTiming, token);

                time += GetLoopUpdateTime(playerLoopTiming);
                progress = time / executionTime;
                progress = evaluate(progress);

                action(progress);
            }
            
            callback?.Invoke();
        }

        public static async UniTask MoveByPath(Transform transform, Vector3[] path, float movementSpeed,
            float rotationSpeed, CancellationToken token = default,
            PlayerLoopTiming playerLoopTiming = PlayerLoopTiming.FixedUpdate, bool isRevertedMovement = false, 
            Action callback = null)
        {
            for (int i = 0; i < path.Length; i++)
            {
                await MoveToPoint(transform, path[i], movementSpeed, rotationSpeed, token, playerLoopTiming,
                    isRevertedMovement);
            }
            
            callback?.Invoke();
        }

        public static async UniTaskVoid InvokeWithDelay(float delay, Action callback, CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);

            callback?.Invoke();
        }
        
        public static async UniTaskVoid InvokeWithDelay(bool condition, Action callback, CancellationToken token)
        {
            await UniTask.WaitUntil(() => condition, cancellationToken: token);

            callback?.Invoke();
        }
        
        public static async UniTask InvokeWithFrameDelay(int frameCount, Action callback,
            CancellationToken token = default)
        {
            await UniTask.DelayFrame(frameCount, cancellationToken: token);

            callback?.Invoke();
        }
        
        private static async UniTask MoveToPoint(Transform transform, Vector3 endPosition, float movementSpeed,
            float rotationSpeed, CancellationToken token = default,
            PlayerLoopTiming playerLoopTiming = PlayerLoopTiming.FixedUpdate, bool isRevertedMovement = false)
        {
            try
            {
                Vector3 startPosition = transform.position;

                if (startPosition == endPosition)
                {
                    return;
                }

                float dist = Vector3.Distance(startPosition, endPosition);
                float progress = 0;

                while (progress < 1)
                {
                    float delta = Time.deltaTime;

                    progress += delta * movementSpeed / dist;

                    Vector3 lookPosition = Vector3.zero;
                    
                    if (isRevertedMovement)
                    {
                        lookPosition = transform.position - endPosition;
                    }
                    else
                    {
                        lookPosition = endPosition - transform.position;
                    }

                    transform.rotation = Quaternion.Lerp(transform.rotation,
                        Quaternion.LookRotation(lookPosition), delta * rotationSpeed);

                    transform.position = Vector3.Lerp(startPosition, endPosition, progress);

                    await UniTask.Yield(playerLoopTiming, token);
                }

                transform.position = endPosition;
            }
            catch { }
        }

        private static float GetLoopUpdateTime(PlayerLoopTiming playerLoopTiming)
        {
            float deltaTime = 0f;
            
            switch (playerLoopTiming)
            {
                case PlayerLoopTiming.Update:
                    deltaTime = Time.deltaTime;
                    break;
                
                case PlayerLoopTiming.FixedUpdate:
                    deltaTime = Time.fixedDeltaTime;
                    break;
                
                default:
                    deltaTime = Time.deltaTime;
                    break;
            }

            return deltaTime;
        }
    }
}