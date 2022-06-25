/****************************************************
    文件：Main.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.Pool;

namespace YC
{
    public class Main : MonoBehaviour 
    {
        private void Start()
        {
            LinkedList<int> list = new LinkedList<int>();
            for (int i = 0; i < 5; i++)
            {
                list.AddFirst(i);
                Debug.Log(list.Count);
                Debug.Log(list.Get(i));
            } 
            
        }
    }
}