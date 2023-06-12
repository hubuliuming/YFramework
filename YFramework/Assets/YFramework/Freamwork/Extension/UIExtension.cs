/****************************************************
    文件：UIExtension.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace YFramework.Extension
{
    public static class UIExtension 
    {
        /// <summary>
        /// 更新ScrollRect下content的长度，注意content的锚点要默认的顶上对其
        /// </summary>
        /// <param name="scrollRect"></param>
        /// <param name="rowNum">展示的页面横向数量</param>
        /// <param name="columnNum">展示的页面纵向数量</param>
        /// <param name="totalGirdNum">content下所有gird的总数目</param>
        public static void UpdateContentLength(this ScrollRect scrollRect,int rowNum,int columnNum,int totalGirdNum)
        {
            var gridGroup = scrollRect.content.GetComponent<GridLayoutGroup>();
            if (gridGroup == null)
            {
                Debug.LogError("UpdateContentLength need require GridLayerGroup component!");
                return;
            }
            scrollRect.content.anchorMin = new Vector2(0, 1);
            scrollRect.content.anchorMax = new Vector2(0, 1);
            scrollRect.content.sizeDelta = Vector2.zero;
            scrollRect.content.localPosition =Vector3.zero;
            var xSize = 1;
            var ySize = 1;
            if (scrollRect.horizontal && !scrollRect.vertical)
            {
                xSize = Mathf.CeilToInt(totalGirdNum / (float) rowNum);
                ySize = rowNum;
            }
            else if (scrollRect.vertical && !scrollRect.horizontal)
            {
                xSize = columnNum;
                ySize = Mathf.CeilToInt(totalGirdNum / (float) columnNum);
            }
            else
            {
                xSize = Mathf.CeilToInt(totalGirdNum / (float) columnNum);
                ySize = Mathf.CeilToInt(totalGirdNum / (float) rowNum);
            }
            var realWith = scrollRect.content.rect.width;
            var expandWith = (gridGroup.cellSize.x + gridGroup.spacing.x) * xSize - gridGroup.spacing.x;
            var width = expandWith < realWith ? 0 : expandWith - realWith;
            var height =(gridGroup.cellSize.y + gridGroup.spacing.y) * ySize + gridGroup.spacing.y;
            scrollRect.content.sizeDelta = new Vector2(width, height);
        }

        /// <summary>
        /// 原比例在指定区域内放大
        /// </summary>
        /// <param name="rectTrans"></param>
        /// <param name="tartTrans"></param>
        public static void AdaptivitySize(this RectTransform rectTrans, RectTransform tartTrans)
        {
            var img = rectTrans.GetComponent<Image>();
            if (img != null)
            {
                img.SetNativeSize();
            }
            else
            {
                var rawImg = rectTrans.GetComponent<RawImage>();
                rawImg.SetNativeSize();
            }
            Vector2 areaSize = tartTrans.sizeDelta;
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

        /// <summary>
        /// 在UI中挂载HorizontalLayoutGroup的物体上使用该方法可以使得下面所有子物体宽度在该物体居中位置HorizontalLayoutGroup
        /// </summary>
        /// <param name="layoutGroup"></param>
        public static void SetCenterSize(this HorizontalLayoutGroup layoutGroup)
        {
            var sumWidth = 0;
            foreach (var t in layoutGroup.transform.GetActiveGameObjectsInChildren())
            {
                sumWidth += (int)t.GetComponent<RectTransform>().sizeDelta.x;
            }
            layoutGroup.padding.left = -sumWidth / 2 + (int)layoutGroup.transform.GetComponent<RectTransform>().sizeDelta.x/ 2;
        }
    }
}