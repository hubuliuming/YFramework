/****************************************************
    文件：XMLYUtility.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：XML工具类
*****************************************************/

using System.Collections.Generic;
using System.Xml;

namespace YFramework
{
    public class YXmlInfo
    {
        public string Path { get; set; }
        public string rootNode = "Root";
        public Dictionary<string, string> attributeDict;
        public List<string> values;
        public string FirstValues => values[0];
        public YXmlInfo(string path)
        {
            attributeDict = new Dictionary<string, string>();
            values = new List<string>();
            if (!path.EndsWith(".xml"))
                path += ".xml";
            Path = path;
        }
    }
    public class YXMLUtility
    {
        public static void CreateStandardXML(YXmlInfo info)
        {
            XmlDocument docx = new XmlDocument();
            XmlElement root = docx.CreateElement(info.rootNode);
            docx.AppendChild(root);
            foreach (var ele in info.attributeDict)
            {
                var e = docx.CreateElement(ele.Key);
                var a =docx.CreateAttribute(ele.Key);
                //e.InnerText = ele.Key;
                a.InnerText = ele.Value;
                e.Attributes.Append(a);
                root.AppendChild(e);
            }
            
            docx.Save(info.Path);
        }

        /// <summary>
        /// 从本地路径加载XML到YXMLInfo,注意xml格式必须符合YXMlInfo格式，建议使用CreateStandardXML生成XML文件
        /// </summary>
        /// <param name="path">加载的文件路径，包括名字后缀加上.xml</param>
        /// <returns></returns>
        public static YXmlInfo LoadXMLToInfo(string path)
        {
            YXmlInfo info = new YXmlInfo(path);
            XmlDocument doc = new XmlDocument();
            if (!path.EndsWith(".xml"))
                path += ".xml";
            doc.Load(path);
            info.Path = path;
            XmlElement root = doc.FirstChild as XmlElement;
            if (root == null) return null;
            info.rootNode = root.Name;
            info.attributeDict = new Dictionary<string, string>();
            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                var name = root.ChildNodes[i].Name;
                XmlElement e = root.ChildNodes[i] as  XmlElement;
                var value = e.GetAttribute(name);
               info.attributeDict.Add(name,value);
               info.values.Add(value);
            }

            return info;
        }
    }
}