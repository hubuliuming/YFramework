using System.Collections;

namespace YFramework.Extension
{
    public static class CollectionsExtension
    {
        public static void StartCoroutineGlobal(this IEnumerator enumerator)
        {
            MonoManager.Instance.StartCoroutine(enumerator);
        }
        
        public static void StartCoroutine(this IEnumerator enumerator,MonoManager mono)
        {
            mono.StartCoroutine(enumerator);
        }
    }
}