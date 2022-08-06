using System;

namespace ZPlugin {
    public enum SetBy {
        Children,
        Parent,
        SceneObject,
    }

    /// <summary>
    /// AutoSet 特性, 自动设置字段引用
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class AutoSetAttribute : Attribute {
        public string Name;
        public SetBy SetBy;

        public AutoSetAttribute(string Name = null, SetBy SetBy = SetBy.Children) {
            this.Name = Name;
            this.SetBy = SetBy;
        }
    }
}