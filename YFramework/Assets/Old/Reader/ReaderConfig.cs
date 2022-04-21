/****************************************************
    文件：ReaderConfig.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class ReaderConfig
{
    private static readonly Dictionary<string, Func<IReader>> m_readersDic = new Dictionary<string, Func<IReader>>()
    {
        //{".json", () => new JsonReader()}
    };

    public static IReader GetReader(string path)
    {
        foreach (KeyValuePair<string, Func<IReader>> pair in m_readersDic)
        {
            if (path.Contains(pair.Key))
            {
                return pair.Value();
            }
        }

        Debug.LogError("未找到对应文件的读取器，文件路径：" + path);
        return null;
    }
}