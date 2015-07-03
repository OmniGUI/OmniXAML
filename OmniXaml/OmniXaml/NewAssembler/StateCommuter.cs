namespace OmniXaml.NewAssembler
{
    using System;
    using System.Collections;
    using Assembler;
    using Glass;
    using Typing;

    public class StateCommuter
    {
        private readonly StackingLinkedList<Level> stack;

        public StateCommuter(StackingLinkedList<Level> stack)
        {
            this.stack = stack;
        }

        public void AssignChildToParentProperty()
        {
            PreviousValue.XamlMember.SetValue(PreviousValue.Instance, CurrentValue.Instance);
        }

        public bool HasCurrentInstance => CurrentValue.Instance != null;

        public XamlType XamlType
        {
            get { return CurrentValue.XamlType; }
            set { CurrentValue.XamlType = value; }
        }

        public object Instance
        {
            get { return CurrentValue.Instance; }
            set { CurrentValue.Instance = value; }
        }

        private Level CurrentValue => stack.CurrentValue;

        private Level PreviousValue => stack.PreviousValue;

        public XamlMember Member
        {
            get { return CurrentValue.XamlMember; }
            set { CurrentValue.XamlMember = value; }
        }

        public int Level => stack.Count;

        public void RaiseLevel()
        {
            stack.Push(new Level());
        }

        public void DecreaseLevel()
        {
            stack.Pop();
        }

        public void CreateInstanceOfCurrentXamlTypeIfNotCreatedBefore()
        {
            if (!HasCurrentInstance)
            {
                MaterializeInstanceOfCurrentType();
            }
        }

        private void MaterializeInstanceOfCurrentType()
        {
            var xamlType = CurrentValue.XamlType;
            if (xamlType == null)
            {
                throw new InvalidOperationException("A type must be set before invoking MaterializeInstanceOfCurrentType");
            }

            CurrentValue.Instance = xamlType.CreateInstance(null);
        }

        public bool IsObjectFromMember
        {
            get { return CurrentValue.IsObjectFromMember; }
            set { CurrentValue.IsObjectFromMember = value; }
        }

        public bool IsPreviousHoldingChildrenIntoACollection => CurrentValue.Collection != null;

        public void AssignChildToCurrentCollection()
        {
            TypeOperations.Add(CurrentValue.Collection, Instance);
        }

        public void ConfigureForGetObject()
        {
            var collection = GetCollection();
            IsObjectFromMember = true;
            RaiseLevel();
            CurrentValue.Collection = collection;            
        }

        private ICollection GetCollection()
        {
            return (ICollection)PreviousValue.XamlMember.GetValue(PreviousValue.Instance);
        }
    }
}