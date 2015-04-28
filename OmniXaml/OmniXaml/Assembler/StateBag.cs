namespace OmniXaml.Assembler
{
    using Glass;

    internal class StateBag
    {
        private readonly StackingLinkedList<Scope> stackingLinkedList = new StackingLinkedList<Scope>();

        public StateBag()
        {
            stackingLinkedList.Push(new Scope());
            SavedDepth = 0;
            Current = new CurrentScopeProxy(stackingLinkedList);
            Parent = new ParentScopeProxy(stackingLinkedList);
        }

        public int Depth => stackingLinkedList.Count;
        public int LiveDepth => Depth - SavedDepth;
        public int SavedDepth { get; }
        public CurrentScopeProxy Current { get; }
        public ParentScopeProxy Parent { get; }

        public void PushScope()
        {
            stackingLinkedList.Push(new Scope());
        }

        public void PopScope()
        {
            stackingLinkedList.Pop();
        }
    }
}