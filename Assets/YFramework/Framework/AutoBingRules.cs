/****************************************************
    文件：AutoBingRules.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


using System.Collections.Generic;

public class AutoBingRules
{

    public static Dictionary<string, string> SignToTypeDic = new Dictionary<string, string>()
    {
        {"Go", Go.TName},
        {"Img", Img.TName},
    };
    
    public static class Go
    {
        public static string Name = "Go";
        public static string TName = typeof(UnityEngine.GameObject).FullName;
    }
    public static class Img
    {
        public static string Name = "Img";
        public static string TName = typeof(UnityEngine.UI.Image).FullName;
    }
    
    
}