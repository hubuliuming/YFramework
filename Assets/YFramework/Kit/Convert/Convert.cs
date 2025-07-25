/****************************************************
    文件：YConvert.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


using System;
using System.Text;

namespace YFramework.Kit.Convert
{
    public class Convert  
    {
        public static byte[] Convert16Byte(string strText)
        {
            strText = strText.Replace(" ", "");
            byte[] bText = new byte[strText.Length / 2];
            for (int i = 0; i < strText.Length / 2; i++)
            {
                bText[i] = System.Convert.ToByte(System.Convert.ToInt32(strText.Substring(i * 2, 2), 16));
            }
            return bText;
        }

        /// <summary>
        /// 把总秒数转化成分和秒显示
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns>itme1 is minute, item2 is seconds</returns>
        public static Tuple<int, int> Seconds2Minute(int seconds)
        {
            return new Tuple<int, int>(seconds / 60, seconds % 60);
        }
        
        /// <summary>
        /// 把数组里的每个元素依次转化成一个string
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ToStringAnyItem(int[] array)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in array)
            {
                sb.Append(item.ToString());
            }
            return sb.ToString();
        }
    }
}