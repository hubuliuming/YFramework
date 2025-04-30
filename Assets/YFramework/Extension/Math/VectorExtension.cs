using UnityEngine;

namespace YFramework.Extension.Math
{
    public static class VectorExtension
    {
        public static Vector2 NormalizeFor1(this Vector2 vector)
        {
            if (vector.x > 0) vector.x = 1;
            else if (vector.x < 0) vector.x = -1;
            if (vector.y > 0) vector.y = 1;
            else if (vector.y < 0) vector.y = -1;
            return vector;
        }
    }
}