namespace OmniXaml.Assembler
{
    using Glass;
    using Typing;

    internal class CurrentScopeProxy
    {
        private readonly StackingLinkedList<Scope> stackingLinkedList;

        public CurrentScopeProxy(StackingLinkedList<Scope> stackingLinkedList)
        {
            this.stackingLinkedList = stackingLinkedList;
        }

        public object Instance
        {
            get
            {
                return stackingLinkedList.Peek().Instance;
            }
            set
            {
                stackingLinkedList.Peek().Instance = value;
            }
        }

        public XamlType Type
        {
            get
            {
                return stackingLinkedList.Peek().XamlType;
            }
            set
            {
                stackingLinkedList.Peek().XamlType = value;
            }
        }

        public XamlMember Property
        {
            get
            {
                return stackingLinkedList.Peek().Member;
            }
            set
            {
                stackingLinkedList.Peek().Member = value;
            }
        }

        public object Collection
        {
            get
            {
                return stackingLinkedList.Peek().Collection;
            }
            set
            {
                stackingLinkedList.Peek().Collection = value;
            }
        }

        public bool IsPropertyValueSet
        {
            set
            {
                stackingLinkedList.Peek().IsPropertyValueSet = value;
            }
        }

        public bool IsObjectFromMember
        {
            get
            {
                return stackingLinkedList.Peek().IsObjectFromMember;
            }
            set
            {
                stackingLinkedList.Peek().IsObjectFromMember = value;
            }
        }

        public bool WasAssignedAtCreation
        {
            get { return stackingLinkedList.Peek().WasAssignedAtCreation; }
            set { stackingLinkedList.Peek().WasAssignedAtCreation = value; }
        }
    }
}