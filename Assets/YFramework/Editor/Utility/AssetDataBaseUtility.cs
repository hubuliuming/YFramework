using System.IO;
using UnityEditor;
using UnityEngine;

namespace YFramework.Kit.Utility.Editor
{
    public class AssetDataBaseUtility
    {
        /// <summary>
        /// 复制指定路径的资源文件
        /// </summary>
        /// <param name="sourcePath">源文件路径(如: "Assets/Textures/Image.png")</param>
        /// <param name="targetPath">目标路径(如: "Assets/Textures/Image_Copy.png")</param>
        /// <returns>是否复制成功</returns>
        public static bool CopyAssetFile(string sourcePath, string targetPath)
        {
            if (!File.Exists(sourcePath))
            {
                Debug.LogError($"源文件不存在: {sourcePath}");
                return false;
            }

            if (File.Exists(targetPath))
            {
                Debug.LogError($"目标文件已存在: {targetPath}");
                return false;
            }
            
            var succuss = AssetDatabase.CopyAsset(sourcePath, targetPath);
            if (!succuss)
            {
                Debug.LogError($"复制文件失败: {sourcePath}");
                return false;
            }

            AssetDatabase.Refresh();
            return true;
        }
        
        public static T CopyAndGetAsset<T>(string sourcePath, string targetPath) where T : Object
        {
            // 复制资源
            var success = CopyAssetFile(sourcePath, targetPath);
            if (!success)
            {
                Debug.LogError($"复制失败: {targetPath}");
                return null;
            }
        
            // 加载复制后的资源
            T copiedAsset = AssetDatabase.LoadAssetAtPath<T>(targetPath);
            if (copiedAsset == null)
            {
                Debug.LogError($"加载复制后的资源失败: {targetPath}");
            }
        
            return copiedAsset;
        }
    }
}