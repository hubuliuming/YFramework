/****************************************************
    文件：TcpServer.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：简单一对多服务器Unity模板
*****************************************************/

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace YFramework
{
    public class TcpServer : MonoBehaviour
    {
        public string ip;
        public int port;
        private string submitStr;
        private bool isSubmit;
        private Socket server;
        private Thread thread;

        private byte[] receiveBuffer = new byte[1024];
    

        public string ReceiveStr => Encoding.UTF8.GetString(receiveBuffer);
    

        private void Start()
        {
            thread = new Thread(Init);
            thread.IsBackground = true;
            thread.Start();
        }
    
        private void Init()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Parse(ip),port));
            server.Listen(0);
            server.BeginAccept(AcceptCallBack,server);
        }

        private void AcceptCallBack(IAsyncResult ar)
        {
            Socket serverSocket = ar.AsyncState as Socket;
            Socket clientSocket = serverSocket.EndAccept(ar);
            Debug.Log("连接客户端成功");
            clientSocket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, ReceiveCallBack, clientSocket);
            serverSocket.BeginAccept(AcceptCallBack, serverSocket);
        }



        private void ReceiveCallBack(IAsyncResult ar)
        {
            Socket clientSocket = null;
            try
            {
                clientSocket = ar.AsyncState as Socket;
                int count = clientSocket.EndReceive(ar);
                if (count <= 0)
                {
                    clientSocket.Close();
                    return;
                }  
                //接收到消息，执行方法
                Debug.Log(ReceiveStr);
                if (ReceiveStr.Contains(submitStr))
                {
                    isSubmit = true;
                }
                clientSocket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, ReceiveCallBack, clientSocket);
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
    
    
        private void OnDestroy()
        {
            Close();
        }

        private void Close()
        {
            if (server != null)
            {
                server.Close();
            }

            if (thread != null)
            {
                thread.Abort();
            }
        
        }
  
    }
}