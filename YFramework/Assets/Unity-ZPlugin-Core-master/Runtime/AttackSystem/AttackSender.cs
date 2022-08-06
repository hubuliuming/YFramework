/**********************************************************************
* 攻击发送者
* 受到 IAttackSenderController 管理, 
* 需要 Init
* 
* 攻击发送流程:
* 1.Init 注册时需要 IAttackSenderController 中的攻击开始事件和结束事件
* 2.在攻击进行阶段进行 OnTriggerStay 检测
* 3.触发到物体后进行筛选
*   3.1 物体是否在选定的层级中 
*   3.2 物体是否带有 AttackReceiver组件
*   3.3 物体在此次攻击间隔中是否已经被攻击过
* 4.触发 AttackReceiver 相应方法
* 5.AttackMessage 消息装箱并且传递
***********************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttackSender : MonoBehaviour {
    [SerializeField] LayerMask receiverLayer; //接收者层级
    [SerializeField] object attackMessage; //消息装箱

    bool attacking = false;
    List<AttackReceiver> AttackReceivers = new List<AttackReceiver>();

    public Action<AttackReceiver> OnAttackedReceiver;

    /// <summary>
    /// 注册方法,需要 IAttackSenderController
    /// </summary>
    /// <param name="receiverLayer"></param>
    /// <param name="attackMessage"></param>
    /// <param name="attackSender"></param>
    public void Init(LayerMask receiverLayer, object attackMessage, IAttackSenderController attackSender) {
        this.receiverLayer = receiverLayer;
        this.attackMessage = attackMessage;
        attackSender.OnAttackBeginEvent += BeginAttack;
        attackSender.OnAttackEndEvent += EndAttack;
    }

    private void BeginAttack() => attacking = true;

    private void EndAttack() {
        attacking = false;
        AttackReceivers.Clear();
    }

    private void OnTriggerStay(Collider other) {
        if (!attacking) {
            return;
        }

        if ((other.gameObject.layer & receiverLayer) == 0) {
            return;
        }

        if (!other.TryGetComponent<AttackReceiver>(out var attackReceiver)) {
            return;
        }

        if (AttackReceivers.Contains(attackReceiver)) {
            return;
        }

        OnAttackedReceiver?.Invoke(attackReceiver);
        attackReceiver.ReceiveAttack(attackMessage, this);
        AttackReceivers.Add(attackReceiver);
    }
}