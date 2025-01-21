/****************************************************
    文件：GuiUtil.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 生成获取GUIid
///
/// </summary>
public class GuidUtil 
{

    private static uint id = 0;
    private static HashSet<uint> ids = new HashSet<uint>();

    /// <summary>
    /// 分配Guid
    /// </summary>
    /// <returns></returns>
    public static uint GetGuid()
    {
        if(id==uint.MaxValue)
            id = 0;
        uint res = id++;
        while (ids.Contains(res))
        {
            if (id == uint.MaxValue)
                id = 0;
            id++;
        }
        ids.Add(res);
        return res;
    }
}