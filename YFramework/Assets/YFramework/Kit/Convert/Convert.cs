/****************************************************
    文件：YConvert.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


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
    }
}