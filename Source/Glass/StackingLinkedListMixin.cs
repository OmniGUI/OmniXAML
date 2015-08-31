namespace Glass
{
    using System;

    public static class StackingLinkedListMixin
    {
        public static T ReverseLookup<T>(this StackingLinkedList<T> sll, Func<T, bool> lookupUntilPredicate) where T : class 
        {
            var currentNode = sll.Previous;
            
            while (currentNode != null && lookupUntilPredicate(currentNode.Value))
            {                
                currentNode = currentNode.Previous;
            } 

            return currentNode?.Value;
        }
    }
}