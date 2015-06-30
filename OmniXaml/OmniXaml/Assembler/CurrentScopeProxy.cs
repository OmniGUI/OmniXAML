namespace OmniXaml.Assembler
{
    using System.Collections.Generic;
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
                return stackingLinkedList.GetValue().Instance;
            }
            set
            {
                stackingLinkedList.GetValue().Instance = value;
            }
        }

        public XamlType Type
        {
            get
            {
                return stackingLinkedList.GetValue().XamlType;
            }
            set
            {
                stackingLinkedList.GetValue().XamlType = value;
            }
        }

        public XamlMember Property
        {
            get
            {
                return stackingLinkedList.GetValue().Member;
            }
            set
            {
                stackingLinkedList.GetValue().Member = value;
            }
        }

        public object Collection
        {
            get
            {
                return stackingLinkedList.GetValue().Collection;
            }
            set
            {
                stackingLinkedList.GetValue().Collection = value;
            }
        }

        public bool IsPropertyValueSet
        {
            set
            {
                stackingLinkedList.GetValue().IsPropertyValueSet = value;
            }
        }

        public bool IsCollectionHoldingObject
        {
            get
            {
                return stackingLinkedList.GetValue().IsObjectFromMember;
            }
            set
            {
                stackingLinkedList.GetValue().IsObjectFromMember = value;
            }
        }

        public bool WasAssignedAtCreation
        {
            get { return stackingLinkedList.GetValue().WasAssignedAtCreation; }
            set { stackingLinkedList.GetValue().WasAssignedAtCreation = value; }
        }

        public List<object> ConstructorArguments { get; set; }
    }
}