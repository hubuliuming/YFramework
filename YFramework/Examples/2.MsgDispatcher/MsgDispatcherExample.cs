/****************************************************
    文件：MsgDispatcherExample.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/13 16:59:38
    功能：简单消息机制示例
*****************************************************/

using UnityEditor;
using UnityEngine;

namespace YFramework
{
    public class MsgDispatcherExample : YMonoBehaviour
    {
#if UNITY_EDITOR
        [MenuItem("YFramework/Examples/2.MsgDispatcherExample", false, 2)]
        private static void ClickMenu()
        {
            EditorApplication.isPlaying = true;
            new GameObject().AddComponent<MsgDispatcherExample>();
        }
#endif
        
        private void Start()
        {
            MsgRegister("1", data => Debug.Log(data));
            MsgRegister("1", data => Debug.Log("222"+data));
            MsgSend("1", "Hello");
            MsgSend("1", "Hello  two");
            UnMsgRegister("1");
            MsgSend("1", "Hello  three");
        }

        protected override void OnBeforeDestroy()
        {
            
        }
    }
}