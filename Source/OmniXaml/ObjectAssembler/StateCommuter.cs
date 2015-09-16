namespace OmniXaml.ObjectAssembler
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Commands;
    using Glass;
    using Typing;

    public class StateCommuter
    {
        private StackingLinkedList<Level> stack;
        private readonly ITopDownValueContext topDownValueContext;
        private readonly InstanceProperties instanceProperties;

        public StateCommuter(StackingLinkedList<Level> stack, IWiringContext wiringContext, ITopDownValueContext topDownValueContext)
        {
            Guard.ThrowIfNull(stack, nameof(stack));
            Guard.ThrowIfNull(wiringContext, nameof(wiringContext));
            Guard.ThrowIfNull(topDownValueContext, nameof(topDownValueContext));

            Stack = stack;
            this.topDownValueContext = topDownValueContext;
            ValuePipeline = new ValuePipeline(wiringContext.TypeContext, topDownValueContext);
            instanceProperties = new InstanceProperties();
        }

        public CurrentLevelWrapper Current => new CurrentLevelWrapper(stack.CurrentValue);
        private PreviousLevelWrapper Previous => new PreviousLevelWrapper(stack.PreviousValue);

        public int Level => stack.Count;

        private bool HasParentToAssociate => Level > 1;
        public ValuePipeline ValuePipeline { get; }

        public ValueProcessingMode ValueProcessingMode { get; set; }

        public object ValueOfPreviousInstanceAndItsMember => GetValueTuple(Previous.Instance, (MutableXamlMember)Previous.XamlMember);

        private StackingLinkedList<Level> Stack
        {
            get { return stack; }
            set
            {
                stack = value;
                UpdateLevelWrappers();
            }
        }

        public void SetKey(object value)
        {
            instanceProperties.Key = value;
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
            UpdateLevelWrappers();
        }

        private void UpdateLevelWrappers()
        {
            new CurrentLevelWrapper(stack.Current != null ? stack.CurrentValue : new NullLevel());
            new PreviousLevelWrapper(stack.Previous != null ? stack.PreviousValue : new NullLevel());
        }

        public void DecreaseLevel()
        {
            stack.Pop();
            UpdateLevelWrappers();
        }

        public void CreateInstanceOfCurrentXamlTypeIfNotCreatedBefore()
        {
            if (!Current.HasInstance)
            {
                MaterializeInstanceOfCurrentType();
            }

            SaveCurrentInstanceToTopDownEnvironment();
        }

        private void SaveCurrentInstanceToTopDownEnvironment()
        {
            topDownValueContext.SetInstanceValue(Current.XamlType, Current.Instance);
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
                TopDownValueContext = topDownValueContext
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
            if (instanceProperties.Name != null)
            {
                nameScope?.Register(instanceProperties.Name, Current.Instance);
            }

            instanceProperties.Name = null;
        }

        private INameScope LookupParentNamescope()
        {
            var level = stack.ReverseLookup(l => !IsNameScope(l));
            return level?.Instance as INameScope;
        }

        private static bool IsNameScope(Level level)
        {
            if (level.XamlType != null)
            {
                var xamlTypeSaysIsNameScope = level.XamlType.IsNameScope;
                var instanceIsNameScope = level.Instance is INameScope;

                return xamlTypeSaysIsNameScope && instanceIsNameScope;
            }

            return false;
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
            TypeOperations.AddToDictionary((IDictionary)Previous.Collection, instanceProperties.Key, Current.Instance);
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
            instanceProperties.Name = value;
        }
    }
}