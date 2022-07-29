/****************************************************
    文件：YFile.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace YFramework.IO
{
    public static class YFile
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
                Debug.LogError("当前要修改的名称已经存在,名称为：" + newName);
            else
                File.Move(sourceName, destPath);
        }

        /// <summary>
        /// 指定文件夹中的所有文件按照断句分类
        /// </summary>
        /// <param name="folderPath">目标文件夹路径</param>
        /// <param name="splitKey">断点关键字</param>
        /// <returns>分类后的文件名字默认排序的列表</returns>
        public static List<string> Classify(string folderPath, string splitKey)
        {
            var tagForFiles = new Dictionary<string, List<string>>();
            var fullFileNames = Directory.GetFiles(folderPath);
            var tags = new List<string>();
            var failFullNames = new List<string>();
            foreach (var fullFileName in fullFileNames)
            {
                List<string> nameList = new List<string>();
                var fileName = Path.GetFileName(fullFileName);
                var tagName = fileName.Split(splitKey)[0];
                //文件名称未找到关键字
                if (tagName == fileName) 
                {
                    failFullNames.Add(fullFileName);
                    Debug.LogWarning("分类时候该文件:"+fileName+", 未包含分类关键字:"+splitKey);
                    continue;
                }
                if (tagForFiles.ContainsKey(tagName))
                    tagForFiles[tagName].Add(fileName);
                else
                {
                    nameList.Add(fileName);
                    tagForFiles.Add(tagName, nameList);
                    tags.Add(tagName);
                }

                if (!Directory.Exists(folderPath + "/" + tagName))
                   Directory.CreateDirectory(folderPath + "/" + tagName);
                if (File.Exists(folderPath + "/" + tagName + "/" + fileName))
                    Debug.LogWarning("要复制的文件地址已存在相同名字的文件，重复的文件名字：" + fileName);
                else
                {
                    FileInfo fileInfo = new FileInfo(fullFileName);
                    fileInfo.MoveTo(folderPath + "/" + tagName + "/" + fileName);
                }
            }

            var failFolderPath = folderPath + "/" + "ClassifyFail";
            if (!Directory.Exists(failFolderPath))
            {
                Directory.CreateDirectory(failFolderPath);
            }
            foreach (var fullName in failFullNames)
            {
                FileInfo info = new FileInfo(fullName);
                info.MoveTo(failFolderPath+"/"+Path.GetFileName(fullName));
            }

            tags.Sort();
            return tags;
        }
    }
}