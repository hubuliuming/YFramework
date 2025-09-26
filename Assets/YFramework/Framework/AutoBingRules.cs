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
            {Img.Name, Img.TName},
            {Txt.Name, Txt.TName},
            {RawImg.Name, RawImg.TName},
            {Tog.Name, Tog.TName},
            {Sld.Name, Sld.TName},
            {ScoBar.Name, ScoBar.TName},
            {Btn.Name, Btn.TName},
            {Drod.Name, Drod.TName},
            {Ipf.Name, Ipf.TName},
            {Cvas.Name, Cvas.TName},
        };
        #region UI
        
        private class Img : IAutoBingElement
        {
            public static string Name = typeof(Img).Name;
            public static string TName = typeof(UnityEngine.UI.Image).FullName;
        }
        private class Txt : IAutoBingElement
        {
            public static string Name =  typeof(Txt).Name;
            public static string TName = typeof(UnityEngine.UI.Text).FullName;
        }
        
        private class RawImg : IAutoBingElement
        {
            public static string Name =  typeof(RawImg).Name;
            public static string TName = typeof(UnityEngine.UI.RawImage).FullName;
        }
        
        private class Tog : IAutoBingElement
        {
            public static string Name =  typeof(Tog).Name;
            public static string TName = typeof(UnityEngine.UI.Toggle).FullName;
        }
        
        private class Sld : IAutoBingElement
        {
            public static string Name =  typeof(Sld).Name;
            public static string TName = typeof(UnityEngine.UI.Slider).FullName;
        }
        
        private class ScoBar : IAutoBingElement
        {
            public static string Name =  typeof(ScoBar).Name;
            public static string TName = typeof(UnityEngine.UI.Scrollbar).FullName;
        }
        
        private class Btn : IAutoBingElement
        {
            public static string Name =  typeof(Btn).Name;
            public static string TName = typeof(UnityEngine.UI.Button).FullName;
        }
        
        private class Drod : IAutoBingElement
        {
            public static string Name =  typeof(Drod).Name;
            public static string TName = typeof(UnityEngine.UI.Dropdown).FullName;
        }
        
        private class Ipf : IAutoBingElement
        {
            public static string Name =  typeof(Ipf).Name;
            public static string TName = typeof(UnityEngine.UI.InputField).FullName;
        }
        
        private class Cvas : IAutoBingElement
        {
            public static string Name =  typeof(Cvas).Name;
            public static string TName = typeof(UnityEngine.Canvas).FullName;
        }
        
        #endregion
    }
    
    public interface IAutoBingElement
    { 
   
    }

    public class AutoBingUti
    {
        public static IAutoBingElement GetElement(string signName)
        {
            if (AutoBingRules.SignToTypeDic.ContainsKey(signName))
            {
                return (IAutoBingElement) Type.GetType(AutoBingRules.SignToTypeDic[signName]);
            }
            return null;
        }
    }
}