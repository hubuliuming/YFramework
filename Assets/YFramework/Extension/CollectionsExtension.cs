using System.Collections;
using UnityEngine;

namespace YFramework.Extension
{
    public static class CollectionsExtension
    {
        public static void StartCoroutineGlobal(this IEnumerator enumerator)
        {
            MonoGlobal.Instance.StartCoroutine(enumerator);
        }
        
        public static void StartCoroutine(this IEnumerator enumerator,MonoBehaviour mono)
        {
            mono.StartCoroutine(enumerator);
        }
    }
}