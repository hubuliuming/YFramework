#if UNITY_EDITOR

using NUnit.Framework;

namespace YFramework
{
    public  class V_0_0_4
    {
        class MonoSingletonTestClass : MonoSingleton<MonoSingletonTestClass>
        {
            private MonoSingletonTestClass(){}
        }
        // A Test behaves as an ordinary method
        [Test]
        public void V_0_0_4Mono()
        {
            var instanceA = MonoSingletonTestClass.Instance;
            var instanceB = MonoSingletonTestClass.Instance;
            
            Assert.AreEqual(instanceA.GetHashCode(), instanceB.GetHashCode());
        }
    
    }
}
#endif