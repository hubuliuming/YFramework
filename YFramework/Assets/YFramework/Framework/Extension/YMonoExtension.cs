/****************************************************
    文件：YMonoExtension.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using YFramework;

public static class YMonoExtension 
{
    public static void RegisterEvent(this YMonoBehaviour yMono, int order)
    {
       Event.RegisterEvent(yMono,order);
    }
}