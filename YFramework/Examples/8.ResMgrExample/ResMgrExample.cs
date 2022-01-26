/****************************************************
    文件：ResMgrExample.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections;
using UnityEngine;

namespace YFramework
{
    public class ResMgrExample : MonoBehaviour 
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem("YFramework/Examples/8.ResMgrExample", false, 8)]
        private static void MenuClick()
        {
            UnityEditor.EditorApplication.isPlaying = true;
            new GameObject("ResMgrExample").AddComponent<ResMgrExample>();
        }
#endif
        private ResLoader resLoader = new ResLoader();
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(3f);
            resLoader.LoadAsync<GameObject>("Panel",panel =>
            {
                Debug.Log(panel.name);
                Debug.Log(Time.time);
            });
            Debug.Log(Time.time);
            yield return new WaitForSeconds(5f);
            resLoader.ReleaseResAll();//虽然能卸载prefabs，但目前还有许多问题
        }
    }
}