namespace OmniXaml.Tests
{
    using System;
    using Glass.Core;
    using Xunit;

    public class StackWithLinkedNodesTests
    {

        private static StackingLinkedList<int> CreateSut()
        {
            return new StackingLinkedList<int>();
        }

        [Fact]
        public void Push()
        {
            var sut = CreateSut();        
            sut.Push(1);
            Assert.Equal(1, sut.Current.Value);
        }

        [Fact]
        public void Pop()
        {
            var sut = CreateSut();
            sut.Push(2);
            sut.Push(1);
            sut.Pop();
            Assert.Equal(2, sut.Current.Value);
        }

        [Fact]
        public void PopEmptyFails()
        {
            var sut = CreateSut();
            Assert.Throws<InvalidOperationException>(() => sut.Pop());
        }

        [Fact]
        public void Count()
        {
            var sut = CreateSut();
            sut.Push(1);
            sut.Push(2);
            sut.Push(3);
            Assert.Equal(3, sut.Count);
        }
    }
}