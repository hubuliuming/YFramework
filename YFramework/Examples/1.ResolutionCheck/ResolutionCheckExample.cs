/****************************************************
    文件：ResolutionCheckExample.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/13 16:53:39
    功能：验证屏幕分辨率示例
*****************************************************/

using UnityEngine;

namespace YFramework
{
    public class ResolutionCheckExample
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem("YFramework/Examples/1.ResolutionCheckExample", false, 1)]
       private static void MenuClick()
        {
            Debug.Log(ResolutionCheck.IsPadResolution() ? "是Pad宽高比" : "不是Pad宽高比");
            Debug.Log(ResolutionCheck.IsPhoneResolution() ? "是手机宽高比" : "不是手机宽高比");
            Debug.Log(ResolutionCheck.GetAspectRatio());
        }
#endif
    }
}