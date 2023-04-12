/****************************************************
    文件：CoroutineManager.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.Internal;

namespace YFramework.Kit.Managers
{
    internal class CoroutineManager : YMonoBehaviour
    {
        private static CoroutineManager _instance;
        public static CoroutineManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject(nameof(CoroutineManager));
                    var t = go.AddComponent<CoroutineManager>();
                    _instance = t;
                }
                return _instance;
            }
        }
    }


    public class CoroutineKit
    {
        /// <summary>
        ///   <para>Starts a coroutine named methodName.</para>
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="value"></param>
        [ExcludeFromDocs]
        public static Coroutine StartCoroutine(string methodName)
        {
            object obj = (object) null;
            return StartCoroutine(methodName, obj);
        }
        public static Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
        {
            return CoroutineManager.Instance.StartCoroutine(methodName, value);
        }
        public static Coroutine StartCoroutine(IEnumerator routine)
        {
            return CoroutineManager.Instance.StartCoroutine(routine);
        }

        public static void StopCoroutine(IEnumerator routine)
        {
            CoroutineManager.Instance.StopCoroutine(routine);
        }


        public static void StopCoroutine(Coroutine routine)
        {
            CoroutineManager.Instance.StopCoroutine(routine);
        }

        public static void StopCoroutine(string methodName)
        {
            CoroutineManager.Instance.StopCoroutine(methodName);
        }

        public static void StopAllCoroutines()
        {
            CoroutineManager.Instance.StopAllCoroutines();
        }
    }
}