/****************************************************
    文件：LoopList.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：自定义根据Item个数拓展content长度，实例化生成的item个数为可显示区域的个数+1，
         避免了大量生成Item，通过model里数据赋值切换元素，算法为content的height
         除以item的height超过或小于则item里的id头尾变化，再根据id更新位置
*****************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YFramework.UI.Kit
{
    /// <summary>
    /// 挂载在ScrollView拓展ScrollView下落框
    /// </summary>
    public class LoopList : UIBase
    {
        public GameObject item;
        public Sprite[] itemIcons;
        public float spaceY;
        private float _itemHeight;
        private ScrollRect _scrollRect;
        private List<LoopListItem> _items;
        private List<LoopListItemModel> _itemModels;

        private void Start()
        {
            _items = new List<LoopListItem>();
            _itemModels = new List<LoopListItemModel>();
            _scrollRect = GetComponent<ScrollRect>();
            _itemHeight = item.GetComponent<RectTransform>().rect.height;
            GetModel();
            int num = GetShowItemNum(_itemHeight);
            SpawnItem(num,item);
            SetContentSize();
            _scrollRect.onValueChanged.AddListener(OnValueChange);
        }
        
        private void SpawnItem(int num,GameObject prefab)
        {
            LoopListItem listItem = null;
            for (int i = 0; i < num; i++)
            {
                var go = Instantiate(prefab,_scrollRect.content);
                go.SetActive(true);
                listItem = go.AddComponent<LoopListItem>();
                listItem.AddGetDataListener(GetData);
                listItem.Init(i,_scrollRect.content,spaceY,num);
                _items.Add(listItem);
            }
        }

        private LoopListItemModel GetData(int index)
        {
            if (index< 0 || index >= _itemModels.Count)
            {
                return new LoopListItemModel();
            }
            return _itemModels[index];
        }
        //todo 修改其他方式获取
        private void GetModel()
        {
            int i = 0;
            foreach (var s in itemIcons)
            {
                var model = new LoopListItemModel();
                model.Icon = s;
                model.Info = i.ToString();
                i++;
                _itemModels.Add(model);
            }
        }
        private int GetShowItemNum(float itemHeight)
        {
            return Mathf.CeilToInt(rectTransform.rect.height / (itemHeight + spaceY)) +1;
        }

        private void SetContentSize()
        {
            var y = _itemModels.Count * _itemHeight + (_itemModels.Count - 1) * spaceY;
            _scrollRect.content.sizeDelta = new Vector2(_scrollRect.content.sizeDelta.x, y);
        }

        private void OnValueChange(Vector2 pos)
        {
            foreach (var listItem in _items)
            {
                listItem.OnValueChange();
            }
        }
    }
}