/****************************************************
    文件：YMonoBehaviour.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/11 16:40:53
    功能：
*****************************************************/

using UnityEngine;

namespace YFramework
{
    public abstract class YMonoBehaviour : MonoBehaviour
    {
        protected virtual bool IsActive()
        {
            return isActiveAndEnabled;
        }
        
    }
}