namespace OmniXaml.NewAssembler
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using Assembler;
    using Commands;
    using Glass;
    using Typing;

    public class StateCommuter
    {
        private readonly StackingLinkedList<Level> stack;
        private readonly ITopDownMemberValueContext topDownMemberValueContext;
        private object key;

        public StateCommuter(StackingLinkedList<Level> stack, WiringContext wiringContext, ITopDownMemberValueContext topDownMemberValueContext)
        {            
            Guard.ThrowIfNull(stack, nameof(stack));
            Guard.ThrowIfNull(wiringContext, nameof(wiringContext));
            Guard.ThrowIfNull(topDownMemberValueContext, nameof(topDownMemberValueContext));

            this.stack = stack;
            this.topDownMemberValueContext = topDownMemberValueContext;
            ValuePipeline = new ValuePipeline(wiringContext);
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

        public XamlMemberBase Member
        {
            get { return CurrentValue.XamlMember; }
            set { CurrentValue.XamlMember = value; }
        }

        public int Level => stack.Count;

        public bool IsGetObject
        {
            get { return CurrentValue.IsGetObject; }
            set { CurrentValue.IsGetObject = value; }
        }

        public ICollection Collection
        {
            get { return CurrentValue.Collection; }
            set { CurrentValue.Collection = value; }
        }

        public Collection<ConstructionArgument> CurrentCtorParameters
        {
            get { return CurrentValue.CtorArguments; }
            set { CurrentValue.CtorArguments = value; }
        }

        public XamlMemberBase PreviousMember => PreviousValue.XamlMember;
        public object PreviousInstance => PreviousValue.Instance;
        private bool IsParentOneToMany => PreviousValue.Collection != null;
        public IList<ConstructionArgument> CtorArguments => CurrentValue.CtorArguments;
        private bool IsParentDictionary => PreviousValue.Collection is IDictionary;
        private bool InstanceCanBeAssociated => !(Instance is IMarkupExtension);
        private bool HasParentToAssociate => Level > 1;
        public bool WasAssociatedRightAfterCreation => CurrentValue.WasAssociatedRightAfterCreation;
        public ValuePipeline ValuePipeline { get; }

        public void SetKey(object value)
        {
            key = value;
        }

        public void AssignChildToParentProperty()
        {
            var previousMember = (MutableXamlMember) PreviousMember;

            var compatibleValue = ValuePipeline.ConvertValueIfNecessary(Instance, previousMember.XamlType);

            previousMember.SetValue(PreviousInstance, compatibleValue);
        }

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
            var parameters = GatherConstructionArguments();
            var instance = xamlType.CreateInstance(parameters);

            CurrentValue.Instance = instance;
        }

        public object GetValueProvidedByMarkupExtension(IMarkupExtension instance)
        {
            var markupExtensionContext = GetExtensionContext();
            return instance.ProvideValue(markupExtensionContext);
        }

        private MarkupExtensionContext GetExtensionContext()
        {
            var inflationContext = new MarkupExtensionContext
            {
                TargetObject = PreviousInstance,
                TargetProperty = PreviousInstance.GetType().GetRuntimeProperty(PreviousMember.Name),
                TypeRepository = ValuePipeline.WiringContext.TypeContext,
                TopDownMemberValueContext = topDownMemberValueContext,
            };

            return inflationContext;
        }

        private object[] GatherConstructionArguments()
        {
            if (CtorArguments == null)
            {
                return null;
            }

            var arguments = CtorArguments.Select(argument => argument.Value).ToArray();
            return arguments.Any() ? arguments : null;
        }

        private void AssignChildToCurrentCollection()
        {
            TypeOperations.Add(PreviousValue.Collection, Instance);
        }

        public void AddCtorArgument(string stringValue)
        {
            CurrentValue.CtorArguments.Add(new ConstructionArgument(stringValue));
        }

        public void ResetCtorArguments()
        {
            CurrentValue.CtorArguments = null;
        }

        public void AssociateCurrentInstanceToParent()
        {
            if (HasParentToAssociate && InstanceCanBeAssociated)
            {                
                if (IsParentOneToMany)
                {
                    AssignInstanceToHost();
                }
                else
                {
                    AssignChildToParentProperty();
                }
            }
        }

        private void AssignInstanceToHost()
        {
            if (IsParentDictionary)
            {
                AssignChildToDictionary();
            }
            else
            {
                AssignChildToCurrentCollection();
            }
        }

        private void AssignChildToDictionary()
        {
            TypeOperations.AddToDictionary((IDictionary) PreviousValue.Collection, key, Instance);
            ClearKey();
        }

        private void ClearKey()
        {   
            SetKey(null);
        }

        public void AssociateCurrentInstanceToParentForCreation()
        {
            AssociateCurrentInstanceToParent();
            CurrentValue.WasAssociatedRightAfterCreation = true;
        }

        public ValueProcessingMode ValueProcessingMode { get; set; }
    }
}