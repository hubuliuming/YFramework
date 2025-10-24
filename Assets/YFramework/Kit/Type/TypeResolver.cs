/****************************************************
    文件：TypeResolver.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace YFramework.Kit
{
    public static class TypeResolverForUnity
    {
        /// <summary>
        /// 在Unity中通过类名解析Type，并返回其所在程序集
        /// </summary>
        /// <param name="className">类名（可包含命名空间）</param>
        /// <param name="searchInAllAssemblies">是否搜索所有程序集</param>
        /// <param name="priorityAssemblie">优先搜索的程序集</param>
        /// <returns>找到的类型，未找到返回null</returns>
        public static Type ResolveType(string className, bool searchInAllAssemblies = true, params string[] priorityAssemblie)
        {
            if (string.IsNullOrEmpty(className))
            {
                Debug.LogError("类名不能为空");
                return null;
            }

            Type type = null;

            // 1. 首先尝试直接获取（对于已知在核心程序集中的类型）
            type = Type.GetType(className);
            if (type != null)
            {
                return type;
            }

            // 2. 在Unity当前已加载的程序集中搜索
            if (searchInAllAssemblies)
            {
                type = FindTypeInUnityAssemblies(className,priorityAssemblie);
                if (type != null)
                {
                    return type;
                }
            }

            Debug.LogWarning($"未找到类型: {className}");
            return null;
        }

        /// <summary>
        /// 获取类型所在的程序集
        /// </summary>
        public static Assembly GetAssemblyByTypeName(string typeName)
        {
            Type type = ResolveType(typeName);
            return type?.Assembly;
        }

        /// <summary>
        /// 在Unity所有已加载程序集中查找类型
        /// </summary>
        private static Type FindTypeInUnityAssemblies(string typeName,params string[] priorityAssemblie)
        {
            // 获取当前域中所有已加载的程序集
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // 优先检查常见的Unity程序集
            var priorityAssemblies = assemblies.Where(asm =>
                priorityAssemblie.Contains(asm.GetName().Name) ||
                asm.FullName.StartsWith("Assembly-CSharp") ||
                asm.FullName.StartsWith("Unity") ||
                asm.FullName.StartsWith("System.") ||
                asm.FullName.StartsWith("mscorlib")).ToArray();

            // 先在高优先级程序集中查找
            foreach (Assembly assembly in priorityAssemblies)
            {
                try
                {
                    Type type = assembly.GetType(typeName);
                    if (type != null)
                    {
                        //Debug.Log($"在程序集 {assembly.FullName} 中找到类型 {typeName}");
                        return type;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"在程序集 {assembly.FullName} 中查找类型 {typeName} 时出错: {ex.Message}");
                }
            }

            // 高优先级中没找到，搜索所有其他程序集
            foreach (Assembly assembly in assemblies.Except(priorityAssemblies))
            {
                try
                {
                    Type type = assembly.GetType(typeName);
                    if (type != null)
                    {
                        Debug.Log($"在程序集 {assembly.FullName} 中找到类型 {typeName}");
                        return type;
                    }

                    // 尝试通过类名匹配（不包含命名空间）
                    type = assembly.GetTypes()
                        .FirstOrDefault(t => t.Name.Equals(typeName, StringComparison.Ordinal) ||
                                             t.FullName.Equals(typeName, StringComparison.Ordinal));
                    if (type != null)
                    {
                        Debug.Log($"在程序集 {assembly.FullName} 中通过名称找到类型 {typeName} (实际类型: {type.FullName})");
                        return type;
                    }
                }
                catch (Exception ex)
                {
                    // 忽略无法反射的程序集
                    continue;
                }
            }

            return null;
        }

        /// <summary>
        /// 打印类型所在的程序集信息
        /// </summary>
        public static void PrintAssemblyInfo(string className)
        {
            Type type = ResolveType(className);

            if (type != null)
            {
                Assembly assembly = type.Assembly;
                Debug.Log($"类型 '{className}' 所在程序集信息:\n" +
                          $"  程序集: {assembly.FullName}\n" +
                          $"  位置: {assembly.Location}\n" +
                          $"  版本: {assembly.GetName().Version}");
            }
            else
            {
                Debug.LogError($"未找到类型 '{className}'");
            }
        }
    }
}