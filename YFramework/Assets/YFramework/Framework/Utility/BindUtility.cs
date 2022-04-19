/****************************************************
    文件：BindUtility.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace YFramework
{
    public class BindUtility
    {
        private static Dictionary<string, Type> prefabAndScriptsMap = new Dictionary<string, Type>();
        public static void Bind(string path,Type type)
        {
            if (!prefabAndScriptsMap.ContainsKey(path))
            {
                prefabAndScriptsMap.Add(path,type);
            }
            else
            {
                Debug.LogError("当前数据中已存在路径："+path);
            }
        }

        public static Type GetType(string path)
        {
            if (!prefabAndScriptsMap.TryGetValue(path,out var type))
            {
                Debug.LogError("当前数据中不包括该路径："+path);
                return null;
            }

            return type;
        }
    }
}