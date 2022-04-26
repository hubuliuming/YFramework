/****************************************************
    文件：TransformExtension.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/10 17:16:37
    功能：transform操作的拓展
*****************************************************/

using UnityEngine;

namespace YFramework
{
    public static  class TransformExtension
    {
        public static void SetLocalPosX(this Transform trans, float target)
        {
            var localPos = trans.localPosition;
            localPos.x = target;
            trans.localPosition = localPos;
        }
        public static void SetLocalPosY(this Transform trans, float target)
        {
            var localPos = trans.localPosition;
            localPos.y = target;
            trans.localPosition = localPos;
        }
        public static void SetLocalPosZ(this Transform trans, float target)
        {
            var localPos = trans.localPosition;
            localPos.z = target;
            trans.localPosition = localPos;
        }
        public static void SetIdentity(this Transform trans)
        {
            trans.localPosition = Vector3.zero;
            trans.localScale = Vector3.one;
            trans.localRotation = Quaternion.identity;
        }
        public static void SetLocalPosX(this MonoBehaviour mono, float target) => SetLocalPosX(mono.transform,target);
        public static void SetLocalPosY(this MonoBehaviour mono, float target) => SetLocalPosY(mono.transform, target);
        public static void SetLocalPosZ(this MonoBehaviour mono, float target) => SetLocalPosZ(mono.transform, target);
        public static void SetIdentity(this MonoBehaviour mono ) => SetIdentity(mono.transform);

        public static void AddLocalPosX(this Transform trans, float target)
        {
            var localPos = trans.localPosition;
            localPos.x += target;
            trans.localPosition = localPos;
        }
        public static void AddLocalPosY(this Transform trans, float target)
        {
            var localPos = trans.localPosition;
            localPos.y += target;
            trans.localPosition = localPos;
        }
        public static void AddLocalPosZ(this Transform trans, float target)
        {
            var localPos = trans.localPosition;
            localPos.z += target;
            trans.localPosition = localPos;
        }
        public static void AddLocalPosX(this MonoBehaviour mono, float target) => AddLocalPosX(mono.transform,target);
        public static void AddLocalPosY(this MonoBehaviour mono, float target) => AddLocalPosY(mono.transform,target);
        public static void AddLocalPosZ(this MonoBehaviour mono, float target) => AddLocalPosZ(mono.transform,target);
        public static T GetOrAddComponent<T>(this GameObject go) where T: Component
        {
            T t = go.GetComponent<T>();
            if (t == null)
            {
                t =go.AddComponent<T>();
            }

            return t;
        }
        public static T GetOrAddComponent<T>(this Transform trans) where T: Component => GetOrAddComponent<T>(trans.gameObject);
        public static RectTransform GetRect(this Transform trans) => trans.GetComponent<RectTransform>();
    }
}