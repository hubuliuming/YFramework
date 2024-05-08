/****************************************************
    文件：BG3.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using YFramework.Extension;
using YFramework.UI;
using Texture2D = UnityEngine.Texture2D;

/// <summary>
/// 该功能需要导入YFramework，且只支持Horizontal
/// </summary>
public class ShowPictures : MonoBehaviour
{
    public Painting painting;
    public Transform parent;
    public GameObject gird;
    public SlideScrollHorizontal slideScrollHorizontal;
    public float pictrueSize ;
    public int rowNum;
    public int columnNum;
    private Texture2D _normalTex;
    private const int MaxShowPhotos = 36;
    private int _curPageShowNum;

    private Vector2 _slideScrollSizeDelta;
    private List<GameObject> photoGos = new List<GameObject>(MaxShowPhotos);

    private int _lastGoCount;
    private string _path;

    //保存参数
    private int _pictureWidth; 
    private int _pictureHeight;
    private void Start()
    {
        _path = painting.SavePicturePath;
        _pictureWidth = (int)(painting.CutPictureArea.sizeDelta.x * pictrueSize);
        _pictureHeight = (int)(painting.CutPictureArea.sizeDelta.y * pictrueSize);
        _curPageShowNum = rowNum * columnNum ;

        //设置脚本里的参数后再初始化
        var verticalScrollbarSizeDelta = slideScrollHorizontal.scrollRect.verticalScrollbar.GetComponent<RectTransform>().sizeDelta;
        var horizontalScrollbarSizeDelta = slideScrollHorizontal.scrollRect.horizontalScrollbar.GetComponent<RectTransform>().sizeDelta;
        var slideScrollTrans = slideScrollHorizontal.GetComponent<RectTransform>();
        var gridGroup = slideScrollHorizontal.scrollRect.content.GetOrAddComponent<GridLayoutGroup>();
        gridGroup.cellSize = new Vector2(_pictureWidth,_pictureHeight);
        slideScrollTrans.sizeDelta =
            new Vector2(gridGroup.cellSize.x * columnNum,gridGroup.cellSize.y * rowNum) 
            +new Vector2(horizontalScrollbarSizeDelta.y,verticalScrollbarSizeDelta.x);
        _slideScrollSizeDelta = slideScrollTrans.sizeDelta;
        slideScrollHorizontal.cellLength = (int) _slideScrollSizeDelta.x;
        slideScrollHorizontal.scrollRect.content.sizeDelta = _slideScrollSizeDelta;
        slideScrollHorizontal.Init();
        UpdateGridGo();
    }

    public void UpdateGridGo()
    {
        var nameList = new List<string>();
        var names = GetImagesNames(_path);
        var needInstantiateNum = MaxShowPhotos;
        if (names.Length < MaxShowPhotos)
        {
            needInstantiateNum = names.Length;
        }

        for (int i = 0; i < needInstantiateNum; i++)
        {
            nameList.Add(names[names.Length-1-i]);
        }
        
        var updateNum = nameList.Count - _lastGoCount;
        Debug.Log("updateNum:"+updateNum);
        Debug.Log("lastCount:"+_lastGoCount);
        //已存在预制体数量并且更新texture
        for (int i = 0; i < _lastGoCount; i++)
        {
            Texture2D texture2D = new Texture2D(_pictureWidth,_pictureHeight);
            texture2D.LoadImage(File.ReadAllBytes(nameList[i]));
            photoGos[i].GetComponent<RawImage>().texture = texture2D;
        }
        //不存在预制体生成
        for (int i = _lastGoCount; i < _lastGoCount + updateNum; i++)
        {
            var go = Instantiate(gird, parent);
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(_pictureWidth, _pictureHeight);
            Texture2D texture2D = new Texture2D(_pictureWidth, _pictureHeight);
            texture2D.LoadImage(File.ReadAllBytes(nameList[i]));
            go.GetComponent<RawImage>().texture = texture2D;
            photoGos.Add(go);
        }
        _lastGoCount = photoGos.Count;
        Debug.Log("photoGosCount:"+photoGos.Count);
        
        UpdateContextLenght(photoGos.Count);
    }
    
    private void UpdateContextLenght(int count)
    {
        var updatePage = (count -1) / _curPageShowNum;
        int max = _curPageShowNum -1;
        
        if (updatePage > max)
        {
            updatePage = max;
        }
        Debug.Log("需要延长的页数："+updatePage);
        Vector2 newSizeDelta = Vector2.zero;
        newSizeDelta = parent.GetComponent<RectTransform>().sizeDelta += new Vector2(_slideScrollSizeDelta.x * (updatePage -1) + 20, 0);//20数值为稍微偏移一点好最后全部显示
        _slideScrollSizeDelta = newSizeDelta;
        slideScrollHorizontal.UpdateTotal();
    }
    private string[] GetImagesNames(string path)
    {
        return Directory.GetFiles(path);
    }
}