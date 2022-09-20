/****************************************************
    文件：AmplificationShow.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YFramework.Kit.UI
{
    /// <summary>
    ///  指定区域内自适应放大图片
    /// </summary>
    public class AdaptivityShow : MonoBehaviour,IPointerClickHandler
    {
        public RectTransform areaTrans;
        public Action onClose;
        public void OnPointerClick(PointerEventData eventData)
        {
            areaTrans.gameObject.SetActive(true);
            var go = Instantiate(gameObject, areaTrans);
            Destroy(go.GetComponent<AdaptivityShow>());
            onClose += () =>
            {
                areaTrans.gameObject.SetActive(false);
                Destroy(go);
            };
        
            //image
            var img = go.GetComponent<Image>();
            if (img != null)
            {
                img.SetNativeSize();
                SetTransSizeDelta(img.rectTransform);
            }
            else
            {
                var rawImg = go.GetComponent<RawImage>();
                rawImg.SetNativeSize();
                SetTransSizeDelta(rawImg.rectTransform);
            }

            go.AddComponent<Button>().onClick.AddListener(()=>
            {
                onClose?.Invoke();
            });
        }

        private void SetTransSizeDelta(RectTransform rectTrans)
        {
            Vector2 areaSize = areaTrans.sizeDelta;
            Vector2 rectSize = rectTrans.sizeDelta;
            //显示区域足够不做处理
            if (rectSize.x <= areaSize.x && rectSize.y <= areaSize.y)
            {
                return;
            }
            var offsetScale = 1f;
            // 宽度都未超出，所以是高度超出了按高度比例来计算
            if (rectSize.x < areaSize.x)
            {
                offsetScale = areaSize.y / rectSize.y;
                Debug.Log(offsetScale);
            }
            // 反之 高度未超出
            else if (rectSize.y < areaSize.y)
            {
                offsetScale = areaSize.x / rectSize.x;
            }
            //两者都超出
            else
            {
                var x =Mathf.Abs(areaSize.x - rectSize.x) / areaSize.x;
                var y =Mathf.Abs(areaSize.y - rectSize.y) / areaSize.y;
                //取最大比例值来适应框
                if (x >= y)
                {
                    offsetScale = areaSize.x / rectSize.x ;
                }
                else if (x < y)
                {
                    offsetScale = areaSize.y / rectSize.y ;
                }
            }
            rectTrans.sizeDelta *= offsetScale;
        }
    }
}