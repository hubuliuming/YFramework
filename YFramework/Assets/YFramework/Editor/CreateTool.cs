/****************************************************
    文件：CreateTool.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


using YFramework.Kit.Utility;

namespace YFramework.Editor
{
#if UNITY_EDITOR
    public class CreateTool 
    {
        [UnityEditor.MenuItem("YFramework/CreateTool/CreateDefaultXML", false, 1)]
        private static void CreateDefaultXML()
        {
            XMLUtility.CreateDefaultXML();
            UnityEditor.AssetDatabase.Refresh();
        }
    }
#endif
}