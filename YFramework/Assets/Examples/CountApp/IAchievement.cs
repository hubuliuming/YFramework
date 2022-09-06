/****************************************************
    文件：IAchievement.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using YFramework;
using UnityEngine;

namespace CounterApp
{
    public interface IAchievement : ISystem 
    {

    }

    public class AchievementSystem : AbstractSystem ,IAchievement
    {
        protected override void OnInit()
        {
            var counterModel = this.GetModel<ICounterModel>();
            var previousCount = counterModel.Count.Value;
            counterModel.Count.RegisterOnValueChange(newCount =>
            {
                if (previousCount < 10 && newCount >= 10)
                {
                    Debug.Log("解锁十次");
                }

                previousCount = newCount;
            });
        }
    }
}