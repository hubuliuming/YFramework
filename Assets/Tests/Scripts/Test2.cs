/****************************************************
    文件：Test2.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using YFramework.UI;

public partial class Test2 : UIBase 
{
    #region 成员变量

    #endregion

    public override void OnStart()
    {
        Debug.Log("OnStart");
    }

    protected override void OnShow()
    {
        Debug.Log("OnShow");
    }

    protected override void OnHide()
    {
        Debug.Log("OnHide");
    }
}