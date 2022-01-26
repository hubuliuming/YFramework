/****************************************************
    文件：MonoSingletonExample.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/19 10:15:49
    功能：Nothing
*****************************************************/

using UnityEditor;
using UnityEngine;

namespace YFramework
{
    public class MonoSingletonExample : MonoSingleton<MonoSingletonExample>
    {
#if UNITY_EDITOR
        [MenuItem("YFramework/Examples/4.MonoSingletonExample", false, 4)]
        private static void MenuClick()
        {
            EditorApplication.isPlaying = true; 
        }
#endif
        //当游戏运行时，该方法运行自动运行，因为当IsPlaying执行时候需要花上一定时间执行Editor，还没打开就会执行该方法里的Instance。发现还没完全运行就会报错
        [RuntimeInitializeOnLoadMethod]
        static void Example()
        { 
            var initInstance = Instance;
            initInstance = Instance;
        }
    }
}