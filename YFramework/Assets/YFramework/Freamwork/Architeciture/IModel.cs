/****************************************************
    文件：IModel.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

namespace YFramework
{
    public interface IModel: ICanSetArchitecture, ICanGetUtility ,ICanSendEvent
    {
        void Init();
        BindableProperty<int> Count { get; }
    }

    public abstract class AbstractModel : IModel
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

        void IModel.Init()
        {
            OnInit();
        }

        public BindableProperty<int> Count { get; }

        protected abstract void OnInit();
    }
}