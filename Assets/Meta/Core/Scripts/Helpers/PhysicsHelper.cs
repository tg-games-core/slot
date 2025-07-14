using UnityEngine;

namespace Core
{
    public class PhysicsHelper
    {
        public static void DrawDebug(Vector3 worldPos, float radius, float seconds)
        {
            UnityEngine.Debug.DrawRay(worldPos, radius * Vector3.up, Color.red, seconds);
            UnityEngine.Debug.DrawRay(worldPos, radius * Vector3.down, Color.red, seconds);
            UnityEngine.Debug.DrawRay(worldPos, radius * Vector3.left, Color.red, seconds);
            UnityEngine.Debug.DrawRay(worldPos, radius * Vector3.right, Color.red, seconds);
            UnityEngine.Debug.DrawRay(worldPos, radius * Vector3.forward, Color.red, seconds);
            UnityEngine.Debug.DrawRay(worldPos, radius * Vector3.back, Color.red, seconds);
        }
    }
}