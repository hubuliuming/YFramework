/****************************************************
    文件：UIExtension.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace YFramework
{
    public static class UIExtension 
    {
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
            switch (gridGroup.constraint)
            {
                case GridLayoutGroup.Constraint.FixedColumnCount :
                    xSize = gridGroup.constraintCount;
                    ySize = Mathf.CeilToInt(totalGirdNum / (float) rawNum);
                    break;
                case GridLayoutGroup.Constraint.FixedRowCount :
                    ySize = gridGroup.constraintCount;
                    xSize = Mathf.CeilToInt(totalGirdNum / (float) columnNum);
                    break;
                //混合的默认都是1不做任何改变
                case GridLayoutGroup.Constraint.Flexible :
                    xSize = 1;
                    ySize = 1;
                    break;
            }
            
            scrollRect.content.sizeDelta = new Vector2((gridGroup.cellSize.x + gridGroup.spacing.x) * xSize - gridGroup.spacing.x, 
                (gridGroup.cellSize.y + gridGroup.spacing.y) * ySize + gridGroup.spacing.y);
        }
    }
}