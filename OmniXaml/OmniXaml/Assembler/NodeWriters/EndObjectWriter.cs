namespace OmniXaml.Assembler.NodeWriters
{
    using System.Reflection;
    using Typing;

    public class EndObjectWriter
    {
        private readonly ObjectAssembler assembler;
        private readonly StateBag bag;
        private readonly IXamlTypeRepository typeRepository;

        public EndObjectWriter(ObjectAssembler assembler, IXamlTypeRepository typeRepository)
        {
            this.assembler = assembler;
            this.typeRepository = typeRepository;
            bag = assembler.Bag;
        }

        public void WriteEndObject()
        {
            if (!bag.Current.IsObjectFromMember)
            {
                if (bag.Current.Instance == null)
                {
                    assembler.PrepareNewInstanceBecauseWeWantToConfigureIt(bag);
                }

                if (IsMarkupExtension(bag.Current.Type))
                {
                    AssignValueFromMarkupExtension(bag);
                }
            }

            if (bag.LiveDepth > 1)
            {
                AssignCurrentInstanceToParent(bag);
            }

            assembler.Result = bag.Current.Instance;

            bag.PopScope();
        }

        // TODO: This may be a duplicate (EndMemberWriter also uses it)
        private void AssignValueFromMarkupExtension(StateBag stateBag)
        {
            var markupExtension = (MarkupExtension) stateBag.Current.Instance;

            var extensionContext = GetExtensionContext(stateBag);

            var providedValue = markupExtension.ProvideValue(extensionContext);

            stateBag.Current.Type = typeRepository.Get(providedValue.GetType());
            stateBag.Current.Instance = providedValue;

            AssignCurrentInstanceToParent(bag);
        }

        private static XamlToObjectWiringContext GetExtensionContext(StateBag stateBag)
        {
            var inflationContext = new XamlToObjectWiringContext
            {
                TargetObject = stateBag.Parent.Instance,
                TargetProperty = stateBag.Parent.Instance.GetType().GetRuntimeProperty(stateBag.Parent.Property.Name),
            };

            return inflationContext;
        }

        private static bool IsMarkupExtension(XamlType type)
        {
            var underlyingType = type.UnderlyingType.GetTypeInfo();

            var meType = typeof (MarkupExtension).GetTypeInfo();
            return meType.IsAssignableFrom(underlyingType);
        }

        internal static void AssignCurrentInstanceToParent(StateBag bag)
        {
            var parentProperty = bag.Parent.Property;
            var currentInstance = bag.Current.Instance;
            var parentPropertyType = parentProperty.Type;

            if (IsAssignmentBeingMadeToContainer(parentProperty, parentPropertyType))
            {               
                AssignInstanceToParentCollection(bag.Parent.Collection, bag.Current.Instance);
            }
            else if (bag.Parent.Instance != null)
            {              
                if (!bag.Current.IsObjectFromMember)
                {
                    ApplyPropertyValue(bag, parentProperty, currentInstance, true);
                }
            }
        }

        private static bool IsAssignmentBeingMadeToContainer(XamlMember parentProperty, XamlType type)
        {
            return parentProperty.IsDirective && type.IsContainer;
        }

        private static void AssignInstanceToParentCollection(object parentCollection, object instance)
        {
            TypeOperations.Add(parentCollection, instance);
        }

        private static void ApplyPropertyValue(StateBag bag, XamlMember parentProperty, object value, bool onParent)
        {
            var instance = onParent ? bag.Parent.Instance : bag.Current.Instance;

            TypeOperations.SetValue(instance, parentProperty, value);
        }
    }
}