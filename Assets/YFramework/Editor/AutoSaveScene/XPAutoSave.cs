using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System;

namespace YFramework.Editor
{
    [InitializeOnLoad]
    public class XPAutoSave
    {
        public static Scene nowScene;
        public static DateTime lastSaveTime = DateTime.Now;

        static XPAutoSave()
        {
            lastSaveTime = DateTime.Now;
            EditorApplication.update += EditorUpdate;
        }

        ~XPAutoSave()
        {
            EditorApplication.update -= EditorUpdate;
        }

        static void EditorUpdate()
        {
            if (AutoSaveWindow.autoSaveScene)
            {
                double seconds = (DateTime.Now - lastSaveTime).TotalSeconds;
                if (seconds > AutoSaveWindow.intervalTime)
                {
                    SaveScene();
                    lastSaveTime = DateTime.Now;
                }
            }
        }

        private static void SaveScene()
        {
            if (Application.isPlaying) return;
            nowScene = EditorSceneManager.GetActiveScene();
            EditorSceneManager.SaveScene(nowScene);
            if (AutoSaveWindow.showMessage)
            {
                Debug.Log("curScenePath:" + nowScene.path + "  " + lastSaveTime);
            }
        }
    }
}