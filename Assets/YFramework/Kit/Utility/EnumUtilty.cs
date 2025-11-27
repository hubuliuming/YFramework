using System;
using System.Collections.Generic;
using UnityEngine;

namespace YFramework.Kit.Utility
{
    public class EnumUtility
    {
        /// <summary>
        /// 获取Flags枚举中所有被选中的成员名称
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="enumValue">枚举值</param>
        /// <param name="excludeZero">是否排除值为0的成员</param>
        /// <returns>被选中的成员名称列表</returns>
        public static List<string> GetSelectedFlagNames<T>(T enumValue, bool excludeZero = true) where T : Enum
        {
            var selectedNames = new List<string>();
            var enumType = typeof(T);
        
            if (!enumType.IsDefined(typeof(FlagsAttribute), false))
            {
                Debug.LogWarning($"枚举类型 {enumType.Name} 没有使用 [Flags] 特性");
                selectedNames.Add(enumValue.ToString());
                return selectedNames;
            }

            foreach (T flag in Enum.GetValues(enumType))
            {
                int flagInt = System.Convert.ToInt32(flag);
            
                // 排除值为0的成员（如Nothing/None）
                if (excludeZero && flagInt == 0)
                    continue;

                // 检查是否包含该标志
                if (enumValue.HasFlag(flag))
                {
                    selectedNames.Add(flag.ToString());
                }
            }

            return selectedNames;
        }

        /// <summary>
        /// 获取Flags枚举中所有被选中的成员名称（字符串形式）
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="enumValue">枚举值</param>
        /// <param name="separator">分隔符</param>
        /// <param name="excludeZero">是否排除值为0的成员</param>
        /// <returns>用分隔符连接的成员名称字符串</returns>
        public static string GetSelectedFlagNamesString<T>(T enumValue, string separator = ", ", bool excludeZero = true) where T : Enum
        {
            var names = GetSelectedFlagNames(enumValue, excludeZero);
            return string.Join(separator, names);
        }
        
        /// <summary>
        /// 检查Flags枚举是否包含特定标志
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="enumValue">枚举值</param>
        /// <param name="flag">要检查的标志</param>
        /// <returns>是否包含该标志</returns>
        public static bool ContainsFlag<T>(T enumValue, T flag) where T : Enum
        {
            return enumValue.HasFlag(flag);
        }

        /// <summary>
        /// 获取枚举的所有可能标志（排除值为0的成员）
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns>所有可能的标志列表</returns>
        public static List<T> GetAllFlags<T>() where T : Enum
        {
            var flags = new List<T>();
            foreach (T flag in Enum.GetValues(typeof(T)))
            {
                int flagInt = System.Convert.ToInt32(flag);
                if (flagInt != 0) // 排除值为0的成员
                {
                    flags.Add(flag);
                }
            }
            return flags;
        }
    }
}