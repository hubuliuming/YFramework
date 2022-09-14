using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace YFramework.Editor
{
#if UNITY_EDITOR
    public class InputResourcesSetting : AssetPostprocessor
    {
        private static FileData _fileData;
        //private string m_ruloPattern = "^[0-9]+_[0-9]+$";
        private void OnPreprocessTexture()
        {
            SetTexture();
            //NamingConvention("Test",m_ruloPattern);
        }

        private void SetTexture()
        {
            TextureImporter importer = assetImporter as TextureImporter;
            importer.textureType = TextureImporterType.Sprite;
            importer.mipmapEnabled = false;
        }

        private void NamingConvention(string path,string rulePattern)
        {
            if (assetPath.Contains(path))
            {
                var name = Path.GetFileNameWithoutExtension(assetPath);
                //正则表达式
                //string pattern = "^[0-9]+_[0-9]+$";
                var result = Regex.Match(name, rulePattern);
                if (result.Value == "")
                {
                    if (_fileData == null)
                    {
                        _fileData = new FileData();
                        _fileData.Path = path;
                        _fileData.NameTip = "规范为(正则表达式)：" + rulePattern;
                    }
                    Debug.LogError("导入的资源命名不符合规范，文件名字："+name);
                    NameMgrWindowData.Add(_fileData,assetPath);
                    NameMgrWindow.Show();
                }
            }
        }
    }
#endif
}