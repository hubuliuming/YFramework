/**********************************************************************
* UI 会话基类
* 
* 在这个UI管理系统下
* 所有的UI面板都要挂载这个类
***********************************************************************/

using UnityEngine;

public abstract class DlgBase : MonoBehaviour {
    public virtual void OnCreate() {
    }

    public virtual void OnDestroy() {
    }
}