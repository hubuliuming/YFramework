#if UNITY_EDITOR

using NUnit.Framework;

namespace YFramework
{
    public  class V_0_0_4
    {
        class SingletonTestClass : Singleton<SingletonTestClass>
        {
            private SingletonTestClass(){}
        }
        // A Test behaves as an ordinary method
        [Test]
        public void V_0_0_4Editor()
        {
            var instanceA = SingletonTestClass.Instance;
            var instanceB = SingletonTestClass.Instance;
            Assert.AreEqual(instanceA.GetHashCode(),instanceB.GetHashCode());
        }
    }
}
#endif
