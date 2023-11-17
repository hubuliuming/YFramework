using System;
using UnityEngine;

namespace YFramework.Extension
{
    public static class StringExtension
    {
        /// <summary>
        /// 获取字符串里规则里特殊替换后的新字符串
        /// </summary>
        /// <param name="original">原始的字符串 example original = "Hello {@Y}",Good morning!</param>
        /// <param name="rules">原始字符串里规则，example rules = new string[]{"{@","}"}</param>
        /// <param name="replaceParameters">替换的参数,example replaceParameters = new string[]{"xiaoming"}</param>
        /// <returns>按照上面example规则返回值为 Hello xiaoming,Good morning! </returns>
        public static string GetSpecialReplaceRuleStr(string original,string[] rules,string[] replaceParameters)
        {
            var strs = original.Split(rules, StringSplitOptions.None);
            if (replaceParameters.Length != strs.Length /2)
            {
                Debug.LogError("传递的参数长度："+replaceParameters.Length+",文本中规则变量的长度："+strs.Length/2+"不一致");
                return original;
            }
            var info = "";
            for (int i = 0; i < strs.Length -1; i+=2)
            {
                info += strs[i] + replaceParameters[i/2];
            }

            info += strs[^1];
            return info;
        }
        
        /// <summary>
        /// 处理一下文字换行
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="splitLength">达到多长开始换行</param>
        /// <returns></returns>
        public static string HandleText(this string txt,int splitLength)
        {
            var startIndex = txt.IndexOf("\n", StringComparison.Ordinal);
            for (int i = startIndex + splitLength; i < txt.Length; i+=splitLength)
            {
                txt = txt.Insert(i, "\n");
            }

            return txt;
        }
    }
}