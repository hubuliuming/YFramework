/****************************************************
    文件：Home.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/14 16:26:54
    功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;
using YFramework.Managers;

public class HomeModule : MainManager
{
    public void LoadScene()
    {
        SceneManager.LoadScene("Game");
    }
    public override void LaunchInDevelopment()
    {
        Debug.Log("development mode");
    }

    public override void LaunchInTest()
    {
        Debug.Log("Test mode");
        LoadScene();
    }

    public override void LaunchInProduct()
    {
        Debug.Log("product mode");
    }
}