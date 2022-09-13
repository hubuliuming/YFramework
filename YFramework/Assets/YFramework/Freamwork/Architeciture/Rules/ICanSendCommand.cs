/****************************************************
    文件：ICanSendCommand.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace YFramework
{
    public interface ICanSendCommand : IBelongToArchitecture
    {

    }
    
    public static class CanSendCommandExtensive
    {
        public static void SendCommand<T>(this ICanSendCommand self) where T : ICommand,new()
        {
             self.GetArchitecture().SendCommand<T>();
        }
        public static void SendCommand<T>(this ICanSendCommand self, T command) where T : ICommand,new()
        {
            self.GetArchitecture().SendCommand<T>(command);
        }
    }
}