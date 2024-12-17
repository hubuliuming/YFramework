/****************************************************
    文件：GameObjectExtension.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace YFramework.Extension
{
    public static class GameObjectExtension  
    {
        /// <summary>
        /// 在UnityEditor下需要使用这个，安卓平台真机下不需要使用会错误
        /// </summary>
        /// <param name="obj"></param>
        public static void ReSetShader(this GameObject obj)
        {
            var renderer = obj.GetComponent<Renderer>();
            renderer.material.shader = Shader.Find(renderer.material.shader.name);
        }

        /// <summary>
        /// 从指定Component添加到该物体上，采用反射获取各种属性值，属性值多的性能耗费较大谨慎使用
        /// </summary>
        /// <param name="goObj">需要添加的物体</param>
        /// <param name="comp">指定的component</param>
        /// <typeparam name="T">限制为Component</typeparam>
        public static T AddComponentFrom<T>(this GameObject goObj, T comp) where T : Component
        {
            var t = goObj.AddComponent<T>(); 
            t.GetCopyOf(comp);
            return t;
        }
    }
}