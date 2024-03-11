/****************************************************
    文件：RotaionDraman2D.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace YFramework.UI.Kit
{
    //需要轮播动画导入ToTween插件有现成的，在RotationDiagram2DItem.SetItemData()方法里
    public class RotationDiagram2D : MonoBehaviour
    {
        public Vector2 ItemSize;
        public Sprite[] ItemSprites;

        public float Offset;
        [Tooltip("缩放比例最大值，与最小值差值越大越明显")]
        public float ScaleTimesMax = 1;
        [Tooltip("缩放比例最小值，与最大值差值越大越明显")]
        public float ScaleTimesMin = 0.2f;

        private List<RotationDiagram2DItem> _items;
        private List<ItemPosData> _itemPosDatas;

        private void Start()
        {
            _items = new List<RotationDiagram2DItem>();
            _itemPosDatas = new List<ItemPosData>();
            CreateItem();
            CalculateData();
            SetItemData();
        }

        private GameObject CreateTemplate()
        {
            var itemGo = new GameObject("Item");
            itemGo.AddComponent<RectTransform>().sizeDelta = ItemSize;
            itemGo.AddComponent<Image>();
            itemGo.AddComponent<RotationDiagram2DItem>();
            return itemGo;
        }

        private void CreateItem()
        {
            var template = CreateTemplate();

            foreach (var sprite in ItemSprites)
            {
                var item = Instantiate(template).GetComponent<RotationDiagram2DItem>();
                item.SetParent(transform);
                item.SetSprite(sprite);
                item.AddMoveListener(Change);
                _items.Add(item);
            }
            Destroy(template);
        }

        private void CalculateData()
        {
            var itemDatas = new List<ItemData>();
            var length = (ItemSize.x + Offset) * _items.Count;
            var ratioOffset = 1 / (float) _items.Count;
            var ratio = 0f;
            for (int i = 0; i < _items.Count; i++)
            {
                var itemData = new ItemData();
                itemData.PosId = i;
                itemDatas.Add(itemData);
                _items[i].posId = i;

                var posData = new ItemPosData();
                posData.X = GetX(ratio, length);
                posData.ScaleTimes = GetScaleTimes(ratio, ScaleTimesMax, ScaleTimesMin);
                ratio += ratioOffset;
                _itemPosDatas.Add(posData);
            }

            itemDatas = itemDatas.OrderBy(u => _itemPosDatas[u.PosId].ScaleTimes).ToList();
            for (int i = 0; i < itemDatas.Count; i++)
            {
                _itemPosDatas[itemDatas[i].PosId].Order = i;
            }
           
        }

        private void Change(float offsetX)
        {
            int symbol = offsetX > 0 ? 1 : -1;
            Change(symbol);
        }
        private void Change(int symbol)
        {
            foreach (var item in _items)
            {
                item.SetPosID(symbol,_items.Count);
            }
            SetItemData();
        }

        private void SetItemData()
        {
            //更新一下位置数据
            for (int i = 0; i < _itemPosDatas.Count; i++)
            {
                _items[i].SetItemData(_itemPosDatas[_items[i].posId]);
            } 
        }
        private float GetX(float radio, float lenght)
        {
            if (radio > 1 || radio < 0)
            {
                Debug.LogError("当前比例必须是0-1的值");
                return 0;
            }

            if (radio >= 0 && radio <0.25f)
            {
                return lenght * radio;
            }
            else if (radio >= 0.25 && radio < 0.75f)
            {
                return lenght * (0.5f - radio);
            }
            else
            {
                return lenght * (radio - 1);
            }
        }

        private float GetScaleTimes(float radio, float max,float min)
        {
            if (radio > 1 || radio < 0)
            {
                Debug.LogError("当前比例必须是0-1的值");
                return 0;
            }
            //比率
            var scaleOffset = (max - min) / 2f;
            if (radio < 0.5f)
            {
                return max - scaleOffset * radio;
            }
            else
            {
                return max - scaleOffset * (1 - radio);
            }
        }
    }

    public class ItemPosData
    {
        public float X;
        public float ScaleTimes;
        public int Order;
    }

    public struct ItemData
    {
        public int PosId;
    }
}