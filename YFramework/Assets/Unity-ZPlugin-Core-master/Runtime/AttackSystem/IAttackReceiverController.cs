/**********************************************************************
* 攻击接收方控制器
***********************************************************************/
public interface IAttackReceiverController
{
    void OnAttackReceive(object attackMessage, AttackSender attackSender);
}