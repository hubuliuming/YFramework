/**********************************************************************
* 攻击发送者控制器
***********************************************************************/
using System;
public interface IAttackSenderController
{
    Action OnAttackBeginEvent { get; set; }
    Action OnAttackEndEvent { get; set; }
}