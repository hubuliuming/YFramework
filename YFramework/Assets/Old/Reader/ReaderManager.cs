/****************************************************
    文件：ReaderManager.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;
using YFrameworkOld;

public class ReaderManager : NormalSingleton<ReaderManager>
{
    private Dictionary<string, IReader> _readersDic = new Dictionary<string, IReader>();

    public IReader GetReader(string path)
    {
        IReader reader = null;
        if (_readersDic.ContainsKey(path))
        {
            reader = _readersDic[path];
        }
        else
        {
            reader = ReaderConfig.GetReader(path);
            //LoaderManager.Instance.LoadConfig(path, (data) => reader.SetData(data));
            if (reader != null)
            {
                _readersDic[path] = reader;
            }
            else
            {
                Debug.LogError("未获取到对应reader，路径：" + path);
            }
        }

        return reader;
    }
}