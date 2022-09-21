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
        /// <param name="rawNum">展示的页面横向数量</param>
        /// <param name="columnNum">展示的页面纵向数量</param>
        /// <param name="totalGirdNum">content下所有gird的总数目</param>
        public static void UpdateContentLength(this ScrollRect scrollRect,int rawNum,int columnNum,int totalGirdNum)
        {
            var gridGroup = scrollRect.content.GetComponent<GridLayoutGroup>();
            if (gridGroup == null)
            {
                Debug.LogError("UpdateContentLength need require GridLayerGroup component!");
                return;
            }
            var xSize = 1;
            var ySize = 1;
            if (scrollRect.horizontal && !scrollRect.vertical)
            {
                xSize = Mathf.CeilToInt(totalGirdNum / (float) rawNum);
                ySize = rawNum;
            }
            else if (scrollRect.vertical && !scrollRect.horizontal)
            {
                xSize = columnNum;
                ySize = Mathf.CeilToInt(totalGirdNum / (float) columnNum);
            }
            else
            {
                xSize = Mathf.CeilToInt(totalGirdNum / (float) columnNum);
                ySize = Mathf.CeilToInt(totalGirdNum / (float) rawNum);
            }
            var realWith = scrollRect.content.rect.width;
            var expandWith = (gridGroup.cellSize.x + gridGroup.spacing.x) * xSize - gridGroup.spacing.x;
            var width = expandWith < realWith ? 0 : expandWith - realWith;
            var height =(gridGroup.cellSize.y + gridGroup.spacing.y) * ySize + gridGroup.spacing.y;
            scrollRect.content.sizeDelta = new Vector2(width, height);
            
        }
    }
}