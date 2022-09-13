/****************************************************
    文件：ICanGetSystem.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace YFramework
{
    public interface ICanGetSystem : IBelongToArchitecture
    {

    }

    public static class CanGetSystemExtensive
    {
        public static T GetSystem<T>(this ICanGetSystem self) where T : class, ISystem
        {
            return self.GetArchitecture().GetSystem<T>();
        }
    }
}