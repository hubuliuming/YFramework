/**********************************************************************
* UI 管理器
* 挂载在Canvas下
* 
* 1.方便呼出和隐藏UI面板
* 2.UI面板挂载UIDlgBase
* 3.UI面板放在Resources文件夹相应目录下面
* 4.多实例使用 Show和 Hide 控制
* 5.单实例使用 Switch 控制
*   单实例依类名为键存储在字典中,根据情况调用 Show和 Hide
***********************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class DlgManager {
    public string ResourcePath = "Dlg/";

    Transform canvasTrans;
    Dictionary<Type, DlgBase> dlgDict;

    public DlgManager(Canvas canvas = null) {
        canvas = canvas ?? Object.FindObjectOfType<Canvas>();
        Debug.Assert(canvas == null, "canvas == null");
        this.canvasTrans = canvas.transform;
        dlgDict = new Dictionary<Type, DlgBase>();
    }

    #region 多实例Dlg

    /// <summary>
    /// 显示 Dlg
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Show<T>(Transform parent = null) where T : DlgBase {
        parent = parent ?? canvasTrans;
        T dlg = Resources.Load<T>(ResourcePath + typeof(T).Name);
        dlg.OnCreate();
        return Object.Instantiate(dlg, parent);
    }

    /// <summary>
    /// 隐藏(销毁) Dlg
    /// </summary>
    /// <param name="dlg"></param>
    public void Hide(DlgBase dlg) {
        dlg.OnDestroy();
        Object.Destroy(dlg.gameObject);
    }

    #endregion

    #region 单实例Dlg

    /// <summary>
    /// 切换 Dlg
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool Switch<T>() where T : DlgBase {
        if (dlgDict.TryGetValue(typeof(T), out var dlg)) {
            Hide(dlg);
            return false;
        } else {
            T t = Show<T>();
            dlgDict.Add(typeof(T), t);
            return true;
        }
    }

    /// <summary>
    /// 切换 Dlg
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public bool Switch<T>(out T t) where T : DlgBase {
        if (dlgDict.TryGetValue(typeof(T), out var dlg)) {
            Hide(dlg);
            t = null;
            return false;
        } else {
            t = Show<T>();
            dlgDict.Add(typeof(T), t);
            return true;
        }
    }

    /// <summary>
    /// 获取 Dlg
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public bool TryGet<T>(out T t) where T : DlgBase {
        if (dlgDict.TryGetValue(typeof(T), out var dlg)) {
            t = dlg as T;
            return true;
        } else {
            t = null;
            return false;
        }
    }

    #endregion
}