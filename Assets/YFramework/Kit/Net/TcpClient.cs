/****************************************************
    文件：TcpClient.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：
*****************************************************/

using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace YFramework.Kit.Net
{
    public class TcpClient
    {
        private string _ip;
        private int _port;
        public Socket client;
        private Thread _thread;
        private byte[] _receiveBuffer;
        
        public string ReceivedStr { get; private set; }
        public Action<string> onReceived;

        public TcpClient(string ip,int port,int receiveBufferLength = 1024)
        {
            this._ip = ip;
            this._port = port;
            this._receiveBuffer = new byte[receiveBufferLength];
        }

        public void Start()
        {
            _thread = new Thread(Main);
            _thread.IsBackground = true;
            _thread.Start();
        }
        public void SendMessage(string msg)
        {
            if (client == null)
            {
                return;
            }

            if (client.Connected)
            {
               var sendBuffer = Encoding.UTF8.GetBytes(msg);
                client.Send(sendBuffer, 0, sendBuffer.Length, SocketFlags.None);
            }
        }
        
        public void SendMessage(byte[] data)
        {
            if (client == null)
            {
                return;
            }
            if (client.Connected)
            {
                client.Send(data, 0, data.Length, SocketFlags.None);
            }
        }
        public void SendMessageBy16Bite(string msg)
        {
            var data =Convert.Convert.Convert16Byte(msg);
            SendMessage(data);
        }


        private void Main()
        {
            try
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(_ip, _port);
                Debug.Log("连接成功！");
                client.BeginReceive(_receiveBuffer,0,_receiveBuffer.Length,SocketFlags.None,ReceivedCallBack,null);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
        private void ReceivedCallBack(IAsyncResult ar)
        {
            try
            {
                if (!client.Connected)
                {
                    return;
                }

                var lenght = client.EndReceive(ar);
                if (lenght > 0)
                {
                    ReceivedStr = Encoding.UTF8.GetString(_receiveBuffer,0,lenght);
                    Debug.Log("收到的信息为："+ReceivedStr+",长度为：" + lenght);
                    onReceived?.Invoke(ReceivedStr);
                }
                client.BeginReceive(_receiveBuffer, 0 ,_receiveBuffer.Length, SocketFlags.None, ReceivedCallBack,null);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
       
        }
        public void Close()
        {
            if (_thread != null)
            {
                _thread.Abort();
                _thread = null;
            }

            if (client != null)
            {
                client.Close();
                client = null;
            }

        }

    }
}