/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.UI;
using YFramework;
using YFramework.Event;
using YFramework.Extension;
using YFramework.Kit;

public class Test1 : YMonoBehaviour
{
    public GameObject target;
    public Image img;

    private ActionFixedUpdate _actionFixedUpdate;

    private List<Vector3> _list;

    private void Start()
    {
        Main();
        ushort i = 3;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(UIUtility.IsUI(FindObjectOfType<GraphicRaycaster>()));
        }
    
    }
    
    public static void Main()
    {
        List<int> a = new List<int> { 1, 3,4, 5, 7, 9 };
        int x = 6;
        var result = FindTwoNumbers(a, x);
        Debug.Log($"x 在 {result.Item1} 和 {result.Item2} 之间");
    }

    public static Tuple<int, int> FindTwoNumbers(List<int> a, int x)
    {
        a.Sort();
        int left = 0, right = a.Count - 1;

        // 使用二分查找定位 x 所在的两个数值之间的位置
        while (left <= right)
        {
            int mid = (left + right) / 2;
            if (a[mid] == x)
                return Tuple.Create(a[mid], a[mid]);
            else if (a[mid] < x)
                left = mid + 1;
            else
                right = mid - 1;
        }

        // 返回 x 所在的两个数值之间的位置
        int smaller = right >= 0 ? a[right] : int.MinValue;
        int larger = left < a.Count ? a[left] : int.MaxValue;

        return Tuple.Create(smaller, larger);
    }

  
    

}

