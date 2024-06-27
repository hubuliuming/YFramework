/****************************************************
    文件：Trigger2DCheck.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Trigger2DCheck
*****************************************************/

using UnityEngine;

namespace YFramework.Kit.Physics2D
{
    public class Trigger2DCheck : MonoBehaviour
    {
        public LayerMask targetLayer;
        public int enterCount;
        public bool Triggered => enterCount > 0;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (IsLayerMask(col.gameObject, targetLayer))
            {
                enterCount++;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (IsLayerMask(other.gameObject, targetLayer))
            {
                enterCount--;
            }
        }

        private bool IsLayerMask(GameObject go, LayerMask mask)
        {
            var goLayerMask = 1 << go.layer;
            return (mask.value & goLayerMask) > 0;
        }
    }
}