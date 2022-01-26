/****************************************************
    文件：LevelManager.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/18 9:59:54
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YFramework.Managers
{
    public class LevelManager : MonoBehaviour
    {
        private static List<string> mLevelNames = new List<string>();
        public static int Index { get; set; }
        public static void Init(List<string> levels)
        {
            mLevelNames = levels;
            Index = 0;
        }

        public static void LoadCurrent()
        {
            SceneManager.LoadScene(mLevelNames[Index]);
        }

        public static void LoadNext()
        {
            Index++;
            if (Index >= mLevelNames.Count)
            {
                Index = 0;
            }

            SceneManager.LoadScene(mLevelNames[Index]);
        }
    }
}