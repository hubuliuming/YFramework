/****************************************************
    文件：ScrenWidth.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/10 16:15:51
    功能：判断屏幕宽高比
*****************************************************/

using UnityEngine;

namespace YFramework
{
    public class ResolutionCheck
    {
        public static float GetAspectRatio()
        {
            bool isLandscape = Screen.width > Screen.height;
            return isLandscape ? (float) Screen.width / Screen.height : (float) Screen.height / Screen.width;
        }
        public static bool IsPadResolution()
        {
            return InAspectRange(4.0f / 3);
        }
        public static bool IsPhoneResolution()
        {
            return InAspectRange(16.0f / 9);
        }
        //计算范围误差
        public static bool InAspectRange(float dstAspectRatio)
        {
            float aspect = GetAspectRatio();
            return aspect > dstAspectRatio - 0.05 && aspect < dstAspectRatio + 0.05;
        }
    }
}