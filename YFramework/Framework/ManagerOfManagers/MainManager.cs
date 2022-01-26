/****************************************************
    文件：MainManager.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/14 16:4:58
    功能：主管理
*****************************************************/
using UnityEngine;
namespace YFramework.Managers
{
    public enum EnvironmentMode
        {
            Development,
            Test,
            Product,
        }
        public abstract class MainManager : MonoBehaviour
        {
            public EnvironmentMode Mode;
            private static EnvironmentMode mShareMode;
            private static bool mModeSetted = false;
            private void Start()
            {
                if (!mModeSetted)
                {
                    mShareMode = Mode;
                    mModeSetted = true;
                }
                switch (mShareMode)
                {
                    case EnvironmentMode.Development:
                        LaunchInDevelopment();
                        break;
                    case EnvironmentMode.Test:
                        LaunchInTest();
                        break;
                    case EnvironmentMode.Product:
                        LaunchInProduct();
                        break;
                }
            }
            
            public abstract void LaunchInDevelopment();
            public abstract void LaunchInTest();
            public abstract void LaunchInProduct();
        }
}