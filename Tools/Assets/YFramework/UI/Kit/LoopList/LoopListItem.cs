/****************************************************
    文件：LoopListItem.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


using System;
using UnityEngine;
using UnityEngine.UI;

namespace YFramework.UI.Kit
{
    public class LoopListItem : UIBase
    {
        private int _id;
        private Image m_image;

        public Image image
        {
            get
            {
                if (m_image == null)
                {
                    m_image = GetComponent<Image>();
                }
                return m_image;
            }
        }
        
        private Text m_info;

        public Text info
        {
            get
            {
                if (m_info == null)
                {
                    m_info = GetComponentInChildren<Text>();
                }
                return m_info;
            }
        }

        private LoopListItemModel _model;
        private Func<int, LoopListItemModel> _getData;
        private RectTransform _content;
        private float _spaceY;
        private int _showNum;

        internal void Init(int id,RectTransform content,float spaceY,int showNum)
        {
            _id = -1;
            _content = content;
            _spaceY = spaceY;
            _showNum = showNum;
            //pivot设定左上角方便位置计算
            rectTransform.pivot = new Vector2(0, 1);
            ChangeID(id);
        }

        internal void AddGetDataListener(Func<int, LoopListItemModel> data)
        {
            _getData = data;
        }

        internal void OnValueChange()
        {
            int startID, endID;
            UpdateIDRange(out startID,out endID);
            JudgeID(startID,endID);
        }
        
        private void UpdateIDRange(out int startID,out int endID)
        {
            startID = Mathf.FloorToInt(_content.anchoredPosition.y / (rectTransform.rect.height + _spaceY));
            endID = startID + _showNum - 1;
        }

        private void JudgeID(int startID,int endID)
        {
            //移动过快偏差值
            int offset = 0;
            if (_id < startID)
            {
                offset = startID - _id - 1;
                ChangeID(endID - offset);
            }
            else if (_id > endID)
            {
                offset = _id - endID - 1;
                ChangeID(startID + offset);
            }
        }
        
        private void ChangeID(int id)
        {
            if (_id != id && JudgeIDIsValid(id))
            {
                _id = id;
                _model = _getData(id);
                image.sprite = _model.Icon;
                info.text = _model.Info;
                SetPos();
            }
        }

        private void SetPos()
        {

            rectTransform.anchoredPosition = new Vector2(0, -_id * (rectTransform.rect.height + _spaceY));
        }

        private bool JudgeIDIsValid(int id)
        {
            return !_getData(id).Equals(new LoopListItemModel());
        }
    }
}