using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace YFramework.UI
{
    /// <summary>
    /// 挂载初始默认值的SlideScroll上，只修改宽高
    /// </summary>
    public class SlideScrollHorizontal : MonoBehaviour,IBeginDragHandler,IEndDragHandler
    {
        public ScrollRect scrollRect;
        public float cellLength;
        public float spacing;
        public int currentIndex;
        public int totalItemNum;

        public Button btnLast;
        public Button btnNext;
        public Text pageText;

        public bool needSendMessage;
        
        private RectTransform _contentTrans;
        private Vector3 _curContentLocalPos;
        private float _beginMousePosX;
        private float _endMousePosX;
        private float _moveOneItemLength;

        private Vector3 _contentInitPos;
        private Vector2 _contentInitSize;
        public void Init()
        {
            scrollRect.inertia = false;
            scrollRect.horizontal = true;
            scrollRect.vertical = false;
            _contentTrans = scrollRect.content;
            var girdGroup = _contentTrans.GetComponent<GridLayoutGroup>();
            if (girdGroup != null)
            {
                cellLength = girdGroup.cellSize.x;
                spacing = girdGroup.spacing.x;
            }
            _moveOneItemLength = cellLength + spacing;
            _curContentLocalPos = _contentTrans.localPosition;
            _contentInitPos = _contentTrans.localPosition;
            _contentInitSize = _contentTrans.sizeDelta;
            currentIndex = 0;
            //todo 验证全部情况的正确性
            UpdateTotal();
            if(pageText != null)
                pageText.text = currentIndex.ToString() + "/" + totalItemNum;
            if (btnLast != null)
            {
                btnLast.onClick.RemoveAllListeners();
                btnLast.onClick.AddListener(ToLastPage);
            }
            if (btnNext != null)
            {
                btnNext.onClick.RemoveAllListeners();
                btnNext.onClick.AddListener(ToNextPage);
            }
        }
        public void UpdateTotal()
        {
            totalItemNum = (int)(_contentTrans.sizeDelta.x / (cellLength+spacing)) +1;
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
            offectX = _beginMousePosX - _endMousePosX;
            if (offectX > 50)//右滑
            { 
                ToNextPage();
            }
            else if(offectX < -50) //左滑
            {
                ToLastPage();
            }
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
            if (needSendMessage)
                OnUpdatePage();
            _contentTrans.localPosition = _curContentLocalPos + new Vector3(moveDistance, 0, 0);
            _curContentLocalPos += new Vector3(moveDistance, 0, 0);
        }
        public void JumpToPage(int targetPage)
        {
            if (targetPage > totalItemNum || targetPage < 0)
            {
                Debug.LogWarning("跳转到的页面超过范围");
                return;
            }
            if (targetPage == currentIndex)
            {
                return;
            }
            while (targetPage != currentIndex)
            {
                if (targetPage > currentIndex)
                {
                    ToNextPage();
                }
                else if (targetPage < currentIndex)
                {
                    ToLastPage();
                }
            }
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
        
        public void OnUpdatePage()
        {
            Debug.Log(currentIndex);
        }
    }
}