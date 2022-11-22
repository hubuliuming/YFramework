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
        private Socket _client;
        private Thread _thread;
        private byte[] _receiveBuffer;
        
        public string ReceivedStr;
        public Action<string> onReceived;

        public TcpClient(string ip,int port,int receiveBufferLength = 1024)
        {
            this._ip = ip;
            this._port = port;
            this._receiveBuffer = new byte[receiveBufferLength];
            this._thread = new Thread(Main);
            this._thread.IsBackground = true;
            this._thread.Start();
        }
        public void SendMessage(string msg)
        {
            if (_client == null)
            {
                return;
            }

            if (_client.Connected)
            {
               var sendBuffer = Encoding.UTF8.GetBytes(msg);
                _client.Send(sendBuffer, 0, sendBuffer.Length, SocketFlags.None);
            }
        }
        
        public void SendMessage(byte[] data)
        {
            if (_client == null)
            {
                return;
            }
            if (_client.Connected)
            {
                _client.Send(data, 0, data.Length, SocketFlags.None);
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
                _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _client.Connect(_ip, _port);
                Debug.Log("连接成功！");
                _client.BeginReceive(_receiveBuffer,0,_receiveBuffer.Length,SocketFlags.None,ReceivedCallBack,null);
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
                if (!_client.Connected)
                {
                    return;
                }

                var lenght = _client.EndReceive(ar);
                if (lenght > 0)
                {
                    ReceivedStr = Encoding.UTF8.GetString(_receiveBuffer);
                    Debug.Log("收到的信息为："+ReceivedStr+",长度为：" + lenght);
                    onReceived?.Invoke(ReceivedStr);
                }
                _client.BeginReceive(_receiveBuffer, 0 ,_receiveBuffer.Length, SocketFlags.None, ReceivedCallBack,null);
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

            if (_client != null)
            {
                _client.Close();
                _client = null;
            }

        }

    }
}