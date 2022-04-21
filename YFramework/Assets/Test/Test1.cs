/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;
using YFramework;

public class Test1 : YMonoBehaviour
{
    private void Start()
    {
        LoaderManager loader = new LoaderManager(new ResLoader());
        var str = loader.LoadConfig(Application.dataPath + "/YFramework/Examples/TempAgs/json.json");
        Debug.Log(str);
    }
}