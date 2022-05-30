/****************************************************
    文件：XMLUtilityExample.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace YFramework.Examples
{
    public class XMLUtilityExample : MonoBehaviour 
    {
#if UNITY_EDITOR
        private static string savePath = Application.dataPath + "/YFramework/Examples/TempAgs/xml";
        [UnityEditor.MenuItem("YFramework/Examples/5/XMlCreate")]
        private static void MenuClick()
        {
            Dictionary<string, string> attributie = new Dictionary<string, string>();
            attributie.Add("ip","aa");
            attributie.Add("port","bb");
            YXmlInfo info = new YXmlInfo(savePath)
            {
                attributeDict = attributie
            };
            YXMLUtility.CreateStandardXML(info);
            UnityEditor.AssetDatabase.Refresh();
        }

        [UnityEditor.MenuItem("YFramework/Examples/5/XMlLoad")]
        private static void MenuClick2()
        {
            var info = YXMLUtility.LoadXMLToInfo(savePath);
            Debug.Log(info.rootNode);
            foreach (var e in info.attributeDict)
            {
                Debug.Log(e.Key);
                Debug.Log(e.Value);
            }
        }
#endif
    }
}