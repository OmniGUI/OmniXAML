namespace OmniXaml.Assembler
{
    using Glass;

    public static class LinkedListStackExtensions
    {
        public static T GetValue<T>(this StackingLinkedList<T> stackingLinkedList)
        {
            return stackingLinkedList.Current.Value;
        }

        public static void SetValue<T>(this StackingLinkedList<T> stackingLinkedList, T value)
        {
            stackingLinkedList.Current.Value = value;
        }

        public static T PeekPrevious<T>(this StackingLinkedList<T> stackingLinkedList)
        {
            return stackingLinkedList.Current.Previous.Value;
        }
    }
}