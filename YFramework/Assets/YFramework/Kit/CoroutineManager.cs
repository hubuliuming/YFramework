/****************************************************
    文件：CoroutineManager.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections;

namespace YFramework.Kit
{
    public  class CoroutineManager : MonoSingleton<CoroutineManager>
    {
        public void ExecuteCor(IEnumerator cor)
        {
            StartCoroutine(cor);
        }

    }
}