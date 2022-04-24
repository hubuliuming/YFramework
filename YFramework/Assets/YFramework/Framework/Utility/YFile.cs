/****************************************************
    文件：YFile.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.IO;
using UnityEngine;

namespace YFramework
{
    public class YFile 
    {
        /// <summary>
        /// 文件重命名操作
        /// </summary>
        /// <param name="sourceName">带有文件路径的全名称</param>
        /// <param name="newName">输入的新文件名字</param>
        public static void ReName(string sourceName, string newName)
        {
            var name = Path.GetFileNameWithoutExtension(sourceName);
            var destPath = sourceName.Replace(name, newName);
            if (File.Exists(destPath))
            {
                Debug.LogError("当前要修改的名称已经存在,名称为："+newName);
            }
            else
            {
                File.Move(sourceName,destPath);
            }
            
        }
    }
}