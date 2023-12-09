/****************************************************
    文件：MsgDispater.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/12 10:44:4
    功能：最简单消息机制
*****************************************************/

using System;
using System.Collections.Generic;

namespace YFramework.Kit
{
    /// <summary>
    /// 消息机制，注意记得注销消息
    /// </summary>
    public class MsgDispatcher
    {
        private static Dictionary<string, Action<object>> mRegisteredDict = new Dictionary<string, Action<object>>();
        private static List<MsgRecorder> mRegisteredRecorders = new List<MsgRecorder>();

        public static void Send(string msgName, object data = null)
        {
            if (mRegisteredDict.ContainsKey(msgName))
            {
                mRegisteredDict[msgName](data);
            }
        }
        public static void Register(string msgName, Action<object> onReceived = null)
        {
            if (mRegisteredDict.ContainsKey(msgName) == false)
            {
                mRegisteredDict.Add(msgName, _ => { });
            }
            mRegisteredDict[msgName] = onReceived;
            mRegisteredRecorders.Add(MsgRecorder.Allocate(msgName,onReceived));
        }
        public static void UnRegister(string msgName)
        {
            var selectRecorder = mRegisteredRecorders.FindAll(recorder => recorder.name == msgName);
            selectRecorder.ForEach(recorder =>
            {
                mRegisteredDict[recorder.name] = null;
                mRegisteredDict.Remove(msgName);
                mRegisteredRecorders.Remove(recorder);
                recorder.Recycle();
            });
            selectRecorder.Clear();
        }
        public static void UnRegisterAll()
        {
            foreach (var msgRecorder in mRegisteredRecorders)
            {
                mRegisteredDict[msgRecorder.name] = null;
                msgRecorder.Recycle();
            }
            mRegisteredDict.Clear();
            mRegisteredRecorders.Clear();
        }

        private class MsgRecorder
        {
            private MsgRecorder(){}//设计私有的构造方法防止这个类被new。
            private static Stack<MsgRecorder> msgRecorderPool = new Stack<MsgRecorder>();
            public static MsgRecorder Allocate(string msgName, Action<object> onReceived)
            {
                return msgRecorderPool.Count > 0 ? msgRecorderPool.Pop() : new MsgRecorder {name = msgName};
            }
            public void Recycle()
            {
                name = null;
                msgRecorderPool.Push(this);
            }
            public string name;
        }
    }
}