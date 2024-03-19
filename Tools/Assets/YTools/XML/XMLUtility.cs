/****************************************************
    文件：XMLYUtility.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：XML工具类
*****************************************************/

using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;


public class XmlInfo
{
    public string Path { get; set; }
    public string rootNode = "Root";
    public Dictionary<string, string> attributeDict;
    public List<string> values;
    public string firstValue => values[0];

    public XmlInfo(string path)
    {
        attributeDict = new Dictionary<string, string>();
        values = new List<string>();
        if (!path.EndsWith(".xml"))
            path += ".xml";
        if (!File.Exists(path))
        {
            Debug.LogWarning("该xml文件不存在，路径：" + path);
        }

        Path = path;
    }
}

public class XMLUtility
{
    public static void CreateDefaultXML()
    {
        var path = Application.streamingAssetsPath + "/Config.xml";
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }

        var info = new XmlInfo(Application.streamingAssetsPath + "/Config.xml");
        info.attributeDict.Add("firsValue", "Hello");
        CreateStandardXML(info);
    }

    public static void CreateStandardXML(XmlInfo info)
    {
        XmlDocument docx = new XmlDocument();
        XmlElement root = docx.CreateElement(info.rootNode);
        docx.AppendChild(root);
        foreach (var ele in info.attributeDict)
        {
            var e = docx.CreateElement(ele.Key);
            var a = docx.CreateAttribute(ele.Key);
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
    public static XmlInfo LoadXMLToInfo(string path)
    {
        XmlInfo info = new XmlInfo(path);
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
            XmlElement e = root.ChildNodes[i] as XmlElement;
            var value = e.GetAttribute(name);
            info.attributeDict.Add(name, value);
            info.values.Add(value);
        }

        return info;
    }
}