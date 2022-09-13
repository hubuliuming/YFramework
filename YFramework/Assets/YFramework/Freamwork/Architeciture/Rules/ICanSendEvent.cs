/****************************************************
    文件：ICanSendEvent.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace YFramework
{
    public interface ICanSendEvent : IBelongToArchitecture 
    {

    }

    public static class CanSendEventExtensive
    {
        public static void SendEvent<T>(this ICanSendEvent self) where T : new()
        {
            self.GetArchitecture().SendEvent<T>();
        }
        public static void SendEvent<T>(this ICanSendEvent self,T t) 
        {
            self.GetArchitecture().SendEvent<T>(t);
        }
    }
}