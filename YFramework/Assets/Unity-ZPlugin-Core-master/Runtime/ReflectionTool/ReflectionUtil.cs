using System;
using System.Reflection;

namespace ZPlugin {
    public static class ReflectionUtil {
        const BindingFlags ALL_FIELDS_FLAG = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public static void EachField(object obj, Action<FieldInfo> action) {
            FieldInfo[] fieldInfos = obj.GetType().GetFields(ALL_FIELDS_FLAG);
            foreach (var fieldInfo in fieldInfos) action(fieldInfo);
        }

        public static void EachField(object obj, Action<string, object> action) {
            EachField(obj, (fi) => action(fi.Name, fi.GetValue(obj)));
        }

        public static void EachField(object obj, Func<string, object, object> action) {
            EachField(obj, (fi) => fi.SetValue(obj, action(fi.Name, fi.GetValue(obj))));
        }

        /// <summary>
        /// 得到类中所有具有 T属性的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="action"></param>
        public static void EachFieldWithAttr<T>(object obj, Action<FieldInfo, T> action) where T : Attribute {
            EachField(obj, (fi) => {
                Array.ForEach(fi.GetCustomAttributes(typeof(T), true), (arr) => {
                    if (arr is T t) action(fi, t);
                });
            });
        }

        /// <summary>
        /// 得到所有具有 T属性的类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="action"></param>
        public static void EachClassWithAttr<T>(object obj, Type assemblyType, Action<Type, T> action) where T : Attribute {
            Assembly asm = Assembly.GetAssembly(assemblyType);
            Type[] types = asm.GetTypes();
            foreach (var type in types) {
                System.Object[] attrs = type.GetCustomAttributes(typeof(T), true);
                if (attrs != null && attrs.Length > 0) {
                    foreach (var attr in attrs) {
                        T tAttr = attr as T;
                        //如果这个类包含有 tAttr 特性
                        if (tAttr != null) {
                            action(type, tAttr);
                            break;
                        }
                    }
                }
            }
        }
    }
}