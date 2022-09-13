/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections;
using UnityEngine;
using YFramework.Kit;

public class Test1 : MonoBehaviour
{
    public RectTransform rectTrans;
    private void Start()
    {
        var picture = new Picture(rectTrans);
        // var data = picture.Cut();
        // picture.SaveLocalFile(Application.dataPath,data);
    }

    private IEnumerator Load()
    {
        yield return new WaitForEndOfFrame();
        //
        
    }
    
}