/**********************************************************************
* 将 AutoSet 特性放在字段上 调用 this.SetFileds(推荐在 Awake方法内)为其设置引用
* 将会自动查找对应字段的类
* 
* name 为引用的实例游戏物体名称
* 当 name == "", name 为字段的名称(自动首字母大写)
* 当 name == null, 代表匹配所有的名称
* 
* SetBy 支持 后代物体,祖先物体,Resource资源(使用 path),场景内唯一且激活的物体
* 
* SetFileds 可以配置进行查找的 Transform 对象 ,默认就为本对象
***********************************************************************/

using System;
using UnityEngine;


namespace ZPlugin {
    /// <summary>
    /// AutoSet 特性辅助类
    /// </summary>
    public static class AutoSetUtil {
        /// <summary>
        /// 为所有 AutoSet 特性的字段设置引用
        /// </summary>
        /// <param name="origin"></param>
        public static void SetFields(this Component origin) {
            ReflectionUtil.EachFieldWithAttr<AutoSetAttribute>(origin, (fieldInfo, attr) => {
                string name = FormatName(attr.Name, fieldInfo.Name);
                object value = GetComp(origin, attr.SetBy, fieldInfo.FieldType, name);
                fieldInfo.SetValue(origin, value);
            });
        }

        public static void SetFields(this object obj, Component origin) {
            ReflectionUtil.EachFieldWithAttr<AutoSetAttribute>(obj, (fieldInfo, attr) => {
                string name = FormatName(attr.Name, fieldInfo.Name);
                object value = GetComp(origin, attr.SetBy, fieldInfo.FieldType, name);
                fieldInfo.SetValue(obj, value);
            });
        }

        public static void SetFields(this object obj) {
            ReflectionUtil.EachFieldWithAttr<AutoSetAttribute>(obj, (fieldInfo, attr) => {
                string name = FormatName(attr.Name, fieldInfo.Name);
                Transform temp = UnityEngine.Object.FindObjectOfType<Transform>();
                object value = GetComp(temp, attr.SetBy, fieldInfo.FieldType, name);
                fieldInfo.SetValue(obj, value);
            });
        }

        private static object GetComp(Component origin, SetBy setBy, Type fieldType, string name) {
            switch (setBy) {
                case SetBy.Children:
                    return origin.transform.GetComponentInChildren(fieldType, name);
                case SetBy.Parent:
                    return origin.transform.GetComponentInParent(fieldType, name);
                case SetBy.SceneObject:
                    return origin.transform.GetComponentInScene(fieldType, name);
                default:
                    return null;
            }
        }

        public static string FormatName(string name, string fiName) {
            if (name == null) return null;
            return FirstCharToUpper(name == "" ? fiName : name);
        }

        private static string FirstCharToUpper(string name) {
            if (string.IsNullOrEmpty(name)) return name;
            return name[0].ToString().ToUpper() + name.Substring(1);
        }
    }
}