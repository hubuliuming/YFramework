using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace YFramework.UI
{
    public class SimpleSlideScroll : UIBase,IBeginDragHandler,IEndDragHandler
    {
        public ScrollRect scrollRect;
        public int cellLength;
        public int spacing;
        public int leftOffset;
        public int currentIndex;
        public int totalItemNum;

        public Button btnLast;
        public Button btnNext;
        public Text pageText;
        public TextMeshProUGUI pageTextPro;

        public bool needSendMessage;

        private Vector3 _curContentLocalPos;
        private RectTransform _contentTrans;
        private float _beginMousePosX;
        private float _endMousePosX;
        private float _moveOneItemLength;
        
        private Vector3 _contentInitPos;
        private Vector2 _contentInitSize;
        
        public new void Init()
        {
            scrollRect.inertia = false;
            _contentTrans = scrollRect.content;
            _moveOneItemLength = cellLength + spacing;
            _curContentLocalPos = _contentTrans.localPosition;
            _contentInitPos = _contentTrans.localPosition;
            _contentInitSize = _contentTrans.sizeDelta;
            currentIndex = 0;
            UpdateTotal();
            if(pageText != null)
                pageText.text = currentIndex.ToString() + "/" + totalItemNum;
            if (pageTextPro != null)
                pageTextPro.text = currentIndex.ToString() + "/" + totalItemNum;
            if(btnLast != null)
                btnLast.onClick.AddListener(ToLastPage);
            if(btnNext != null)
                btnNext.onClick.AddListener(ToNextPage);
            
        }

        public void UpdateTotal()
        {
            totalItemNum = (int)(_contentTrans.sizeDelta.x / cellLength) + 1;
        }

        public void InitPos()
        {
            currentIndex = 0;
            if (_contentTrans!=null)
            {
                _contentTrans.localPosition = _contentInitPos;
                _curContentLocalPos = _contentInitPos;
            }
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            _endMousePosX = Input.mousePosition.x;
            float offectX = 0;
            float moveDistance = 0;
            offectX = _beginMousePosX - _endMousePosX;
            if (offectX > 0)//右滑
            {
                if (currentIndex >= totalItemNum)
                {
                    return;
                }
                moveDistance = -_moveOneItemLength;
                currentIndex++;
                if (needSendMessage)
                    OnUpdatePage();
            }
            else //左滑
            {
                if (currentIndex <= 0)
                {
                    return;
                }
                moveDistance = _moveOneItemLength;
                currentIndex--;
                if (needSendMessage)
                    OnUpdatePage();
            }
            if (pageText != null)
                pageText.text = currentIndex.ToString() + "/" + totalItemNum;
            if (pageTextPro != null)
                pageTextPro.text = currentIndex.ToString() + "/" + totalItemNum;
            
            _contentTrans.localPosition = _curContentLocalPos + new Vector3(moveDistance, 0, 0);
            _curContentLocalPos += new Vector3(moveDistance, 0, 0);
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            _beginMousePosX = Input.mousePosition.x;
        }
        private void ToNextPage()
        {
            float moveDistance;
            if (currentIndex >= totalItemNum)
                return;
            moveDistance = -_moveOneItemLength;
            currentIndex++;
            if (pageText != null)
            {
                pageText.text = currentIndex.ToString() + "/" + totalItemNum.ToString();
            }
            if (pageTextPro != null)
                pageTextPro.text = currentIndex.ToString() + "/" + totalItemNum;
            if (needSendMessage)
                OnUpdatePage();

            _contentTrans.localPosition = _curContentLocalPos + new Vector3(moveDistance, 0, 0);
            _curContentLocalPos += new Vector3(moveDistance, 0, 0);
        }
    

        private void ToLastPage()
        {
            float moveDistance;
            if (currentIndex <= 0)
                return;
            moveDistance = _moveOneItemLength;
            currentIndex--;
            if (pageText != null)
                pageText.text = currentIndex.ToString() + "/" + totalItemNum.ToString();
            if (pageTextPro != null)
                pageTextPro.text = currentIndex.ToString() + "/" + totalItemNum;
            if (needSendMessage)
                OnUpdatePage();

            _contentTrans.localPosition = _curContentLocalPos + new Vector3(moveDistance, 0, 0);
            _curContentLocalPos += new Vector3(moveDistance, 0, 0);
        }
        public void SetContentLength(int itemNum)
        {
            _contentTrans.sizeDelta = new Vector2(_contentTrans.sizeDelta.x +
                                                 (cellLength + spacing) * (itemNum - 1), _contentTrans.sizeDelta.y);
            totalItemNum = itemNum;
        }
        public void InitScrollLength()
        {
            _contentTrans.sizeDelta = _contentInitSize;
        }
    
        // public void UpdatePanel(bool isNext)
        // {
        //     if (isNext)
        //     {
        //         gameObject.SendMessageUpwards("ToNextLevel");
        //     }
        //     else
        //     {
        //         gameObject.SendMessageUpwards("ToLastLevel");
        //     }
        // }

        public void OnUpdatePage()
        {
            Debug.Log(currentIndex);
            MsgDispatcher.Send("OnUpdatePage",currentIndex);
        }

    }
}