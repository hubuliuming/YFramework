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
using YFramework.Extension;

namespace YFramework.Kit.UI
{
    /// <summary>
    ///  指定区域内自适应放大图片
    /// </summary>
    public class AdaptivityImageShow : MonoBehaviour,IPointerClickHandler
    {
        public RectTransform areaTrans;
        public Action onClose;
        public void OnPointerClick(PointerEventData eventData)
        {
            areaTrans.gameObject.SetActive(true);
            var go = Instantiate(gameObject, areaTrans);
            var trans = go.GetComponent<RectTransform>();
            trans.anchorMin = Vector2.one * 0.5f;
            trans.anchorMax = Vector2.one * 0.5f;
            Destroy(go.GetComponent<AdaptivityImageShow>());
            onClose += () =>
            {
                areaTrans.gameObject.SetActive(false);
                Destroy(go);
            };
            go.GetComponent<RectTransform>().AdaptivitySize(areaTrans);
            go.AddComponent<Button>().onClick.AddListener(()=>
            {
                onClose?.Invoke();
            });
        }
    }
}