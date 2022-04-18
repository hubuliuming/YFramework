using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace YFramework.UI
{
    public class SimpleSlideScroll : MonoBehaviour,IBeginDragHandler,IEndDragHandler
    {
        private RectTransform contentTrans;
        private float beginMousePosX;
        private float endMousPosX;
        private ScrollRect scrollRect;
        private Vector3 contentInitPos;
        private Vector2 contentInitSize;

        public int cellLength;
        public int spacing;
        public int leftOffset;
        private float moveOneItemLength;

        private Vector3 curContentLocalPos;
        public int totalItemNunm;
        public int currentIndex;

        public Button btnLast;
        public Button btnNext;
        public Text pageText;
        public TextMeshProUGUI pageTextPro;
        public bool needSendMessage;


        private void Start()
        {
            scrollRect = GetComponent<ScrollRect>();
            contentTrans = scrollRect.content;
            moveOneItemLength = cellLength + spacing;
            curContentLocalPos = contentTrans.localPosition;
            contentInitPos = contentTrans.localPosition;
            contentInitSize = contentTrans.sizeDelta;
            currentIndex = 0;
            if(pageText != null)
                pageText.text = currentIndex.ToString() + "/" + totalItemNunm;
            if (pageTextPro != null)
                pageTextPro.text = currentIndex.ToString() + "/" + totalItemNunm;
            if(btnLast != null)
                btnLast.onClick.AddListener(ToLastPage);
            if(btnNext != null)
                btnNext.onClick.AddListener(ToNextPage);
        
            UpdatePanel();
        }
        public void Init()
        {
            currentIndex = 0;
            if (contentTrans!=null)
            {
                contentTrans.localPosition = contentInitPos;
                curContentLocalPos = contentInitPos;
            }
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            endMousPosX = Input.mousePosition.x;
            float offectX = 0;
            float moveDistance = 0;
            offectX = beginMousePosX - endMousPosX;
            if (offectX > 0)//右滑
            {
                if (currentIndex >= totalItemNunm)
                {
                    return;
                }
                moveDistance = -moveOneItemLength;
                currentIndex++;
                if (needSendMessage)
                    UpdatePanel();
            }
            else //左滑
            {
                if (currentIndex <= 0)
                {
                    return;
                }
                moveDistance = moveOneItemLength;
                currentIndex--;
                if (needSendMessage)
                    UpdatePanel();
            }
            if (pageText != null)
                pageText.text = currentIndex.ToString() + "/" + totalItemNunm;
            if (pageTextPro != null)
                pageTextPro.text = currentIndex.ToString() + "/" + totalItemNunm;

            // DOTween.To(() => contentTrans.localPosition,
            //     lerpValue => contentTrans.localPosition = lerpValue,
            //     curContentLocalPos + new Vector3(moveDistance, 0, 0), 0.5f).SetEase(Ease.OutQuint);

            contentTrans.localPosition = curContentLocalPos + new Vector3(moveDistance, 0, 0);
            curContentLocalPos += new Vector3(moveDistance, 0, 0);
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            beginMousePosX = Input.mousePosition.x;
        }
        private void ToNextPage()
        {
            float moveDistance;
            if (currentIndex >= totalItemNunm)
                return;
            moveDistance = -moveOneItemLength;
            currentIndex++;
            if (pageText != null)
            {
                pageText.text = currentIndex.ToString() + "/" + totalItemNunm.ToString();
            }
            if (pageTextPro != null)
                pageTextPro.text = currentIndex.ToString() + "/" + totalItemNunm;
            if (needSendMessage)
                UpdatePanel();

            contentTrans.localPosition = curContentLocalPos + new Vector3(moveDistance, 0, 0);
            curContentLocalPos += new Vector3(moveDistance, 0, 0);
        }
    

        private void ToLastPage()
        {
            float moveDistance;
            if (currentIndex <= 0)
                return;
            moveDistance = moveOneItemLength;
            currentIndex--;
            if (pageText != null)
                pageText.text = currentIndex.ToString() + "/" + totalItemNunm.ToString();
            if (pageTextPro != null)
                pageTextPro.text = currentIndex.ToString() + "/" + totalItemNunm;
            if (needSendMessage)
                UpdatePanel();

            contentTrans.localPosition = curContentLocalPos + new Vector3(moveDistance, 0, 0);
            curContentLocalPos += new Vector3(moveDistance, 0, 0);
        }
        public void SetContentLength(int itemNum)
        {
            contentTrans.sizeDelta = new Vector2(contentTrans.sizeDelta.x +
                                                 (cellLength + spacing) * (itemNum - 1), contentTrans.sizeDelta.y);
            totalItemNunm = itemNum;
        }
        public void InitScrollLength()
        {
            contentTrans.sizeDelta = contentInitSize;
        }
    
        public void UpdatePanel(bool isNext)
        {
            if (isNext)
            {
                gameObject.SendMessageUpwards("ToNextLevel");
            }
            else
            {
                gameObject.SendMessageUpwards("ToLastLevel");
            }
        }

        private void UpdatePanel()
        {
            Debug.Log(currentIndex);
            //todo Method
        }

    }
}