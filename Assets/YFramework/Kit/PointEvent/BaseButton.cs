
using UnityEngine.Events;
using UnityEngine.EventSystems;
using YFramework;
using YFramework.Event;

public class BaseButton : YMonoBehaviour,IPointerClickHandler
{
   public enum ObjType
   {
      /// <summary>
      /// 2D物体
      /// </summary>
      TwoD,
      /// <summary>
      /// 3D物体
      /// </summary>
      ThreeD,
   }

   protected ObjType objType;
   
   protected virtual void Start()
   {
      EventUtil.CheckRaycaster(objType);
   }
   /// <summary>
   /// Function definition for a button click event.
   /// </summary>
   public class ButtonClickedEvent : UnityEvent {}
   private ButtonEvent.ButtonClickedEvent _onClick = new ButtonEvent.ButtonClickedEvent();
   public ButtonEvent.ButtonClickedEvent onClick
   {
      get { return _onClick; }
      set { _onClick = value; }
   }
    
   public void OnPointerClick(PointerEventData eventData)
   {
      if(!IsActive()) return;
      onClick?.Invoke();
   }
}
