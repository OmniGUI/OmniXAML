namespace OmniXaml.Assembler
{
    using Glass;
    using Typing;

    internal class ParentScopeProxy
    {
        private readonly StackingLinkedList<Scope> stackingLinkedList;

        public ParentScopeProxy(StackingLinkedList<Scope> stackingLinkedList)
        {
            this.stackingLinkedList = stackingLinkedList;
        }

        public object Instance => stackingLinkedList.PeekPrevious().Instance;

        public XamlMember Property => stackingLinkedList.PeekPrevious().Member;

        public XamlType Type => stackingLinkedList.PeekPrevious().XamlType;

        public object Collection => stackingLinkedList.PeekPrevious().Collection;

        public bool IsPropertyValueSet
        {
            get
            {
                return stackingLinkedList.PeekPrevious().IsPropertyValueSet;
            }
            set
            {
                stackingLinkedList.PeekPrevious().IsPropertyValueSet = value;
            }
        }
    }
}