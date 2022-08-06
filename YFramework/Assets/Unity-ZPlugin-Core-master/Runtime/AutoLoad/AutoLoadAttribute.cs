using System;

namespace ZPlugin {
    public enum LoadFrom {
        Resources
    }

    /// <summary>
    /// AutoSet 特性, 自动设置字段引用
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class AutoLoadAttribute : Attribute {
        public string Name;
        public LoadFrom LoadFrom;
        public string Path;

        public AutoLoadAttribute(string Path = "", string Name = "", LoadFrom LoadFrom = LoadFrom.Resources) {
            this.Name = Name;
            this.Path = Path;
            this.LoadFrom = LoadFrom;
        }
    }
}