/**********************************************************************
* 动画状态机事件Behaviour的实现和包装
* 挂载在动画状态机的状态上面
* 
* 此脚本为宿主状态开放出 4个事件供外界订阅
***********************************************************************/

using System;
using UnityEngine;

public class StateEvent : StateMachineBehaviour
{
    public new string name;

    public Action OnStateEnterEvent;
    public Action OnStateExitEvent;

    public Action OnStateMachineEnterEvent;
    public Action OnStateMachineExitEvent;

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        OnStateMachineEnterEvent?.Invoke();
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        OnStateMachineExitEvent?.Invoke();
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnStateEnterEvent?.Invoke();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnStateExitEvent?.Invoke();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
