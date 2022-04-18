/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using YFramework;

public class Test1 : YMonoBehaviour
{
    private void Awake()
    {
        MsgRegister("1", o =>
        {
            Debug.Log("receiver the data is "+o);
        });
    }

    protected override void OnBeforeDestroy()
    {
        
    }
}