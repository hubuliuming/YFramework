/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using YFramework.Kit;
using YFramework.UI;

public partial class Test1 : UIBase
{
    protected void Start()
    {
        OnStart();
    }

    private void Update()
    {
        
    }

    public override void OnStart()
    {
        // BtnShow.onClick.AddListener(() =>
        // {
        //     UIETest2.Show();
        // });
        // BtnHide.onClick.AddListener(() =>
        // {
        //     UIETest2.Hide();
        // });
    }

    protected override void OnShow()
    {
    }

    protected override void OnHide()
    {
        
    }
}



