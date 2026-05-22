
namespace YFramework.Interaction.PointerEvent
{
    //非UI的按钮响应事件
    public class ButtonEvent : BaseButton
    {
        protected override void Start()
        {
            objType = ObjType.ThreeD;
            base.Start();
        }
    }
}
