/****************************************************
    文件：BindableProperty.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;

namespace YFramework
{
    public class BindableProperty<T>  where T : IEquatable<T>
    {
        public BindableProperty(T defaultValue = default)
        {
            _value = defaultValue;
        }
        private T _value = default(T);

        public T Value
        {
            get => _value;
            set
            {
                if(value == null && _value ==null) return;
                if (!_value.Equals(value))
                {
                    _value = value;
                    _onValueChanged?.Invoke(value);
                }
            }
        }

        private Action<T> _onValueChanged = v => { };
        public IUnRegister RegisterOnValueChange(Action<T> onValueChange)
        {
            _onValueChanged += onValueChange;
            return new BindablePropertyUnRegister<T>()
            {
                BindableProperty = this,
                OnValueChanged = onValueChange
            };
        }

        public void UnRegisterOnValueChange(Action<T> onValueChange)
        {
            _onValueChanged -= onValueChange;
        }
    }

    public class BindablePropertyUnRegister<T> : IUnRegister where T : IEquatable<T>
    {
        public BindableProperty<T> BindableProperty { get; set; }
        public Action<T> OnValueChanged { get; set; }
        public void UnRegister()
        {
            BindableProperty.UnRegisterOnValueChange(OnValueChanged);
            BindableProperty = null;
            OnValueChanged = null;
        }
    }
    
}