/****************************************************
    文件：MonoGloable.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace YFramework
{
    /// <summary>
    /// 全局唯一共享MonoBehaviour
    /// </summary>
    public class MonoGlobal : YMonoBehaviour
    {
        private static MonoGlobal _instance;
        public static MonoGlobal Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject(nameof(MonoGlobal));
                    var t = go.AddComponent<MonoGlobal>();
                    _instance = t;
                }
                return _instance;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        
    }
}