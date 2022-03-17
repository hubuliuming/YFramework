/****************************************************
    文件：YJsonUtility.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Json信息存储操作，注意需要序列化的数据
*****************************************************/

using System.IO;
using UnityEngine;

namespace YFramework
{
    public static class YJsonUtility 
    {
        public static void Save<T>(T data,string savePath) where T : class
        {
            var jsonStr = JsonUtility.ToJson(data);
            StreamWriter sw = new StreamWriter(savePath);
            sw.Write(jsonStr);
            sw.Close();
        }

        public static T Load<T>(string path) where T : class
        {
            StreamReader sr = new StreamReader(path);
            var data = sr.ReadToEnd(); 
            sr.Close();
            return JsonUtility.FromJson<T>(data);
        }
    }
}