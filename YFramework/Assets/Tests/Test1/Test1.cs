/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using YFramework;

public interface ITimerSystem
{
    float CurrentSeconds { get; }
    void AddDelayTask(float second, Action onDelayFinish);
}

public class  TimerSystem : ITimerSystem
{
    public float CurrentSeconds { get; }
    public void AddDelayTask(float second, Action onDelayFinish)
    {
        
    }
}

public class DelayTask
{
    public float Seconds { get; set; }
    public Action OnFinish { get; set; }
    
    public float StarSeconds { get; set; }
    public float FinishSeconds { get; set; }
    public DelayTaskState State { get; set; }
}

public enum DelayTaskState
{
    NotStart,
    Start
}

public class Test1 : YMonoBehaviour
{
    
}