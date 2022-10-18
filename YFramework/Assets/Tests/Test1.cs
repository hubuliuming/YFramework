/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using YFramework.Kit.Net;

public class Test1 : MonoBehaviour
{
   
    private void Start()
    {
        UnityWebRequest request = new UnityWebRequest("192.168.1");
        
        // UnityWebRequestTexture
        // or use VideoPlayer.url
        
    }

    private void Update()
    {
       
    }

    private void OnDestroy()
    {
        
    }
}