/****************************************************
    文件：UDPClient.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace YFramework.Kit.Net
{
    public class UDPClient
    {
        private string _ip;
        private int _port;

        private Socket _localSocket;
        private Thread _localThread;
        private byte[] _receiveBuffer;

        public string ReceivedStr;
        public Action<string> OnReceived;
        
        private Socket _remoteSocket;
        public UDPClient(string ip, int port,int receiveBufferLength = 1024)
        {
            this._ip = ip;
            this._port = port;
            this._receiveBuffer = new byte[receiveBufferLength];
            this. _localSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this._localThread = new Thread(LocalStart);
            this._localThread.IsBackground = true;
            this._localThread.Start();
            this. _remoteSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        private void LocalStart()
        {
            try
            {
                _localSocket.Bind(new IPEndPoint(IPAddress.Parse(_ip),_port));
                _localSocket.BeginReceive(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, ReceivedCallBack, null);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        public void SendMessage(string ip,int port,string msg)
        {
            if (_remoteSocket == null)
            {
                return;
            }
            try
            {
                _remoteSocket.Connect(new IPEndPoint(IPAddress.Parse(ip),port));  
                var sendBuffer = Encoding.UTF8.GetBytes(msg);
                _remoteSocket.Send(sendBuffer, 0, sendBuffer.Length, SocketFlags.None);
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
                if (_localSocket == null)
                {
                    return;
                }
                var lenght = _localSocket.EndReceive(ar);
                if (lenght > 0)
                {
                    ReceivedStr = Encoding.UTF8.GetString(_receiveBuffer);
                    OnReceived?.Invoke(ReceivedStr);
                    Debug.Log("收到的信息为：" + ReceivedStr + ",长度为：" + lenght);
                }

                _localSocket.BeginReceive(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, ReceivedCallBack, null);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        public void Close()
        {
            if (_localThread != null)
            {
                _localThread.Abort();
                _localThread = null;
            }
            if (_localSocket != null)
            {
                _localSocket.Close();
                _localSocket = null;
            }

            if (_remoteSocket != null)
            {
                _remoteSocket.Close();
                _remoteSocket = null;
            }
        }
    }
}