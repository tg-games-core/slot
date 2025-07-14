using System;
using System.Collections;
using UnityEngine;

namespace Core
{
    public static class GameObjectExtensions
    {
        public static Coroutine InvokeWithDelay(this MonoBehaviour self, float delay, Action action)
        {
            if (!self.enabled)
            {
                DebugSafe.LogException(new Exception("gameobject is disabled"));
                return null;
            }

            return self.StartCoroutine(InvokeWithDelayCor(self, delay, action));
        }

        public static Coroutine InvokeWithFrameDelay(this MonoBehaviour self, Action action)
        {
            if (!self.enabled)
            {
                DebugSafe.LogException(new Exception("gameobject is disabled"));
                return null;
            }

            return self.StartCoroutine(InvokeWithFrameDelayCor(self, action));
        }

        private static IEnumerator InvokeWithDelayCor(MonoBehaviour self, float delay, Action method)
        {
            yield return new WaitForSeconds(delay);
            try
            {
                method();
            }
            catch (Exception ex)
            {
                DebugSafe.LogError(
                    $"[InvokeWithDelay] - exception on action : name: {self?.gameObject?.name}, method: {method?.Method?.ToString()}, target: {method?.Target?.GetType().ToString()}; ex: {ex.Message}");

                throw;
            }
        }

        private static IEnumerator InvokeWithFrameDelayCor(MonoBehaviour self, Action action)
        {
            yield return null;

            try
            {
                action();
            }
            catch (Exception ex)
            {
                DebugSafe.LogError(
                    $"[InvokeWithDelay] - exception on action : name: {self?.gameObject?.name}, method: {action?.Method?.ToString()}, target: {action?.Target?.GetType().ToString()}; ex: {ex.Message}");

                throw;
            }
        }
    }
}