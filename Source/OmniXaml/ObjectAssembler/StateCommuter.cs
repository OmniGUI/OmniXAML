namespace OmniXaml.ObjectAssembler
{
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using Commands;
    using Glass;
    using Typing;

    public class StateCommuter
    {
        private StackingLinkedList<Level> stack;
        private readonly ITopDownValueContext topDownValueContext;
        private PreviousLevelWrapper previous;
        private CurrentLevelWrapper current;
        private readonly InstanceLifeCycleNotifier lifecycleNotifier;

        public StateCommuter(IObjectAssembler objectAssembler, StackingLinkedList<Level> stack, IWiringContext wiringContext, ITopDownValueContext topDownValueContext)
        {
            Guard.ThrowIfNull(stack, nameof(stack));
            Guard.ThrowIfNull(wiringContext, nameof(wiringContext));
            Guard.ThrowIfNull(topDownValueContext, nameof(topDownValueContext));

            Stack = stack;
            this.topDownValueContext = topDownValueContext;
            ValuePipeline = new ValuePipeline(wiringContext.TypeContext, topDownValueContext);
            lifecycleNotifier = new InstanceLifeCycleNotifier(objectAssembler);
        }

        public CurrentLevelWrapper Current => current;

        public PreviousLevelWrapper Previous => previous;

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

        public bool ParentIsOneToMany => Previous.XamlMemberIsOneToMany;

        public void SetKey(object value)
        {
            InstanceProperties.Key = value;
        }

        public InstanceProperties InstanceProperties => Current.InstanceProperties;

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
            current = new CurrentLevelWrapper(stack.Current != null ? stack.CurrentValue : new NullLevel());
            previous = new PreviousLevelWrapper(stack.Previous != null ? stack.PreviousValue : new NullLevel());
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
            lifecycleNotifier.NotifyBegin(instance);            
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
            if (HasParentToAssociate && !(Current.IsMarkupExtension))
            {

                lifecycleNotifier.NotifyAfterProperties(Current.Instance);

                if (Previous.CanHostChildren)
                {
                    AddChildToHost();
                }
                else
                {
                    AssignChildToParentProperty();
                }
                lifecycleNotifier.NotifyAssociatedToParent(Current.Instance);

                RegisterInstanceNameToNamescope();
                lifecycleNotifier.NotifyEnd(Current.Instance);
            }
        }

        private void RegisterInstanceNameToNamescope()
        {
            if (InstanceProperties.Name != null)
            {
                var nameScope = LookupParentNamescope();
                nameScope?.Register(InstanceProperties.Name, Current.Instance);
            }

            InstanceProperties.Name = null;
        }

        public void PutNameToCurrentInstanceIfAny()
        {
            var runtimeNameMember = Current.XamlType.RuntimeNamePropertyMember;
            if (InstanceProperties.Name != null)
            {
                runtimeNameMember?.SetValue(Current.Instance, InstanceProperties.Name);
            }
        }

        private INameScope LookupParentNamescope()
        {
            return stack.TraverseBackwards()
                .Select(x => x.Value.XamlType?.GetNamescope(x.Value.Instance))
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
            TypeOperations.AddToDictionary((IDictionary)Previous.Collection, InstanceProperties.Key, Current.Instance);
            ClearKey();
        }

        private void ClearKey()
        {
            SetKey(null);
        }

        private static object GetValueTuple(object instance, MutableXamlMember member)
        {
            var xamlMemberBase = member;
            return xamlMemberBase.GetValue(instance);
        }

        public void SetNameForCurrentInstance(string value)
        {
            InstanceProperties.Name = value;
        }
    }
}