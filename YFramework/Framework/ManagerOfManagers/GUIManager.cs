/****************************************************
    文件：GUIManager.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/17 9:28:22
    功能：UI管理
*****************************************************/


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YFramework.Managers
{
    public enum UILayers
    {
        BG,
        Common,
        Top,
    }

    public class GUIManager
    {
        private static GameObject mUIRoot;

        public static GameObject UIRoot
        {
            get
            {
                if (mUIRoot == null)
                {
                    mUIRoot = Object.Instantiate(Resources.Load<GameObject>("UIRoot"));
                    mUIRoot.name = "UIRoot";
                }

                return mUIRoot;
            }
        }

        private static Dictionary<string, GameObject> mPanelDict = new Dictionary<string, GameObject>();

        public static void SetResolution(float width, float height, float matchWidthOrHeight)
        {
            var canvasScale = UIRoot.GetComponent<CanvasScaler>();
            canvasScale.referenceResolution = new Vector2(width, height);
            canvasScale.matchWidthOrHeight = matchWidthOrHeight;
        }

        public static GameObject LoadPanel(string prefabName, UILayers layers)
        {
            var prefab = Resources.Load<GameObject>(prefabName);
            var panel = Object.Instantiate(prefab);
            panel.name = prefabName;
            switch (layers)
            {
                case UILayers.BG:
                    panel.transform.SetParent(UIRoot.transform.Find("Bg"));
                    break;
                case UILayers.Common:
                    panel.transform.SetParent(UIRoot.transform.Find("Common"));
                    break;
                case UILayers.Top:
                    panel.transform.SetParent(UIRoot.transform.Find("Top"));
                    break;
            }

            var panelRectTrans = panel.transform as RectTransform;
            panelRectTrans.offsetMin = Vector2.zero;
            panelRectTrans.offsetMax = Vector2.zero;
            panelRectTrans.anchoredPosition3D = Vector3.zero;
            panelRectTrans.anchorMin = Vector2.zero;
            panelRectTrans.anchorMax = Vector2.one;

            mPanelDict.Add(panel.name, panel);
            return panel;
        }

        public static void UnLoadPanel(string name)
        {
            if (mPanelDict.ContainsKey(name))
            {
                Object.Destroy(mPanelDict[name]);
            }
        }
    }
}