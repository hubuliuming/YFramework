/****************************************************
    文件：ComponentExtension.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace YFramework.Extension
{
    public static  class ComponentExtension 
    {
        public static T GetCopyOf<T>(this Component comp, T other) where T : Component
        {
            System.Type type = comp.GetType();
            if (type != other.GetType()) return null; // 类型不匹配，返回null
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                if (field.IsStatic) continue;
                field.SetValue(comp, field.GetValue(other));
            }
            System.Reflection.PropertyInfo[] properties = type.GetProperties();
            foreach (System.Reflection.PropertyInfo prop in properties)
            {
                if (!prop.CanWrite || prop.Name == "name") continue;
                prop.SetValue(comp, prop.GetValue(other, null), null);
            }
            return comp as T;
        }

    }
}