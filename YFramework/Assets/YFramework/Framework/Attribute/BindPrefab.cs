/****************************************************
    文件：BindPrefab.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：绑定prefab参数特性
*****************************************************/

using System;

namespace YFramework
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BindPrefab : Attribute 
    {
        public string Path { get; private set; }

        public BindPrefab(string path)
        {
            Path = path;
        }
    }

}