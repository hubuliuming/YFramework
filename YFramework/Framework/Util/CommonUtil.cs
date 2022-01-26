/****************************************************
    文件：CommonUtil.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/13 15:20:1
    功能：公共工具类
*****************************************************/

using UnityEngine;

namespace YFramework
{
    public class CommonUtil : MonoBehaviour
    {
        public static void CopyStrBuffer(string context)
        {
            GUIUtility.systemCopyBuffer = context;
        }
    }
}