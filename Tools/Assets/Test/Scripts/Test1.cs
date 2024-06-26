
using System.Collections;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;



public class Test1 : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return null;
       
        using (var stream = File.Open(Application.streamingAssetsPath+"/HH.txt", FileMode.Create))
        {
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
            {
                writer.Write(1.250F);
                writer.Write(@"c:\Temp");
                writer.Write(10);
                writer.Write(true);
            }
        }
        using (var readerSteam = new FileStream(Application.streamingAssetsPath+"/DD.txt",FileMode.Open,FileAccess.Read))
        {
            using (var reader = new BinaryReader(readerSteam,Encoding.UTF8))
            {
                    Debug.Log(reader.ReadString());
                // Debug.Log(reader.ReadString());
                // Debug.Log(reader.ReadString());
            }
        }
    
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            
       
        }   
    }
    
}

public static class TestExtensive
{
    /// <summary>
    /// 获取目标点在自身的空间方向
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target"></param>
    /// <returns>x表示前后，1为前，-1为后，0则中间，y值为左右，-1为左，1为右，0则中间</returns>
    public static Vector2 HorizonDirection(this Transform self, Vector3 target)
    {
        //点乘计算前后，x 为1则为前，-1为后
        var v1 = Vector3.Dot(self.forward, target - self.position);
        var x = ToInt(v1);
        //叉积计算左右，Unity是左手坐标故取反
        var v2 = Vector3.Cross(self.forward, target - self.position).y * -1;
        var y = ToInt(v2);
        //原理上是y值为负数则在右边，但为了结果方便理解下面取反操作
        y *= -1;
        return new Vector2(x, y);
        int ToInt(float a) => a > 0 ? 1 : a < 0 ? -1 : 0;
        
        Regex reg = new Regex(@"^(?<x>\d+)-(?<y>\d+)$");
        var match = reg.Match("sddd");
        
    }
}

