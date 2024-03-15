/****************************************************
    文件：BtnCollege.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YFramework.UI
{
    /// <summary>
    ///  相当于点击保持切换图片样式，只有点击同组的Image才更换选择状态图片
    /// </summary>
    public class BtnGrid : MonoBehaviour,IPointerClickHandler
    {
        public BtnGirdGroup gridGroup;
        public Sprite normalSpr;
        public Sprite selectSpr;
        private Image _img;
        public UnityEvent onClick;
        private void Start()
        {
            if (gridGroup == null)
            {
                Debug.LogError("Please add " + typeof(BtnGirdGroup));
            }
            _img = GetComponent<Image>();
            gridGroup.Register(this);
        }

        private void OnDestroy()
        {
            gridGroup.UnRegister(this);
        }
    
        public void OnPointerClick(PointerEventData eventData)
        {
            gridGroup.UpdateAllNormal();
            this.UpdateSelectImg();
            onClick?.Invoke();
        }

        public void UpdateNormalImg()
        {
            _img.sprite = normalSpr;
        }
        public void UpdateSelectImg()
        {
            _img.sprite = selectSpr;
        }

    }
}