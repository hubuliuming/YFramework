/****************************************************
    文件：SimplePC.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：简单计数器
*****************************************************/

namespace YFramework
{
    public interface IRefCounter
    {
        int RefCount { get; }
        void Retain(object refOwner = null);
        void Release(object refOwner = null);
    }
    public class SimplePC : IRefCounter 
    {
        public int RefCount { get; private set; }
        public void Retain(object refOwner = null)
        {
            RefCount++;
        }

        public void Release(object refOwner = null)
        {
            RefCount--;
            if (RefCount == 0)
            {
                OnZeroRef();
            }
        }

        protected virtual void OnZeroRef()
        {
            
        }
    }
}