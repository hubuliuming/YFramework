/**********************************************************************
* Component 拓展方法
* 最常用 拓展脚本查找方法,
* 可以更方便的查找组件
***********************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZPlugin {
    /// <summary>
    /// 拓展脚本查找方法
    /// </summary>
    public static class ComponentExtension {
        /// <summary>
        /// 得到场景中的一个未隐藏的脚本实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static T GetComponentInScene<T>(this Component component, string name = null) where T : Component {
            return component.GetComponentInScene(typeof(T), name) as T;
        }

        /// <summary>
        /// 得到场景中的一个未隐藏的脚本实例
        /// </summary>
        /// <param name="component"></param>
        /// <param name="target"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Component GetComponentInScene(this Component component, Type target, string name = null) {
            UnityEngine.Object[] objects = GameObject.FindObjectsOfType(target);
            foreach (var obj in objects) {
                if (name == null || obj.name == name) {
                    return obj as Component;
                }
            }

            return null;
        }

        /// <summary>
        /// 根据 name 从后代物体中得到脚本
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target_type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Component GetComponentInChildren(this Component source, Type target_type, string name) {
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(source.transform);

            while (queue.Count != 0) {
                Transform trans_cur = queue.Dequeue();
                if (name == null || trans_cur.name == name) {
                    Component t = trans_cur.GetComponent(target_type);
                    if (t != null) {
                        return t;
                    }
                }

                for (int i = 0; i < trans_cur.childCount; i++) {
                    queue.Enqueue(trans_cur.GetChild(i));
                }
            }

            return null;
        }

        /// <summary>
        /// 根据 name 从后代物体中得到脚本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetComponentInChildren<T>(this Component source, string name) where T : Component {
            return (T) source.GetComponentInChildren(typeof(T), name);
        }

        /// <summary>
        /// 根据 name 从先祖物体中得到脚本
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Component GetComponentInParent(this Component source, Type target, string name) {
            for (Transform current = source.transform; current != null; current = current.parent)
                if (name == null || current.name == name)
                    return current.GetComponent(target);
            return null;
        }

        /// <summary>
        /// 根据 name 从先祖物体中得到脚本
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetComponentInParent<T>(this Component source, string name) where T : Component {
            return (T) source.GetComponentInParent(typeof(T), name);
        }
    }
}