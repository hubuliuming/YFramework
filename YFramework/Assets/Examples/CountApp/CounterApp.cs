/****************************************************
    文件：CounterApp.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using YFramework;
using YFramework.Examples;

namespace CounterApp
{
    public class CounterApp : Architecture<CounterApp>
    {
        protected override void Init()
        {
            RegisterModel<ICounterModel>(new CounterModel());
            RegisterSystem<IAchievement>(new AchievementSystem());
        }
    }
}