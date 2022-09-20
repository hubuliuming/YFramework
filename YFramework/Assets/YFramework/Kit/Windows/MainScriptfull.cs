/****************************************************
    文件：Server.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：填充整个屏幕
*****************************************************/

using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace YFramework.Kit.Windows
{
    public class MainScriptfull : MonoBehaviour
    {
        // Start is called before the first frame update
        public Rect screenPosition;

        [DllImport("user32.dll")]
        static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();

        private const uint SWP_SHOWWINDOW = 0x0040;
        private const int GWL_STYLE = -16;
        private const int WS_BORDER = 1;
        private int i = 0;
        private void Awake()
        {
            Screen.SetResolution(Screen.width, Screen.height, false);
        }
        // 初始化串口
        private void Start()
        {
            // Cursor.visible = false; //鼠标隐藏
            SetWindowLong(GetActiveWindow(), GWL_STYLE, WS_BORDER);
            SetWindowPos(GetActiveWindow(), -1, (int)screenPosition.x, (int)screenPosition.y, (int)screenPosition.width, (int)screenPosition.height, SWP_SHOWWINDOW);
        }
    
        void Update()
        {
            if (i < 5)
            {
                i++;
                SetWindowLong(GetActiveWindow(), GWL_STYLE, WS_BORDER);
                SetWindowPos(GetActiveWindow(), -1, (int)screenPosition.x, (int)screenPosition.y, (int)screenPosition.width, (int)screenPosition.height, SWP_SHOWWINDOW);
            }
        }
        public void Quit()
        {
            Application.Quit();
        }
    }
}