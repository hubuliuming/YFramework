/****************************************************
    文件：PoolManager.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/17 16:41:14
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace YFramework
{
    class Fish
    {
        
    }
    public class PoolExample : MonoBehaviour 
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem("YFramework/Examples/Managers/PoolExample")]
        private static void MenuClick()
        {
           var fishPool = new SimpleObjectPool<Fish>(() => new Fish(), null, 100);
            Debug.Log("curFishCount :"+ fishPool.curCount);
            var fishOne = fishPool.Allocate();
            Debug.Log("curFishCount :"+ fishPool.curCount);
            fishPool.Recycle(fishOne);
            Debug.Log("curFishCount :"+ fishPool.curCount);
            for (int i = 0; i < 10; i++)
            {
                fishPool.Allocate();
            }

            Debug.Log("curFishCount :"+ fishPool.curCount);
        }
#endif
    }
}