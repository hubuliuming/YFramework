/****************************************************
    文件：UnLoadResourcesExamples.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections;
using UnityEditor;
using UnityEngine;

namespace YFramework
{
    public class UnLoadResourcesExample : MonoBehaviour
    {
#if UNITY_EDITOR
        [MenuItem("YFramework/Examples/6.UnLoadResourcesExample", false, 6)]
        private static void MenuClick()
        {
            EditorApplication.isPlaying = true;
            new GameObject("UnLoadResourcesExample").AddComponent<UnLoadResourcesExample>();
        }
#endif
        private IEnumerator Start()
        {
            var prefab = Resources.Load<GameObject>("Home");
            yield return new WaitForSeconds(5f);
            prefab = null;
            //清楚未引用的资源,但是问题很大，会触发GC造成开单
            Resources.UnloadUnusedAssets();
            Debug.Log("is UnLoad");
        }
        
    }
}