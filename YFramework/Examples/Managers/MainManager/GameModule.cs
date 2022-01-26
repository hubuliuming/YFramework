/****************************************************
    文件：Game.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/14 16:26:45
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace YFramework.Managers
{
    public class GameModule : MainManager
    {
        public override void LaunchInDevelopment()
        {
            Debug.Log("development mode");
        }

        public override void LaunchInTest()
        {
            Debug.Log("Test mode");
        }

        public override void LaunchInProduct()
        {
            Debug.Log("product mode");
        }
    }
}