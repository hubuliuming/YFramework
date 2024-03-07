/****************************************************
    文件：College.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace YFramework.Kit.UI
{
    public class BtnGirdGroup : MonoBehaviour
    {
        private List<BtnGrid> _btnGrids = new List<BtnGrid>();
        public void Register(BtnGrid grid)
        {
            _btnGrids.Add(grid);
        }

        public void UnRegister(BtnGrid grid)
        {
            if (!_btnGrids.Contains(grid))
            {
                Debug.LogError("UnRegisterError");
                return;
            }
            _btnGrids.Remove(grid);
        }

        public void UpdateAllNormal()
        {
            foreach (var college in _btnGrids)
            {
                college.UpdateNormalImg();
            }
        }
        public void UpdateAllSelect()
        {
            foreach (var college in _btnGrids)
            {
                college.UpdateSelectImg();
            }
        }
    }
}