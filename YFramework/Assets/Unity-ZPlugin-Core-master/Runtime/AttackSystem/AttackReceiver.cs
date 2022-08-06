/**********************************************************************
* 攻击接收者
* 一般受到  IAttackReceiverController 管理
* 需要 Init
* 
* 攻击接收流程:
* 1.注册时需要有 IAttackReceiverController 的相应 Action
* 2.受到攻击时调用 ReceiveAttack 并获取相应信息
* 3.ReceiveAttack 调用 IAttackReceiverController 的 OnAttackReceive
* 4.OnAttackReceive 一般会调用 相应的事件表示受到攻击的事件触发了
***********************************************************************/

using System;
using UnityEngine;
public class AttackReceiver : MonoBehaviour
{
    Action<object, AttackSender> OnAttackReceiveEvent;

    public void Init(IAttackReceiverController attackReceiverController)
    {
        OnAttackReceiveEvent += attackReceiverController.OnAttackReceive;
    }

    internal void ReceiveAttack(object attackMessage, AttackSender attackSender)
    {
        OnAttackReceiveEvent?.Invoke(attackMessage, attackSender);
    }
}
