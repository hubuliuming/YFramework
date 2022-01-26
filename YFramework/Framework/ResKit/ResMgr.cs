/****************************************************
    文件：ResMgr.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：负责全局资源缓存池
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace YFramework
{
    public class ResMgr : MonoSingleton<ResMgr>
    {
        public  List<Res> mShareLoadRes = new List<Res>();
#if UNITY_EDITOR
        private void OnGUI()
        {
            if (!Input.GetKey(KeyCode.F1)) return;
            GUILayout.BeginVertical("box");
            mShareLoadRes.ForEach(loader =>
            {
                GUILayout.Label("name:"+ loader.Name+"  refCount"+loader.RefCount);
            });
            GUILayout.EndVertical();
        }
#endif
    }
}