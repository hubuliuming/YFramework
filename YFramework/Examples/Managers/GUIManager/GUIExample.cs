/****************************************************
    文件：GUIExample.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/17 10:0:56
    功能：GUI 示例
*****************************************************/

using UnityEditor;
using YFramework.Managers;

namespace YFramework
{
    public class GUIExample : YMonoBehaviour 
    {
#if UNITY_EDITOR
        [MenuItem("YFramework/Examples/Managers/GUIExample")]
        static void MenuClick()
        {
            EditorApplication.isPlaying = true;
            new UnityEngine.GameObject("GUIExample").AddComponent<GUIExample>();
        }
#endif
        private void Start()
        { 
            GUIManager.SetResolution(1280,750,0);
            GUIManager.LoadPanel("Panel",UILayers.Common);
            Delay(3,()=>GUIManager.UnLoadPanel("Panel"));
        }

        protected override void OnBeforeDestroy()
        {
        
        }
    }
}