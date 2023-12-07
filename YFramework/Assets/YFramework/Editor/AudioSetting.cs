/****************************************************
    文件：AudioSetting.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEditor;
using UnityEngine;

namespace YFramework.Editor
{
#if UNITY_EDITOR
    public class AudioSetting : AssetPostprocessor
    {
        AudioImporterSampleSettings  setting = new AudioImporterSampleSettings();

        private void OnPostprocessAudio(AudioClip arg)
        {
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
        }
    }
#endif
}