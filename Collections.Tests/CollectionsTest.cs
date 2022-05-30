using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Poly.Collections.Tests
{
    [TestClass]
    public partial class CollectionsTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
        }
        [ClassCleanup]
        public static void ClassCleanup()
        {
        }
        [TestInitialize]
        public void TestInitialize()
        {
        }
        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void ArrayPoolTest()
        {
            var array = ArrayPool<TestStruct>.Shared.Rent(5);
            Assert.AreEqual(8, array.Length);

            ArrayPool<TestStruct>.Shared.Return(array);

            var array1 = ArrayPool<TestStruct>.Shared.Rent(7);
            Assert.AreEqual(array, array1);
        }

        [TestMethod]
        public void FastArrayTest()
        {
            var array = new FastArray<TestStruct>(3);
            Assert.AreEqual(4, array.Capacity);
            array[2] = new TestStruct { Value = 13 };
            Assert.AreEqual(13, array[2].Value);
            Assert.AreEqual(3, array.Length);

            array.Length = 5;
            Assert.AreEqual(8, array.Capacity);
            Assert.AreEqual(5, array.Length);
            Assert.AreEqual(13, array[2].Value);

            ref var item = ref array.ElementAt(4);
            item.Value = 23;
            Assert.AreEqual(23, array[4].Value);

            array.Dispose();
        }
    }
    public struct TestStruct
    {
        public int Value;
    }
}