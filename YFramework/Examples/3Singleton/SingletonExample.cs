/****************************************************
    文件：SingletonExample.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/18 17:40:53
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace YFramework
{
    public class SingletonExample : Singleton<SingletonExample>
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem("YFramework/Examples/3.SingletonExample", false, 3)]
        private static void MenuClick()
        {
            var initInstance = Instance; 
            initInstance = Instance;//打印一次就是代表唯一
        }
#endif
        private SingletonExample()
        {
            Debug.Log("singletonExample ctor");
        
        }
    }
}