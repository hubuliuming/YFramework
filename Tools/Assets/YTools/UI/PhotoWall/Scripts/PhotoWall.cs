/****************************************************
    文件：PhotoWall.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using DG.Tweening;
using UnityEngine;

//todo 
public class PhotoWall : MonoBehaviour
{
    public RectTransform prefab;
    public Vector2 space = new Vector2(10, 10);
    public float maxScale = 50f;
    public float showScale = 5f;
    
    private Vector2 _fistPos;
    
    private void Start()
    {
        _fistPos = new Vector2(Screen.width + prefab.rect.width * 0.5f, -prefab.rect.height * 0.5f);
        DOTween.SetTweensCapacity(500,50);
        SpawnItem();
    }

    private void SpawnItem()
    {
        int row = Mathf.FloorToInt(Screen.height / (prefab.rect.height + space.y));
        int colum = Mathf.FloorToInt(Screen.width / (prefab.rect.width + space.x));

        int index = 0;
        for (int i = 0; i < row; i++)
        {
            var curX = _fistPos.x;
            var curY = _fistPos.y - i * (prefab.rect.height +space.y);
            for (int j = 0; j < colum; j++)
            {
                var item = Instantiate(prefab.gameObject, transform).GetComponent<RectTransform>();
                item.gameObject.AddComponent<PhotoWallItem>().Init(index,maxScale,showScale);
                //受到父物体的描点影响
                item.anchoredPosition = new Vector2(curX, curY);
                item.DOAnchorPosX(curX - Screen.width, 1);
                curX +=prefab.rect.width + space.x;
                index++;
            }
        }
    }
}