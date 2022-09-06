/****************************************************
    文件：ICommand.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

namespace YFramework
{
    public interface ICommand : ICanSetArchitecture,ICanSendCommand,ICanGetModel,ICanGetSystem,ICanGetUtility,ICanSendEvent
    {
        void Execute();
    }
    
    public abstract class AbstractCommand : ICommand
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
        void ICommand.Execute()
        {
            OnExecute();
        }

        protected abstract void OnExecute();
    }
}