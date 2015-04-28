namespace OmniXaml.Tests
{
    using System;
    using Glass;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StackWithLinkedNodesTests
    {
        private StackingLinkedList<int> sut;

        [TestInitialize]
        public void Initialize()
        {
            sut = new StackingLinkedList<int>();
        }

        [TestMethod]
        public void Push()
        {            
            sut.Push(1);
            Assert.AreEqual(1, sut.Current.Value);
        }

        [TestMethod]
        public void Pop()
        {
            sut.Push(2);
            sut.Push(1);
            sut.Pop();
            Assert.AreEqual(2, sut.Current.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PopEmptyFails()
        {
            sut.Pop();
        }

        [TestMethod]
        public void Count()
        {
            sut.Push(1);
            sut.Push(2);
            sut.Push(3);
            Assert.AreEqual(3, sut.Count);
        }
    }
}