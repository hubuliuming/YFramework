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
        
        /// <summary>
        /// 像面板上显示，180度为界限
        /// </summary>
        /// <param name="eulerAngles"></param>
        /// <returns></returns>
        public static Vector3 ConvertEulerAngles(this Vector3 eulerAngles)
        {
            eulerAngles.x %= 360;
            eulerAngles.y %= 360;
            eulerAngles.z %= 360;
            if (eulerAngles.x > 180) eulerAngles.x -= 360;
            if (eulerAngles.y > 180) eulerAngles.y -= 360;
            if (eulerAngles.z > 180) eulerAngles.z -= 360;
            return eulerAngles;
        }
    }
}