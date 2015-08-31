namespace Glass
{
    using System.Collections.Generic;

    public class StackingLinkedList<T>
    {
        private readonly LinkedList<T> linkedList = new LinkedList<T>();

        public void Push(T item)
        {
            linkedList.AddLast(item);
        }

        public LinkedListNode<T> Current => linkedList.Last;
        public LinkedListNode<T> Previous => Current?.Previous;
        public T CurrentValue
        {
            get { return Current.Value; }
            set { Current.Value = value; }
        }

        public void Pop()
        {
            linkedList.RemoveLast();
        }

        public int Count => linkedList.Count;
        public T PreviousValue
        {
            get { return Previous.Value; }
            set { Previous.Value = value; }
        }
    }  
}