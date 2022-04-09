/****************************************************
    文件：XMLUtitlityexam.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace YFramework.Examples
{
    public class XMLUtilityExample : MonoBehaviour 
    {
        private static string savePath = Application.dataPath + "/TempAgs/xml.xml";

        [UnityEditor.MenuItem("YFramework/Examples/5/XMlCreate")]
        private static void MenuClick()
        {
            YXmlInfo xml = new YXmlInfo(savePath);
        }
    }
}