using UnityEngine;

namespace Core
{
    public static class VectorExtensions
    {
        public static Vector3 ChangeX(this Vector3 origin, float value)
        {
            origin.x = value;
            return origin;
        }

        public static Vector3 ChangeY(this Vector3 origin, float value)
        {
            origin.y = value;
            return origin;
        }

        public static Vector3 ChangeZ(this Vector3 origin, float value)
        {
            origin.z = value;
            return origin;
        }

        public static Vector2 ChangeX(this Vector2 origin, float value)
        {
            origin.x = value;
            return origin;
        }

        public static Vector2 ChangeY(this Vector2 origin, float value)
        {
            origin.y = value;
            return origin;
        }
        
        public static float SqrDistance(Vector3 a, Vector3 b)
        {
            return (a - b).sqrMagnitude;
        }

        public static float SqrDistance(Transform transform, Transform another)
        {
            return SqrDistance(another.position, transform.position);
        }

        public static bool IsInSqrRange(Vector3 a, Vector3 b, float range)
        {
            return SqrDistance(a, b) <= range;
        }
        
        public static bool IsInSqrRange(Transform transform, Transform another, float range)
        {
            return SqrDistance(transform.position, another.position) <= range;
        }

        public static Vector3 Direction(Vector3 a, Vector3 b, bool isNormalized = false)
        {
            var direction = b - a;

            if (isNormalized)
            {
                direction = direction.normalized;
            }

            return direction;
        }
        
        public static Vector3 Direction(Transform transform, Transform another, bool isNormalized = false)
        {
            return Direction(transform.position, another.position, isNormalized);
        }

        public static Vector2 ToVector2(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.y);
        }

        public static Vector3 ToVector3(this Vector2 vector)
        {
            return new Vector3(vector.x, vector.y, 0f);
        }
    }
}