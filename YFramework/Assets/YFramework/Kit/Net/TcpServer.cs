/****************************************************
    文件：TcpServer.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：简单一对多服务器Unity模板
*****************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace YFramework.Kit.Net
{
    //todo SendMessage
    public class TcpServer
    {
        private string _ip;
        private int _port;

        public Socket server;
        private Thread _thread;
        private byte[] _receiveBuffer;
        public Action<string> onReceved;
        public string ReceiveStr { get; private set; }
        
        public TcpServer(string ip,int port,int receiveBufferLength = 1024)
        {
            this._ip = ip;
            this._port = port;
            this._receiveBuffer = new byte[receiveBufferLength];
        }

        public void Start()
        {
            _thread = new Thread(Init);
            _thread.IsBackground = true;
            _thread.Start();
        }
        private void Init()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Parse(_ip),_port));
            server.Listen(0);
            server.BeginAccept(AcceptCallBack,server);
        }
        
        private void AcceptCallBack(IAsyncResult ar)
        {
            Socket serverSocket = ar.AsyncState as Socket;
            Socket clientSocket = serverSocket.EndAccept(ar);
            Debug.Log("连接客户端成功");
            clientSocket.BeginReceive(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, ReceiveCallBack, clientSocket);
            
            serverSocket.BeginAccept(AcceptCallBack, serverSocket);
        }
        private void ReceiveCallBack(IAsyncResult ar)
        {
            Socket clientSocket = null;
            try
            {
                clientSocket = ar.AsyncState as Socket;
                int length = clientSocket.EndReceive(ar);
                if (length <= 0)
                {
                    clientSocket.Close();
                    return;
                }
                ReceiveStr = Encoding.UTF8.GetString(_receiveBuffer, 0, length);
                Debug.Log("收到消息:"+ReceiveStr+",长度为："+length);
                onReceved?.Invoke(ReceiveStr);
                clientSocket.BeginReceive(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, ReceiveCallBack, clientSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (clientSocket != null)
                {
                    clientSocket.Close();
                }
            }
        }
     
        public void Close()
        {
            if (server != null)
            {
                server.Close();
                server = null;
            }

            if (_thread != null)
            {
                _thread.Abort();
                _thread = null;
            }
        }
    }
    
    
    [Obsolete("该类无效",true)]
    public class EncodeTool
    {
        /// <summary>
        /// 封包
        /// </summary>
        public static byte[] EncodePacket(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(data.Length);
            bw.Write(data);
            byte[] packet = new byte[data.Length];
            Buffer.BlockCopy(ms.GetBuffer(),0,packet,0,(int)ms.Length);
            bw.Close();
            ms.Close();
            Console.WriteLine(packet.Length);
            return packet;
        }
        /// <summary>
        /// 解包
        /// </summary>
        public static byte[] DecodePacket(ref List<byte> cache)
        {
            if (cache.Count < 4) return null;
            MemoryStream ms = new MemoryStream(cache.ToArray());
            BinaryReader br = new BinaryReader(ms);
            int lenght = br.ReadInt32();
            int remainLenght = (int) (ms.Length - ms.Position);
            if (lenght > remainLenght) return null;
            byte[] data = br.ReadBytes(lenght);
            //更新数据
            cache.Clear();
            int remainLenghtAgain =  (int) (ms.Length - ms.Position);
            cache.AddRange(br.ReadBytes(remainLenghtAgain));
            br.Close();
            ms.Close();
            return data;
        }
    }
}