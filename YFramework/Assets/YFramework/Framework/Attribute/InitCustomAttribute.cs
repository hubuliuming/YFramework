/****************************************************
    文件：InitCustomAttribute.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Reflection;

namespace YFramework
{
    public class InitCustomAttribute 
    {
        public void Init()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(BindPrefab));
            var types = assembly.GetExportedTypes();
            foreach (var type in types)
            {
                foreach (var attribute in Attribute.GetCustomAttributes(type,true))
                {
                    if (attribute is BindPrefab)
                    {
                        BindPrefab data = attribute as BindPrefab;
                        BindUtility.Bind(data.Path,type);
                    }
                }
            }
        }
    }
}