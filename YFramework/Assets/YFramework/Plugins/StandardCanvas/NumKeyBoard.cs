/****************************************************
    文件：NumKeyBoard.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NumKeyBoard : MonoBehaviour
{
    public Text TxtShow;
    public Button BtnSure;
    public int NumMaxInput;
    public bool AllowFirstZero;
    public UnityEvent OnSure;
    private int _index = 0;

    private void Start()
    {
        BtnSure.onClick.AddListener(() =>
        {
            _index = 0;
            TxtShow.text = "";
            OnSure?.Invoke();
        });
    }

    public void OnClickSure(string str)
    {
        if (_index >= NumMaxInput)
            return;
        TxtShow.text += str;
        if (!AllowFirstZero && TxtShow.text == "0")
        {
            TxtShow.text = "";
            return;
        }
        _index++;
    }
}