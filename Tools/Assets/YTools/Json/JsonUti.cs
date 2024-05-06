/****************************************************
    文件：YJsonUtility.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Json信息存储操作，注意需要序列化的数据
*****************************************************/

using System.IO;
using Newtonsoft.Json;


public static class JsonUti
{
    public static string WriteToJson<T>(T data, string savePath)
    {
        if (!savePath.EndsWith(".json"))
            savePath += ".json";
        var jsonStr = JsonConvert.SerializeObject(data, Formatting.Indented);
        StreamWriter sw = new StreamWriter(savePath);

        sw.Write(jsonStr);
        sw.Close();
        return jsonStr;
    }

    public static T ReadFromJson<T>(string path)
    {
        if (!path.EndsWith(".json"))
            path += ".json";
        StreamReader sr = new StreamReader(path);
        var data = sr.ReadToEnd();
        sr.Close();
        return JsonConvert.DeserializeObject<T>(data);
    }
}