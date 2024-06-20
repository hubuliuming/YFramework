
using UnityEngine;
using UnityEngine.UI;

public class ScrollTextTipControll : MonoBehaviour
{
    public RectTransform TextTips;
    public Button BtnClose;
    public float scrollSpeed = 65f;
    private Vector2 _initialPosition;
    private float _maxLength;

    void Start()
    {
        // 记录初始位置
        _initialPosition = TextTips.anchoredPosition;
        _maxLength = TextTips.sizeDelta.x;
        BtnClose.onClick.AddListener(()=> gameObject.SetActive(false));
    }

    private void Update()
    {
        TextTips.anchoredPosition -= new Vector2(scrollSpeed * Time.deltaTime, 0);
			
        // 如果文本滚动到一定位置，则重置到初始位置
        if (TextTips.anchoredPosition.x <= _initialPosition.x - _maxLength)
        {
            TextTips.anchoredPosition = _initialPosition;
        }
    }
}
