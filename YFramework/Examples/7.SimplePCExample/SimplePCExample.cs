/****************************************************
    文件：SimplePCExample.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace YFramework
{
    public class SimplePCExample 
    {
        class Light
        {
            public void Open()
            {
                Debug.Log("灯打开了");
            }

            public void Close()
            {
                Debug.Log("灯关闭了");
            }
        }

        class Room : SimplePC
        {
            private Light _light = new Light();

            public void Enter()
            {
                if (RefCount == 0)
                {
                    _light.Open();
                }

                Retain();
                Debug.LogFormat("当前房间人数{0}", RefCount);
            }

            public void Leave()
            {
                Release();
                if (RefCount <= 0)
                {
                    _light.Close();
                }

                Debug.LogFormat("当前房间人数{0}", RefCount);
            }
            
        }
#if UNITY_EDITOR
        [UnityEditor.MenuItem("YFramework/Examples/7.SimplePCExample", false, 7)]
        private static void MenuClick()
        {
            var room = new Room();
            room.Enter();
            room.Enter();
            room.Enter();
            
            room.Leave();
            room.Leave();
            room.Leave();
            
            room.Enter();
        }
#endif
    }
}