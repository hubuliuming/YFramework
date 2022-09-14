/****************************************************
    文件：NumKeyBoard.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class NumKeyBoard : MonoBehaviour
{
    public Text TxtShow;
    public Button BtnSure;
    public Button BtnClear;
    public int NumMaxInput;
    public bool AllowFirstZero;
    private int _index = 0;


    private void Start()
    {
        BtnSure.onClick.AddListener(() =>
        {
            //Clear();
        });
        BtnClear.onClick.AddListener(Clear);
    }

    public void NumClick(string str)
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

    private void Clear()
    {
        _index = 0;
        TxtShow.text = "";
    }
}