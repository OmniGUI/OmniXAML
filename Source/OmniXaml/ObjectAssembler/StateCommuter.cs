namespace OmniXaml.ObjectAssembler
{
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using Commands;
    using Glass.Core;
    using TypeConversion;
    using Typing;

    public class StateCommuter
    {
        private readonly IInstanceLifeCycleListener lifecycleListener;
        private readonly IValueContext valueContext;
        private StackingLinkedList<Level> stack;

        public StateCommuter(StackingLinkedList<Level> stack,
            IRuntimeTypeSource typeSource,
            IInstanceLifeCycleListener lifecycleListener,
            IValueContext valueContext)
        {
            Guard.ThrowIfNull(stack, nameof(stack));
            Guard.ThrowIfNull(typeSource, nameof(typeSource));

            Stack = stack;
            this.lifecycleListener = lifecycleListener;
            this.valueContext = valueContext;
        }

        public CurrentLevelWrapper Current { get; private set; }

        public PreviousLevelWrapper Previous { get; private set; }

        public int Level => stack.Count;

        private bool HasParentToAssociate => Level > 1;

        public bool WasAssociatedRightAfterCreation => Current.WasAssociatedRightAfterCreation;

        public IValueContext ValueContext => valueContext;

        public ValueProcessingMode ValueProcessingMode { get; set; }

        public object ValueOfPreviousInstanceAndItsMember => GetValueTuple(Previous.Instance, (MutableMember) Previous.Member);

        private StackingLinkedList<Level> Stack
        {
            get { return stack; }
            set
            {
                stack = value;
                UpdateLevelWrappers();
            }
        }

        public bool ParentIsOneToMany => Previous.XamlMemberIsOneToMany;

        public InstanceProperties InstanceProperties => Current.InstanceProperties;
        public bool HasParent => !Previous.IsEmpty;

        public ITopDownValueContext TopDownValueContext => valueContext.TopDownValueContext;

        public void SetKey(object value)
        {
            InstanceProperties.Key = value;
        }

        public void AssignChildToParentProperty()
        {
            var previousMember = (MutableMember) Previous.Member;
            object compatibleValue;

            var success = CommonValueConversion.TryConvert(Current.Instance, previousMember.XamlType, valueContext, out compatibleValue);

            if (!success)
            {
                compatibleValue = Current.Instance;
            }

            previousMember.SetValue(Previous.Instance, compatibleValue, valueContext);
        }

        public void RaiseLevel()
        {
            stack.Push(new Level());
            UpdateLevelWrappers();
        }

        private void UpdateLevelWrappers()
        {
            Current = new CurrentLevelWrapper(stack.Current != null ? stack.CurrentValue : new NullLevel(), valueContext);
            Previous = new PreviousLevelWrapper(stack.Previous != null ? stack.PreviousValue : new NullLevel());
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
        }

        private void MaterializeInstanceOfCurrentType()
        {
            var xamlType = Current.XamlType;
            if (xamlType == null)
            {
                throw new ParseException("A type must be set before invoking MaterializeInstanceOfCurrentType");
            }
            var parameters = GatherConstructionArguments();
            var instance = xamlType.CreateInstance(parameters);

            Current.Instance = instance;
            lifecycleListener.OnBegin(instance);
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
                TargetProperty = Previous.Instance.GetType().GetRuntimeProperty(Previous.Member.Name),
                ValueContext = ValueContext,                
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

        private void AddChildToCurrentCollection()
        {
            TypeOperations.AddToCollection(Previous.Collection, Current.Instance);
        }

        public void AddCtorArgument(string stringValue)
        {
            Current.CtorArguments.Add(new ConstructionArgument(stringValue));
        }

        public void AssociateCurrentInstanceToParent()
        {
            if (HasParentToAssociate && !Current.IsMarkupExtension)
            {
                if (Previous.CanHostChildren)
                {
                    AddChildToHost();
                }
                else
                {
                    AssignChildToParentProperty();
                }

                lifecycleListener.OnAssociatedToParent(Current.Instance);
            }
        }

        public void RegisterInstanceNameToNamescope()
        {
            if (InstanceProperties.Name != null)
            {
                var nameScope = FindNamescope();
                nameScope?.Register(InstanceProperties.Name, Current.Instance);
            }

            InstanceProperties.Name = null;
            InstanceProperties.HadPreviousName = false;
        }

        public void PutNameToCurrentInstanceIfAny()
        {
            if (InstanceProperties.Name != null)
            {
                if (Current.InstanceName != null)
                {
                    Current.InstanceProperties.HadPreviousName = true;
                }

                Current.InstanceName = InstanceProperties.Name;
            }
        }

        private INameScope FindNamescope()
        {
            if (Current.InstanceProperties.HadPreviousName)
            {
                return FindNamescopeForInstanceThatHadPreviousName();
            }
            else
            {
                return FindNamescopeForInstanceWithNoName();
            }
        }

        private INameScope FindNamescopeForInstanceWithNoName()
        {
            return FindNamescopeSkippingAncestor(0);
        }

        private INameScope FindNamescopeForInstanceThatHadPreviousName()
        {
            return FindNamescopeSkippingAncestor(1);
        }

        private INameScope FindNamescopeSkippingAncestor(int skip)
        {
            return stack.GetAncestors()
                .Skip(skip)
                .Select(level => level.XamlType?.GetNamescope(level.Instance))
                .FirstOrDefault(x => x != null);
        }

        private void AddChildToHost()
        {
            if (Previous.IsDictionary)
            {
                AddChildToDictionary();
            }
            else
            {
                AddChildToCurrentCollection();
            }
        }

        private void AddChildToDictionary()
        {
            TypeOperations.AddToDictionary((IDictionary) Previous.Collection, InstanceProperties.Key, Current.Instance);
            ClearKey();
        }

        private void ClearKey()
        {
            SetKey(null);
        }

        private static object GetValueTuple(object instance, MutableMember member)
        {
            var xamlMemberBase = member;
            return xamlMemberBase.GetValue(instance);
        }

        public void SetNameForCurrentInstance(string value)
        {
            InstanceProperties.Name = value;
        }

        public void AssociateCurrentInstanceToParentForCreation()
        {
            AssociateCurrentInstanceToParent();
            Current.WasAssociatedRightAfterCreation = true;
        }
    }
}