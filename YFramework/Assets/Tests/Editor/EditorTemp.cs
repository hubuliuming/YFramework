/****************************************************
    文件：EditorTemp1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using YFramework.IO;

#if UNITY_EDITOR
public class EditorTemp
{
    //private static string _path = @"F:\UnityProjects\Y_Person\项目素材\华科_毕业照\整理好后制作开发";
    //
    // [MenuItem("TempClick/Click1")]
    // private static void Click1()
    // {
    //     List<string> infoNames = new List<string>();
    //     var dirNames = Directory.GetDirectories(_path);
    //     foreach (var dirName in dirNames)
    //     {
    //         FileInfo info = new FileInfo(dirName);
    //         Debug.Log(info.Name);
    //         infoNames.Add(info.Name);
    //     }
    //
    //     var splitKey = "届";
    //     
    //     foreach (var infoName in infoNames)
    //     {
    //         var folderPath = _path +"/"+infoName;
    //         var tagForFiles = new Dictionary<string, List<string>>();
    //         var fullFileNames = Directory.GetFiles(folderPath);
    //         var tags = new List<string>();
    //         foreach (var fullFileName in fullFileNames)
    //         {
    //             List<string> nameList = new List<string>();
    //             var fileName = Path.GetFileName(fullFileName);
    //             var tagName = fileName.Split(splitKey)[0];
    //             //文件名称未找到关键字
    //             if (tagName == fileName) 
    //             {
    //                 Debug.LogWarning("分类时候该文件:"+fileName+", 未包含分类关键字:"+splitKey);
    //                 var failFolderPath = folderPath + "/" + infoName +"/" + "ClassifyFail";
    //                 if (!Directory.Exists(failFolderPath))
    //                 {
    //                     Directory.CreateDirectory(failFolderPath);
    //                 }
    //
    //                 FileInfo info = new FileInfo(fullFileName);
    //                 info.MoveTo(failFolderPath + "/" + fileName);
    //                 continue;
    //             }
    //             if (tagForFiles.ContainsKey(tagName))
    //                 tagForFiles[tagName].Add(fileName);
    //             else
    //             {
    //                 nameList.Add(fileName);
    //                 tagForFiles.Add(tagName, nameList);
    //                 tags.Add(tagName);
    //             }
    //
    //             var destFolder = folderPath + "/" + tagName + splitKey +"/"+ infoName;
    //             if (!Directory.Exists(destFolder))
    //                 Directory.CreateDirectory(destFolder);
    //             if (File.Exists(destFolder + "/" + fileName))
    //                 Debug.LogWarning("要移动的文件地址已存在相同名字的文件，重复的文件名字：" + fileName);
    //             else
    //             {
    //                 FileInfo fileInfo = new FileInfo(fullFileName);
    //                 fileInfo.MoveTo(destFolder + "/" + fileName);
    //             }
    //         }
    //         tags.Sort();
    //     }
    //     
    // }
    //

    [MenuItem("TempClick/Click2")]
    private static void Click2()
    {
        var path = @"F:\UnityProjects\Y_Person\项目素材\华科_毕业照\整理好后制作开发\整理好后制作开发\13机械\ClassifyFail";

        // var fileNames = Directory.GetFiles(path);
        // foreach (var fileName in fileNames)
        // {
        //     var name = Path.GetFileName(fileName);
        //     var newName = name.Insert(5, "机械学院");
        //     YFile.ReName(fileName,newName);
        //     
        // }
    }

    [MenuItem("TempClick/Click3")]
    private static void Click3()
    {
        var path = @"F:\UnityProjects\Y_Person\项目素材\华科_毕业照\整理好的";
        var newFolders = YFile.Classify(path, "届");
        foreach (var newFolder in newFolders)
        {
            var folderPath = path+"/" + newFolder;
            if (!Directory.Exists(folderPath))
            {
                Debug.LogError("found path error path :"+folderPath);
                continue;
            }
            YFile.Classify(folderPath, "学院");
        }
    }
}
#endif