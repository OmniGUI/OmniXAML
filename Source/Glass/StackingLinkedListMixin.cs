namespace Glass
{
    using System;
    using System.Collections.Generic;

    public static class StackingLinkedListMixin
    {
        public static IEnumerable<LinkedListNode<T>> TraverseBackwards<T>(this StackingLinkedList<T> sll) where T : class 
        {
            var currentNode = (LinkedListNode<T>)sll.Current;
            
            while (currentNode != null)
            {
                yield return currentNode;
                currentNode = currentNode.Previous;
            } 
        }
    }
}