
using UnityEngine;


public class Test1 : MonoBehaviour
{
    public Transform go1;
    
    private void Start()
    {
        var v = Vector3.Dot(transform.forward, go1.position - transform.position);
         Debug.Log(v);
        var v2 = Vector3.Cross(transform.forward, go1.position - transform.position);
        Debug.Log(v2);
        // Debug.Log(transform.Get(go1.position));
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
    }
}

