/****************************************************
    文件：SubCounterCommand.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using YFramework;

namespace CounterApp
{
    public class SubCounterCommand : AbstractCommand 
    {
        protected override void OnExecute()
        {
            this.GetModel<ICounterModel>().Count.Value--;
        }
    }
}