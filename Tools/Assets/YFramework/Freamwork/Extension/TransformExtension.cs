/****************************************************
    文件：TransformExtension.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/10 17:16:37
    功能：transform操作的拓展
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace YFramework.Extension
{
    public static class TransformExtension
    {
        public static void SetPosX(this Transform trans, float target)
        {
            var pos = trans.position;
            pos.x = target;
            trans.position = pos;
        }
        public static void SetPosY(this Transform trans, float target)
        {
            var pos = trans.position;
            pos.y = target;
            trans.position = pos;
        }
        public static void SetPosZ(this Transform trans, float target)
        {
            var pos = trans.position;
            pos.z = target;
            trans.position = pos;
        }
        public static void SetPosX(this MonoBehaviour mono, float target) => SetPosX(mono.transform, target);
        public static void SetPosY(this MonoBehaviour mono, float target) => SetPosY(mono.transform, target);
        public static void SetPosZ(this MonoBehaviour mono, float target) => SetPosZ(mono.transform, target);

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

        public static void AddPosX(this Transform trans, float target)
        {
            var pos = trans.position;
            pos.x += target;
            trans.position = pos;
        }
        public static void AddPosY(this Transform trans, float target)
        {
            var pos = trans.position;
            pos.y += target;
            trans.position = pos;
        }
        public static void AddPosZ(this Transform trans, float target)
        {
            var pos = trans.position;
            pos.z += target;
            trans.position = pos;
        }
        public static void AddPosX(this MonoBehaviour mono, float target) => AddPosX(mono.transform, target);
        public static void AddPosY(this MonoBehaviour mono, float target) => AddPosY(mono.transform, target);
        public static void AddPosZ(this MonoBehaviour mono, float target) => AddPosZ(mono.transform, target);
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
                t = go.AddComponent<T>();
            }

            return t;
        }
        public static T GetOrAddComponent<T>(this Transform trans) where T: Component => GetOrAddComponent<T>(trans.gameObject);
        public static Transform[] GetActiveGameObjectsInChildren(this Transform trans)
        {
            List<Transform> activeGos = new List<Transform>();
            for (int i = 0; i < trans.childCount; i++)
            {
                if (trans.transform.GetChild(i).gameObject.activeSelf)
                {
                    activeGos.Add(trans.GetChild(i));
                }
            }

            return activeGos.ToArray();
        }
        public static GameObject[] GetActiveGameObjectsInChildren(this GameObject go)
        {
            var activeTrans =GetActiveGameObjectsInChildren(go.transform);
            var activeGos = new GameObject[activeTrans.Length];
            for (int i = 0; i < activeTrans.Length; i++)
            {
                activeGos[i] = activeTrans[i].gameObject;
            }
            return activeGos;
        }
      
        public static RectTransform GetRect(this Transform trans) => trans.GetComponent<RectTransform>();

        public static void LookAt2D(this Transform self, Vector3 target)
        {
            //计算目标方向
            Vector3 dir = target - self.position;
            //计算旋转角度
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            //设置旋转
            self.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}