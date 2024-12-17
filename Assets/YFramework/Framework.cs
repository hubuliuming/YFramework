/****************************************************
    文件：Framework.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using YFramework.Kit;

namespace YFramework
{
    public class Framework  
    {
        
    }

    public class Trigger
    {
        private bool _value;
        public Action OnTrigger;
        private bool Value
        {
            get => _value; 
            set
            {
                _value = value;
                OnTrigger?.Invoke();
            }
        }

        public void Execute()
        {
            Value = true;
            ActionKit.DelayOneFrame(()=>Value = false);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}