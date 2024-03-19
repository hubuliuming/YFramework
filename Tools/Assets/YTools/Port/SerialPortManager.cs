using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using UnityEngine;

//Unity的平台框架需要选择为.framework
public class SerialPortManager : MonoBehaviour
{
    private static SerialPortManager _instance;

    public static SerialPortManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject(nameof(SerialPortManager)).AddComponent<SerialPortManager>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
       DontDestroyOnLoad(gameObject);
    }

    private readonly Dictionary<string, SerialPortController> SerialPortDic = new Dictionary<string, SerialPortController>();

    private string ErrorMessage
    {
        set
        {
            ErrorMessage = value;
            Debug.LogError("错误：" + value);
            return;
        }
    }

    /// <summary>
    /// 打开串口
    /// </summary>
    /// <param name="portName">串口名——Example:COM1</param>
    /// <param name="baudRate">波特率</param>
    /// <param name="parity">校验位</param>
    /// <param name="dataBits">数据位</param>
    /// <param name="stopBits">停止位</param>
    public void OpenSerialPort(string portName, int baudRate, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
    {
        SerialPortController serialPort = new SerialPortController(portName, baudRate, parity, dataBits, stopBits);
        SerialPortDic.Add(serialPort.Port.PortName, serialPort);
    }

    /// <summary>
    /// 关闭串口
    /// </summary>
    /// <param name="portName">串口名</param>
    public void CloseSerialPort(string portName)
    {
        try
        {
            SerialPortDic.TryGetValue(portName, out var serialPort);
            serialPort.Close();
            SerialPortDic.Remove(portName);
        }
        catch (Exception e)
        {
            SerialPortDic.TryGetValue(portName, out var serialPort);
            Debug.LogError(serialPort + "关闭失败——" + e.Message);
            throw;
        }
    }


    /// <summary>
    /// 关闭所有串口
    /// </summary>
    public void CloseAllSerialPort()
    {
        foreach (var value in SerialPortDic)
        {
            value.Value.Close();
        }
    }

    /// <summary>
    /// 向指定串口发送消息
    /// </summary>
    /// <param name="portName">串口名</param>
    /// <param name="message">消息内容</param>
    public void SendMessageToSerialPort(string portName, string message)
    {
        try
        {
            SerialPortDic.TryGetValue(portName, out var value);
            value.Port.Write(Convert16(message), 0, Convert16(message).Length);
            Debug.Log("成功发送消息：" + message);
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            //throw;
        }
    }

    /// <summary>
    /// 获取串口消息
    /// </summary>
    /// <param name="portName">串口名</param>
    /// <returns></returns>
    public string GetMessage(string portName)
    {
        SerialPortDic.TryGetValue(portName, out var port);
        return port.Message;
    }

    private byte[] Convert16(string strText) //转换为16进制
    {
        strText = strText.Replace(" ", "");
        byte[] bText = new byte[strText.Length / 2];
        for (int i = 0; i < strText.Length / 2; i++)
        {
            bText[i] = Convert.ToByte(Convert.ToInt32(strText.Substring(i * 2, 2), 16));
        }

        return bText;
    }
}

public class SerialPortController
{
    public SerialPortController(string portName, int baudRate, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
    {
        Port = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
        Port.ReadTimeout = 400;
        if (Port.IsOpen)
        {
            Debug.LogError(portName + "已经打开：" + Port.IsOpen);
        }

        try
        {
            Port.Open();
            dataReceiveThread = new Thread(new ThreadStart(DataReceiveFunction));
            Debug.Log(portName + "成功打开！");
        }
        catch (Exception e)
        {
            Debug.LogError(portName + "打开失败：" + e.Message);
            //throw;
        }

        Debug.Log(portName + "串口是否打开：" + Port.IsOpen);
    }

    public SerialPort Port { get; private set; }
    private readonly Thread dataReceiveThread;

    public string Message
    {
        get => Message;
        set
        {
            Message = value;
            Debug.Log(Port.PortName + Message);
        }
    }

    public void Close()
    {
        if (Port != null)
        {
            Port.Close();
        }

        if (dataReceiveThread != null)
        {
            dataReceiveThread.Abort();
        }
    }

    private void DataReceiveFunction()
    {
        byte[] buffer = new byte [1024];
        while (true)
        {
            if (Port is { IsOpen: true })
            {
                try
                {
                    var bytes = Port.Read(buffer, 0, buffer.Length);
                    if (bytes == 0)
                    {
                        continue;
                    }
                    else
                    {
                        Message = Encoding.Default.GetString(buffer);
                        Debug.Log(Port.PortName + ":" + Message);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }

            Thread.Sleep(10);
        }
    }
}