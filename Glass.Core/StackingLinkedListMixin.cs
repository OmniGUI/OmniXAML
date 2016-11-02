namespace Glass.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Glass.Core;

    public static class StackingLinkedListMixin
    {
        public static IEnumerable<T> GetAncestors<T>(this StackingLinkedList<T> sll) where T : class
        {
            return sll.ToList().Reverse();
        }
    }
}