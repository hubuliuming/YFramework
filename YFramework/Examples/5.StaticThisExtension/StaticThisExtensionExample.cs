/****************************************************
    文件：StaticThisExtensionEample.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/20 15:52:4
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace YFramework
{
    public static class StaticThisExtensionExample
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem("YFramework/Examples/5.StaticthisExtensionExample", false, 5)]
        private static void MenuClick()
        {
            new object().Test();
            "string".Test();
        }       

        private static void Test(this object  selfObject)
        {
            Debug.Log(selfObject);
        }
#endif       
    }
}