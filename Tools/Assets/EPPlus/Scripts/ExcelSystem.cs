/****************************************************
    文件：ExcelSystem.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using UnityEngine;

public class ExcelSystem 
{

    /// <summary>
    /// 获取从当前行往下一定长度的Excel信息
    /// </summary>
    /// <param name="path"></param>
    /// <param name="sheetName"></param>
    /// <param name="raw">从哪个行开始</param>
    /// <param name="rawLength">读取行最大长度</param>
    /// <param name="columnLenght">读取本行列数最大长度</param>
    /// <returns></returns>
    public static Dictionary<string, List<string>> GetInfo(string path, string sheetName, int raw, int rawLength = 100,int columnLenght =20)
    {
        var resFileInfo = new FileInfo(path);
        var dic = new Dictionary<string, List<string>>();
        //打开流Excel
        using ExcelPackage excelPackage = new ExcelPackage(resFileInfo);
        if (excelPackage.Workbook.Worksheets[sheetName] == null) {
            //表格创建
            Debug.Log("读取Sheet失败,名字："+sheetName);
            return null;
        }
        var worksheet = excelPackage.Workbook.Worksheets[sheetName];
        for (int i = raw; i < rawLength; i++)
        {
            string a = worksheet.Cells[i, 1].Value as string;
            if (a == null || a.Equals(""))
            {
                break;
            }
            var list = new List<string>();
            for (int j = 2; j < columnLenght; j++)
            {
                string info = worksheet.Cells[i, j].Value as string;
                if(info == null || info.Equals("")) continue;
                list.Add(info); 
            }
            dic.Add(a, list);
        }
        
        return dic;
    }

    
}