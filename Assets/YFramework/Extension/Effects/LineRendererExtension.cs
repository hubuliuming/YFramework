/****************************************************
    文件：LineRenderer.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace YFramework.Extension
{
    public static class LineRendererExtension  
    {
        public static void AddPoint(this LineRenderer lr, Vector3 pos)
        {
            lr.positionCount++;
            lr.SetPosition(lr.positionCount - 1, pos);
        }

        public static void AddPoints(this LineRenderer ly, Vector3[] pos)
        {
            foreach (var po in pos)
            {
                ly.AddPoint(po);
            }
        }
    }
}