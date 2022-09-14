/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using YFramework.Kit;

public class Test1 : MonoBehaviour
{
    public Picture picture;
    private void Start()
    {
        picture.CreatePictureToLocalFile(Application.dataPath+"/Video");
    }
    
    
}