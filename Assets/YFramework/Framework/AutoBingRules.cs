/****************************************************
    文件：AutoBingRules.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


using System;
using System.Collections.Generic;

namespace YFramework
{
    public class AutoBingRules
    {

        public static Dictionary<string, string> SignToTypeDic = new Dictionary<string, string>()
        {
            {"Img", Img.TName},
            {"Txt", Text.TName},
        };
        
        public static class Img
        {
            public static string Name = "Img";
            public static string TName = typeof(UnityEngine.UI.Image).FullName;
        }
        public static class Text
        {
            public static string Name = "Txt";
            public static string TName = typeof(UnityEngine.UI.Text).FullName;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class AutoBingAttribute : Attribute
    {
        public string Name { get; }
        
        public AutoBingAttribute(string name = "")
        {
            Name = name;
        }
    }
}