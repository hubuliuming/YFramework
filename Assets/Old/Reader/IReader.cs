/****************************************************
    文件：IReader.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


using System;

public interface IReader
{
    IReader this[int key] { get; }
    IReader this[string key] { get; }
    void Get<T>(Action<T> callback);
    void SetData(object data);
}