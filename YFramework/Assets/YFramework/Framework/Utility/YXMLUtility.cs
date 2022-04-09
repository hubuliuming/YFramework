/****************************************************
    文件：XMLYUtility.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：XML工具类
*****************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace YFramework
{
    public class YXmlInfo
    {
        public string mPath;
        public List<string> elist;

        public YXmlInfo(string path)
        {
            this.mPath = path;
            elist = new List<string>();
            
            // FileInfo info = new FileInfo(mPath);
            // var fs = info.Create();
            // StreamWriter sr = new StreamWriter(fs);
            // sr.Write("<Root>");
            // sr.WriteLine();
            // sr.Write("/<Root>");
            // sr.Close();
            // fs.Close();


            XmlDocument doc = new XmlDocument();
        
            doc.CreateElement("root",mPath);
            doc.CreateElement("port", mPath);
            doc.CreateAttribute("ip", "192", mPath);

            // var xmlWriter = XmlWriter.Create(mPath);
            // xmlWriter.WriteStartDocument();
            // xmlWriter.WriteStartElement("root");
            // xmlWriter.WriteStartElement("ip");
            // xmlWriter.WriteAttributeString("hao","dd");
            // xmlWriter.WriteEndElement();
            // xmlWriter.WriteEndElement();
            // xmlWriter.WriteEndDocument();
            // xmlWriter.Close();
        }

        public void Add(string e)
        {
            elist.Add(e);
          

            foreach (var s in elist)
            {
                StreamWriter sr = new StreamWriter(mPath);
                sr.Write("Roo/");
            }
            
        }
    }
    public class YXMLUtility 
    {
        public static void Create()
        {
            
        }
        
        
        public void Load()
        {
            
            // XmlDocument xml = new XmlDocument();
            // ipconfigs.Load(Application.streamingAssetsPath + "//" + ipconfigPath + ".xml");
            // XmlElement ipconfigE = ipconfigs.FirstChild as XmlElement;
            // if (ipconfigE != null)
            // {
            //     XmlElement ipe = ipconfigE.FirstChild as XmlElement;
            //     ip = ipe.GetAttribute("ip");
            //     Debug.Log(ip);
            //     XmlElement porte = ipconfigE.ChildNodes[1] as XmlElement;
            //     port = Convert.ToInt32(porte.GetAttribute("port"));
            //     Debug.Log(port);
            // }
        }
    }
}