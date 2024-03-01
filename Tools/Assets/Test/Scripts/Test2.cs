/****************************************************
    文件：Test2.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test2 : MonoBehaviour , IPointerClickHandler
{
    public RectTransform ui;
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Dd");
        var v = ui.position - transform.position;
        Debug.Log(v);
        Debug.Log(v.normalized);
        transform.DOMove(ui.position, 1);
        //OnItemClicked();
    }
    
    
    
    
    private Rigidbody rb;
    private Vector3 backpackPosition; // 背包在3D世界中的位置  
    private float flightDuration = 1.0f; // 飞行持续时间  

    void Start()
    {
        Debug.Log(ui.anchoredPosition);
        Debug.Log(Camera.main.ScreenToWorldPoint(ui.anchoredPosition));
        rb = GetComponent<Rigidbody>();
        // 获取背包位置（这里假设你已经有了一个方法来获取背包的3D世界位置）  
        //backpackPosition = ui.position;
        backpackPosition = Camera.main.ScreenToWorldPoint(ui.anchoredPosition);
        
    }

    public void OnItemClicked()
    {
        StartCoroutine(FlyToBackpack());
    }

    private IEnumerator FlyToBackpack()
    {
        Vector3 flightDirection = (backpackPosition - transform.position).normalized;
        float flightDistance = Vector3.Distance(backpackPosition, transform.position);
        float flightSpeed = flightDistance / flightDuration;

        float elapsedTime = 0f;
        while (elapsedTime < flightDuration)
        {
            float t = elapsedTime / flightDuration;
            Vector3 currentPosition = Vector3.Lerp(transform.position, backpackPosition, t);
            rb.velocity = Vector3.zero; // 清除当前速度，避免叠加  
            rb.AddForce(flightDirection * flightSpeed * Time.deltaTime, ForceMode.Impulse);

            elapsedTime += Time.deltaTime;
            yield return null; // 等待下一帧  
        }

        // 到达背包位置，停止移动并处理其他逻辑（如更新UI）  
        rb.isKinematic = true; // 禁用物理模拟以精确放置道具  
        transform.position = backpackPosition;
        //UpdateBackpackUI(); // 更新背包UI界面  
    }

    private Vector3 GetBackpackPosition()
    {
        // 这里应该是获取背包在3D世界中的位置逻辑  
        // ...  
        return backpackPosition;
    }
}


public class ItemFlyToBackpack : MonoBehaviour
{
 
}