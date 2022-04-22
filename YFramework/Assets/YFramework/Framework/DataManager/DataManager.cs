/****************************************************
    文件：DataManager.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


namespace YFramework
{
    public interface IDataMemory
    {
        T Get<T>(string key);
        void Set<T>(string key,T value);
        void Clear(string key);
        void ClearAll();
    }
    
    public class DataManager : NormalSingleton<DataManager>
    {
        private  IDataMemory m_dataMemory;

        public DataManager()
        {
            //后面类型多了可重构拓展
            m_dataMemory = new PlayerPrefsMemory();
        }
        public T Get<T>(string key) => m_dataMemory.Get<T>(key);
        public void Set<T>(string key, T value) => m_dataMemory.Set<T>(key,value);
        public void Clear(string key) => m_dataMemory.Clear(key);
        public void ClearAll() => m_dataMemory.ClearAll();

    }
}