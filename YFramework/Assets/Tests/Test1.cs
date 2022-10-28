/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using DG.Tweening;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using YFramework.Kit.Net;

public class Test1 : MonoBehaviour
{
   
    private void Start()
    {
        var curPos = transform.localPosition;
        transform.DOLocalPath(new[] { curPos, curPos + Vector3.right * 10f ,curPos}, 3,PathType.Linear);

    }

    private void Update()
    {
       
    }

    private void OnDestroy()
    {
        
    }
}