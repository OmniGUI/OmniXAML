namespace OmniXaml.Assembler
{
    using Glass;

    public static class LinkedListStackExtensions
    {
        public static T Peek<T>(this StackingLinkedList<T> stackingLinkedList)
        {
            return stackingLinkedList.Current.Value;
        }
        public static T PeekPrevious<T>(this StackingLinkedList<T> stackingLinkedList)
        {
            return stackingLinkedList.Current.Previous.Value;
        }
    }
}