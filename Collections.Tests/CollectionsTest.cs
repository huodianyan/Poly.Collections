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

            //foreach(var item1 in array)
            //{
            //    Console.WriteLine($"FastArrayTest.Enumerator: {item1.Value}");//, {string.Join(",", array)}
            //}
            Console.WriteLine($"FastArrayTest: array: {string.Join(",", array)}");//, 

            array.Dispose();
        }
        [TestMethod]
        public void FastListTest()
        {
            var list = new FastList<TestStruct>(3);
            Assert.AreEqual(4, list.Capacity);
            list[2] = new TestStruct { Value = 13 };
            Assert.AreEqual(13, list[2].Value);
            Assert.AreEqual(3, list.Length);

            list.Length = 5;
            Assert.AreEqual(8, list.Capacity);
            Assert.AreEqual(5, list.Length);
            Assert.AreEqual(13, list[2].Value);

            ref var item = ref list.ElementAt(4);
            item.Value = 23;
            Assert.AreEqual(23, list[4].Value);

            //foreach (var item1 in list)
            //{
            //    Console.WriteLine($"FastArrayTest.Enumerator: {item1.Value}");//, {string.Join(",", array)}
            //}
            Console.WriteLine($"FastArrayTest: array: {string.Join(",", list)}");//, 

            list.Dispose();
        }
    }
    public struct TestStruct
    {
        public int Value;
        public override string ToString()
        {
            return $"TestStruct:{{{Value}}}";
        }
    }
}