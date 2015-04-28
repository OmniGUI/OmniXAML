namespace OmniXaml.Assembler.NodeWriters
{
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

            var property = bag.Current.Type != null || bag.Depth <= 1
                               ? bag.Current.Property
                               : bag.Parent.Property;           

            if (bag.Current.Type != null)
            {
                bag.PushScope();
            }

            bag.Current.IsObjectFromMember = true;

            var parentInstance = bag.Parent.Instance;
            bag.Current.Type = property.Type;

            var valueOfProperty = TypeOperations.GetValue(parentInstance, property);
           
            bag.Current.Instance = valueOfProperty;

            if (property.Type.IsContainer)
            {
                bag.Current.Collection = valueOfProperty;
            }
        }
    }
}