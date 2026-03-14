using System.Collections;
using System.Collections.Generic;
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
        
        public static void RemoveAtFast<T>(this List<T> list, int index)
        {
            int lastIndex = list.Count - 1;
            list[index] = list[lastIndex];
            list.RemoveAt(lastIndex);
        }
    }
}