/****************************************************
    文件：ICanRegisterEvent.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;

namespace YFramework
{
    public interface ICanRegisterEvent : IBelongToArchitecture 
    {
        
    }

    public static class CanRegisterEventExpensive
    {
        public static IUnRegister RegisterEvent<T>(this ICanRegisterEvent self,Action<T> onEvent)
        {
            return self.GetArchitecture().RegisterEvent<T>(onEvent);
        }
        
        public static void UnRegisterEvent<T>(this ICanRegisterEvent self,Action<T> onEvent)
        {
             self.GetArchitecture().UnRegisterEvent<T>(onEvent);
        }
    }
}