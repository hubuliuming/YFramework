/****************************************************
    文件：RawImageAdaptivityLayout.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;
using YFramework.Extension;

namespace YFramework.UI.Layout
{
    [ExecuteAlways]
    [RequireComponent(typeof(RawImage))]
    public class RawImageAdaptivityLayout : YMonoBehaviour 
    {
        public RectTransform areaTrans;

        private void Update()
        {
            if (areaTrans)
            {
                GetComponent<RectTransform>().AdaptivitySize(areaTrans);
            }
        }
    }
}