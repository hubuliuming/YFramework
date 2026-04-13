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
        public static List<Type> BindElementTypes = CreateBindElementTypes();

        private static List<Type> CreateBindElementTypes()
        {
            var bindElementTypes = new List<Type>()
            {
                typeof(Go),
                typeof(Anim),
                typeof(Rig),
                typeof(Rig2),
                typeof(Col),
                typeof(Col2),
                typeof(Rect),
                typeof(Img),
                typeof(Txt),
            };

            AddOptionalBindElement(bindElementTypes, typeof(TMP_InputField));
            AddOptionalBindElement(bindElementTypes, typeof(TMP_Dropdown));
            AddOptionalBindElement(bindElementTypes, typeof(TMP));

            bindElementTypes.Add(typeof(RawImg));
            bindElementTypes.Add(typeof(Tog));
            bindElementTypes.Add(typeof(Sld));
            bindElementTypes.Add(typeof(ScoB));
            bindElementTypes.Add(typeof(ScoV));
            bindElementTypes.Add(typeof(Btn));
            bindElementTypes.Add(typeof(Drod));
            bindElementTypes.Add(typeof(Ipf));
            bindElementTypes.Add(typeof(Cvas));

            return bindElementTypes;
        }

        private static void AddOptionalBindElement(List<Type> bindElementTypes, Type bindElementType)
        {
            var targetTypeName = bindElementType.GetField("TName").GetValue(null)?.ToString();
            if (string.IsNullOrEmpty(targetTypeName))
            {
                return;
            }

            if (ResolveOptionalType(targetTypeName) != null)
            {
                bindElementTypes.Add(bindElementType);
            }
        }

        private static Type ResolveOptionalType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null)
            {
                return type;
            }

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = assembly.GetType(typeName);
                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }

        public static List<Type> FiltrationElementTypes = new List<Type>()
        {
            typeof(YFramework.YMonoBehaviour)
        };
        
        private class Go : IAutoBindElement
        {
            public static string Name = typeof(Go).Name;
            public static string TName = typeof(UnityEngine.GameObject).FullName;
        }
        private class Rect : IAutoBindElement
        {
            public static string Name = typeof(Rect).Name;
            public static string TName = typeof(UnityEngine.RectTransform).FullName;
        }
        
        private class Anim : IAutoBindElement
        {
            public static string Name = typeof(Anim).Name;
            public static string TName = typeof(UnityEngine.Animator).FullName;
        }
        
        private class Rig : IAutoBindElement
        {
            public static string Name = typeof(Rig).Name;
            public static string TName = typeof(UnityEngine.Rigidbody).FullName;
        }
        
        private class Rig2 : IAutoBindElement
        {
            public static string Name = typeof(Rig2).Name;
            public static string TName = typeof(UnityEngine.Rigidbody2D).FullName;
        }
        
        private class Col : IAutoBindElement
        {
            public static string Name = typeof(Col).Name;
            public static string TName = typeof(UnityEngine.Collider).FullName;
        }
        
        private class Col2 : IAutoBindElement
        {
            public static string Name = typeof(Col2).Name;
            public static string TName = typeof(UnityEngine.Collider2D).FullName;
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

        private class TMP : IAutoBindElement
        {
            public static string Name = typeof(TMP).Name;
            public static string TName = "TMPro.TextMeshProUGUI";
        }

        private class TMP_InputField : IAutoBindElement
        {
            public static string Name = typeof(TMP_InputField).Name;
            public static string TName = "TMPro.TMP_InputField";
        }

        private class TMP_Dropdown : IAutoBindElement
        {
            public static string Name = typeof(TMP_Dropdown).Name;
            public static string TName = "TMPro.TMP_Dropdown";
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
        
        private class ScoB : IAutoBindElement
        {
            public static string Name =  typeof(ScoB).Name;
            public static string TName = typeof(UnityEngine.UI.Scrollbar).FullName;
        }
        
        private class ScoV : IAutoBindElement
        {
            public static string Name =  typeof(ScoV).Name;
            public static string TName = typeof(UnityEngine.UI.ScrollRect).FullName;
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
        UnityEngine.MonoBehaviour MonoSelf { get; }

        /// <summary>
        ///  Is Ignore sef, if ture parent bing is ignore this
        /// </summary>
        bool IgnoreSelf { get; }
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
