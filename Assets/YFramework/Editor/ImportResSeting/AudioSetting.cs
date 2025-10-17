/****************************************************
    文件：AudioSetting.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace YFramework.Editor
{
#if UNITY_EDITOR
    public class AudioSetting : AssetPostprocessor
    {
        private static List<string> _curApplyPaths = new List<string>(100);
        AudioImporterSampleSettings  setting = new AudioImporterSampleSettings();

        
        private void OnPostprocessAudio(AudioClip arg)
        {
            if (_curApplyPaths.Contains(assetPath)) return;
            var importer = assetImporter as AudioImporter;
            //很短的音频
            if (arg.length >1f)
            {
                setting.loadType = AudioClipLoadType.DecompressOnLoad;
            }
            else
            {
                setting.loadType = AudioClipLoadType.Streaming;
            }

            importer.defaultSampleSettings = setting;
            _curApplyPaths.Add(assetPath);
        }
    }
#endif
}