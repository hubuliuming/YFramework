/****************************************************
    文件：Test2.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using YFramework;
using YFramework.UI;

namespace CC
{
    public partial class Test2 : MonoBehaviour,IAutoBindMono 
    {
        #region 成员变量
        public GameObject dd;
        #endregion
        

        public MonoBehaviour MonoSelf => this;
        public bool IgnoreSelf { get; }
    }
}