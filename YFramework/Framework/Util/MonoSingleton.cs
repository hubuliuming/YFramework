/****************************************************
    文件：MonoSingleton.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/19 9:40:13
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace YFramework
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T mInstance = null;

        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = FindObjectOfType<T>();
                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        Debug.LogWarning("Instance More than 1");
                        return mInstance;
                    }

                    if (mInstance == null)
                    {
                        var instanceName = typeof(T).Name;
                        Debug.Log("Instance Name:" + instanceName);
                        var instanceObj = GameObject.Find(instanceName);
                        if (!instanceObj)
                        {
                            instanceObj = new GameObject(instanceName);
                        }

                        mInstance = instanceObj.AddComponent<T>();
                        DontDestroyOnLoad(instanceObj);
                        Debug.LogFormat("Add new singleton: {0} in Game!", instanceName);
                    }
                    else
                    {
                        Debug.LogFormat("Already exist: {0} in Game!", mInstance.name);
                    }
                }

                return mInstance;
            }
        }

        private void OnDestroy()
        {
            mInstance = null;
        }
    }
}