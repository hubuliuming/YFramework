/****************************************************
    文件：ISystem.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace YFramework
{
    public interface ISystem : ICanSetArchitecture,ICanGetSystem,ICanGetModel,ICanGetUtility,ICanRegisterEvent,ICanSendEvent
    {
        void Init();
    }
    
    public abstract class AbstractSystem : ISystem
    {
        private IArchitecture _architecture;
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return _architecture;
        }
        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            _architecture = architecture;
        }
        void ISystem.Init()
        {
            OnInit();
        }

        protected abstract void OnInit();
    }
}