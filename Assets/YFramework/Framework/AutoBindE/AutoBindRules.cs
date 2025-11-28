/****************************************************
    文件：AutoBindRules.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


using System;
using System.Collections.Generic;

namespace YFramework
{
    public class AutoBindRules
    {
        public static List<Type> BindElementTypes = new List<Type>()
        {
            typeof(Go),
            typeof(Img),
            typeof(Txt),
            typeof(RawImg),
            typeof(Tog),
            typeof(Sld),
            typeof(ScoBar),
            typeof(Btn),
            typeof(Drod),
            typeof(Ipf),
            typeof(Cvas),
        };

        public static List<Type> FiltrationElementTypes = new List<Type>()
        {
            typeof(YFramework.YMonoBehaviour)
        };
        
        private class Go : IAutoBindElement
        {
            public static string Name = typeof(Go).Name;
            public static string TName = typeof(UnityEngine.GameObject).FullName;
        }
        #region UI
        
        private class Img : IAutoBindElement
        {
            public static string Name = typeof(Img).Name;
            public static string TName = typeof(UnityEngine.UI.Image).FullName;
        }
        private class Txt : IAutoBindElement
        {
            public static string Name =  typeof(Txt).Name;
            public static string TName = typeof(UnityEngine.UI.Text).FullName;
        }
        
        private class RawImg : IAutoBindElement
        {
            public static string Name =  typeof(RawImg).Name;
            public static string TName = typeof(UnityEngine.UI.RawImage).FullName;
        }
        
        private class Tog : IAutoBindElement
        {
            public static string Name =  typeof(Tog).Name;
            public static string TName = typeof(UnityEngine.UI.Toggle).FullName;
        }
        
        private class Sld : IAutoBindElement
        {
            public static string Name =  typeof(Sld).Name;
            public static string TName = typeof(UnityEngine.UI.Slider).FullName;
        }
        
        private class ScoBar : IAutoBindElement
        {
            public static string Name =  typeof(ScoBar).Name;
            public static string TName = typeof(UnityEngine.UI.Scrollbar).FullName;
        }
        
        private class Btn : IAutoBindElement
        {
            public static string Name =  typeof(Btn).Name;
            public static string TName = typeof(UnityEngine.UI.Button).FullName;
        }
        
        private class Drod : IAutoBindElement
        {
            public static string Name =  typeof(Drod).Name;
            public static string TName = typeof(UnityEngine.UI.Dropdown).FullName;
        }
        
        private class Ipf : IAutoBindElement
        {
            public static string Name =  typeof(Ipf).Name;
            public static string TName = typeof(UnityEngine.UI.InputField).FullName;
        }
        
        private class Cvas : IAutoBindElement
        {
            public static string Name =  typeof(Cvas).Name;
            public static string TName = typeof(UnityEngine.Canvas).FullName;
        }
        
        #endregion

        #region Custom
        
        #endregion
    }
    
    public interface IAutoBindElement
    { 
   
    }

    public interface IAutoBindMono 
    {
        UnityEngine.MonoBehaviour Mono { get; }
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class AutoBindFieldAttribute : Attribute
    {
        public string Name;
        public AutoBindFieldAttribute(string name = "")
        {
            Name = name;
        }
    }
    
    public class AutoBindUti
    {
        public static IAutoBindElement GetElement(string signName)
        {
            if (AutoBindRules.BindElementTypes.Contains(Type.GetType(signName)))
            {
                return Activator.CreateInstance(Type.GetType(signName)) as IAutoBindElement;
            }
            return null;
        }
    }
}