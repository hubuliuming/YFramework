/****************************************************
    文件：IOCExamples.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;

namespace YFramework.Examples
{
    public class IOCExample : MonoBehaviour 
    {
        private void Start()
        {
           var container = new IOCContainer();
           container.Register<IBluetoothManager>(new BluetoothManager());
           var bluetoothManager = container.Get<IBluetoothManager>();
           bluetoothManager.Connect();
        }
        
    }

    public interface IBluetoothManager
    {
        void Connect();
    }

    public class BluetoothManager : IBluetoothManager
    {
        public void Connect()
        {
            Debug.Log("蓝牙链接");
        }
    }
}