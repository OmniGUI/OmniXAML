namespace OmniXaml.ObjectAssembler
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using Commands;
    using Glass;
    using Typing;

    public class StateCommuter
    {
        private readonly StackingLinkedList<Level> stack;
        private readonly ITopDownMemberValueContext topDownMemberValueContext;
        private object key;

        public StateCommuter(StackingLinkedList<Level> stack, IWiringContext wiringContext, ITopDownMemberValueContext topDownMemberValueContext)
        {
            Guard.ThrowIfNull(stack, nameof(stack));
            Guard.ThrowIfNull(wiringContext, nameof(wiringContext));
            Guard.ThrowIfNull(topDownMemberValueContext, nameof(topDownMemberValueContext));

            this.stack = stack;
            this.topDownMemberValueContext = topDownMemberValueContext;
            ValuePipeline = new ValuePipeline(wiringContext.TypeContext);
        }

        public CurrentLevelWrapper Current => new CurrentLevelWrapper(stack.CurrentValue);
        private PreviousLevelWrapper Previous => new PreviousLevelWrapper(stack.PreviousValue);

        public int Level => stack.Count;

        private bool HasParentToAssociate => Level > 1;
        public ValuePipeline ValuePipeline { get; }

        public ValueProcessingMode ValueProcessingMode { get; set; }

        public object ValueOfPreviousInstanceAndItsMember => GetValueTuple(Previous.Instance, (MutableXamlMember)Previous.XamlMember);

        private string Name { get; set; }

        public void SetKey(object value)
        {
            key = value;
        }

        public void AssignChildToParentProperty()
        {
            var previousMember = (MutableXamlMember)Previous.XamlMember;

            var compatibleValue = ValuePipeline.ConvertValueIfNecessary(Current.Instance, previousMember.XamlType);

            previousMember.SetValue(Previous.Instance, compatibleValue);
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
            if (!Current.HasInstance)
            {
                MaterializeInstanceOfCurrentType();
            }
        }

        private void MaterializeInstanceOfCurrentType()
        {
            var xamlType = Current.XamlType;
            if (xamlType == null)
            {
                throw new XamlParseException("A type must be set before invoking MaterializeInstanceOfCurrentType");
            }
            var parameters = GatherConstructionArguments();
            var instance = xamlType.CreateInstance(parameters);

            Current.Instance = instance;
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
                TargetObject = Previous.Instance,
                TargetProperty = Previous.Instance.GetType().GetRuntimeProperty(Previous.XamlMember.Name),
                TypeRepository = ValuePipeline.TypeRepository,
                TopDownMemberValueContext = topDownMemberValueContext
            };

            return inflationContext;
        }

        private object[] GatherConstructionArguments()
        {
            if (Current.CtorArguments == null)
            {
                return null;
            }

            var arguments = Current.CtorArguments.Select(argument => argument.Value).ToArray();
            return arguments.Any() ? arguments : null;
        }

        private void AssignChildToCurrentCollection()
        {
            TypeOperations.AddToCollection(Previous.Collection, Current.Instance);
        }

        public void AddCtorArgument(string stringValue)
        {
            Current.CtorArguments.Add(new ConstructionArgument(stringValue));
        }

        public void AssociateCurrentInstanceToParent()
        {
            if (HasParentToAssociate && !(Current.IsMarkupExtension))
            {
                if (Previous.IsOneToMany)
                {
                    AssignInstanceToHost();
                }
                else
                {
                    AssignChildToParentProperty();
                }

                TryAddInstanceToNameScope();
            }
        }

        private void TryAddInstanceToNameScope()
        {
            var nameScope = LookupParentNamescope();
            if (Name != null)
            {
                nameScope?.Register(Name, Current.Instance);
            }

            Name = null;
        }

        private INameScope LookupParentNamescope()
        {
            var node = stack.ReverseLookup(level => !(level.Instance is INameScope));
            return node?.Instance as INameScope;
        }

        private void AssignInstanceToHost()
        {
            if (Previous.IsDictionary)
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
            TypeOperations.AddToDictionary((IDictionary)Previous.Collection, key, Current.Instance);
            ClearKey();
        }

        private void ClearKey()
        {
            SetKey(null);
        }

        public void AssociateCurrentInstanceToParentRightAfterCreation()
        {
            AssociateCurrentInstanceToParent();
            Current.WasInstanceAssignedRightAfterBeingCreated = true;
        }

        private static object GetValueTuple(object instance, MutableXamlMember member)
        {
            var xamlMemberBase = member;
            return xamlMemberBase.GetValue(instance);
        }

        public void SetNameForCurrentInstance(string value)
        {
            Name = value;
        }

        public class CurrentLevelWrapper
        {
            private readonly Level level;

            public CurrentLevelWrapper(Level level)
            {
                this.level = level;
            }

            public XamlMemberBase XamlMember
            {
                get { return level.XamlMember; }
                set { level.XamlMember = value; }
            }

            public bool IsGetObject
            {
                get { return level.IsGetObject; }
                set { level.IsGetObject = value; }
            }

            public ICollection Collection
            {
                get { return level.Collection; }
                set { level.Collection = value; }
            }

            public object Instance
            {
                get { return level.Instance; }
                set { level.Instance = value; }
            }

            public bool WasInstanceAssignedRightAfterBeingCreated
            {
                get { return level.WasInstanceAssignedRightAfterBeingCreated; }
                set { level.WasInstanceAssignedRightAfterBeingCreated = value; }
            }

            public XamlType XamlType
            {
                get { return level.XamlType; }
                set { level.XamlType = value; }
            }

            public Collection<ConstructionArgument> CtorArguments
            {
                get { return level.CtorArguments; }
                set { level.CtorArguments = value; }
            }

            public bool HasInstance => Instance != null;
            public bool IsMarkupExtension => Instance is IMarkupExtension;
        }

        private class PreviousLevelWrapper
        {
            private readonly Level level;

            public PreviousLevelWrapper(Level level)
            {
                this.level = level;
            }

            public XamlMemberBase XamlMember
            {
                get { return level.XamlMember; }
                set { level.XamlMember = value; }
            }

            public object Instance
            {
                get { return level.Instance; }
                set { level.Instance = value; }
            }

            public ICollection Collection
            {
                get { return level.Collection; }
                set { level.Collection = value; }
            }

            public bool IsOneToMany => Collection != null;
            public bool IsDictionary => Collection is IDictionary;
        }
    }
}