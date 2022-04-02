
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlideScrollView : MonoBehaviour,IBeginDragHandler,IEndDragHandler
{
    private RectTransform contentTrans;
    private float beginMousePosX;
    private float endMousPosX;
    private ScrollRect scrollRect;
    private Vector3 contentInitPos;
    private Vector2 contentInitSize;
    public Text pageText;

    public int cellLength;
    public int spacing;
    public int leftOffect;
    private float moveOneItemLength;

    private Vector3 curContentLocalPos;
    public int totalItemNunm;
    public int currentIndex;

    public bool needSendMessage;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        contentTrans = scrollRect.content;
        moveOneItemLength = cellLength + spacing;
        curContentLocalPos = contentTrans.localPosition;
        contentInitPos = contentTrans.localPosition;
        contentInitSize = contentTrans.sizeDelta;
        currentIndex = 1;
        if (pageText!=null)
        {
            pageText.text = currentIndex.ToString() + "/" + totalItemNunm;
        }
    }
    public void Init()
    {
        currentIndex = 1;
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
            if (needSendMessage)
                UpdatePanel(true);
            moveDistance = -moveOneItemLength;
            currentIndex++;
        }
        else //左滑
        {
            if (currentIndex <= 1)
            {
                return;
            }
            if (needSendMessage)
                UpdatePanel(false);
            moveDistance = moveOneItemLength;
            currentIndex--;
        }
        if (pageText != null)
        {
            pageText.text = currentIndex.ToString() + "/" + totalItemNunm;
        }
        // DOTween.To(() => contentTrans.localPosition,
        //     lerpValue => contentTrans.localPosition = lerpValue,
        //     curContentLocalPos + new Vector3(moveDistance, 0, 0), 0.5f).SetEase(Ease.OutQuint);
        Debug.Log("1");
        curContentLocalPos += new Vector3(moveDistance, 0, 0);
        Vector3.Lerp(contentTrans.localPosition, curContentLocalPos, 1);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginMousePosX = Input.mousePosition.x;
    }
    public void ToNextPage()
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
        if (needSendMessage)
            UpdatePanel(true);
        // DOTween.To(() => contentTrans.localPosition, lerpValue => contentTrans.localPosition = lerpValue,
        //     curContentLocalPos + new Vector3(moveDistance, 0, 0), 0.5f).SetEase(Ease.OutQuint);
        Debug.Log("1");
        curContentLocalPos += new Vector3(moveDistance, 0, 0);
        Vector3.Lerp(contentTrans.localPosition, curContentLocalPos, 1);
    }
    public void ToLastPage()
    {
        float moveDistance;
        if (currentIndex <= 1)
            return;
        moveDistance = moveOneItemLength;
        currentIndex--;
        if (pageText != null)
        {
            pageText.text = currentIndex.ToString() + "/" + totalItemNunm.ToString();
        }
        if (needSendMessage)
            UpdatePanel(false);
        // DOTween.To(() => contentTrans.localPosition, lerpValue => contentTrans.localPosition = lerpValue,
        //     curContentLocalPos + new Vector3(moveDistance, 0, 0), 0.5f).SetEase(Ease.OutQuint);
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

}
