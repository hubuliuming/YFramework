using System;
using System.Collections;
using System.Collections.Generic;
using MirrorOfWitch;
using UnityEngine;
using UnityEngine.Profiling;

public class Test1 : MonoBehaviour
{
    private GameObject go;
    private void Start()
    {
        AssetsLoadSystem.Instance.AALoadAssetAsync<GameObject>("Assets/AATools/AAObj.prefab", data =>
        {
            go = data;
            Debug.Log(go.name);
            Instantiate(go);
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            
        }
    }
}
