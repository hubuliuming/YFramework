/****************************************************
    文件：Test2.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using YFramework;

public class Test2 : YMonoBehaviour 
{
    
    private void Start()
    {
        
        MsgDispatcher.Register("123", o =>
        {
            var data = (MsgData.Test1)o;
            Debug.Log(data.index);
        });
    }
}