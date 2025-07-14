using UnityEngine;

namespace Core
{
    public static class BezierHelper
    {
        public static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1f - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 p = uu * p0;
            p += 2f * u * t * p1;
            p += tt * p2;

            return p;
        }
    }
}