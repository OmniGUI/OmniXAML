namespace OmniXaml.Assembler.NodeWriters
{
    using Typing;

    internal class GetObjectWriter
    {
        private readonly ObjectAssembler objectAssembler;
        private readonly StateBag bag;

        public GetObjectWriter(ObjectAssembler objectAssembler)
        {
            this.objectAssembler = objectAssembler;
            bag = objectAssembler.Bag;
        }

        public void WriteGetObject()
        {
            objectAssembler.SetUnfinishedResult();

            if (bag.Current.Type != null)
            {
                bag.PushScope();
            }

            var property = GetPropertyForGetObject();

            bag.Current.IsCollectionHoldingObject = true;

            var valueOfProperty = GetValueOfMemberFromParentInstance(property);

            bag.Current.Instance = valueOfProperty;

            if (property.Type.IsContainer)
            {
                bag.Current.Collection = valueOfProperty;
            }
        }

        private XamlMember GetPropertyForGetObject()
        {
            if (bag.Current.Type != null || bag.Depth <= 1)
            {
                return bag.Current.Property;
            }

            return bag.Parent.Property;
        }

        private object GetValueOfMemberFromParentInstance(XamlMember property)
        {
            var parentInstance = bag.Parent.Instance;
            bag.Current.Type = property.Type;

            var valueOfProperty = TypeOperations.GetValue(parentInstance, property);
            return valueOfProperty;
        }
    }
}