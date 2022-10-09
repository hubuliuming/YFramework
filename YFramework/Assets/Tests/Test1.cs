/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;
using YFramework.Kit.Net;

public class Test1 : MonoBehaviour
{
    // private TcpServer _server;
     private TcpClient _client;
    //private UDPClient _udpClient;
    private void Start()
    {
         //_server = new TcpServer("192.168.2.39", 6666);
         _client = new TcpClient("192.168.2.39", 7777);
         //_udpClient = new UDPClient("192.168.2.39", 8888);
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     _udpClient.SendMessage("192.168.2.39", 9999,"haha");
        // }
        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     _udpClient.SendMessage("192.168.2.39", 10000,"baba");
        // }
        if (_client.Received)
        {
            Debug.Log(_client.ReceivedStr);
            _client.Received = false;
        }
    }

    private void OnDestroy()
    {
        //_server.Close();
        _client.Close();
        //_udpClient.Close();
    }
}