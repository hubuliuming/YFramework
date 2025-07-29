using UnityEngine;

namespace YFramework.Extension
{
    public static class EngineExtension
    {
        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }
    }
}