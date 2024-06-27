/****************************************************
    文件：Test2.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


public class Test2 : MonoBehaviour
{
    private AudioSource source;
    private void Start()
    {
        source = gameObject.AddComponent<AudioSource>();

        var path = Application.streamingAssetsPath + "/麦田.wav";
        if (!File.Exists(path))
        {
            Debug.LogError("不存在音频，路径："+path);
        }
        else
        {
            StartCoroutine(LoadAudio(path,AudioType.WAV));
        }
        
        
    }

    private IEnumerator LoadAudio(string url,AudioType type)
    {
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, type);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        { 
            var clip =DownloadHandlerAudioClip.GetContent(request);
            source.clip = clip;
            source.Play();
           
        }
        else
        {
            Debug.Log("加载错误:"+url);
        }
    }

    private void Update()
    {
        
    }


    public void OnSelected(int index)
    {
        Debug.Log(index);
    }
    
}