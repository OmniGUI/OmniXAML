namespace OmniXaml.Assembler.NodeWriters
{
    using Typing;

    public class ValueWriter
    {
        private readonly ObjectAssembler objectAssembler;
        private readonly StateBag bag;

        public ValueWriter(ObjectAssembler objectAssembler)
        {
            this.objectAssembler = objectAssembler;
            bag = objectAssembler.Bag;
        }

        public void WriteValue(object value)
        {
            var previousProperty = bag.Current.Property;

            objectAssembler.SetUnfinishedResult();
            bag.PushScope();
            bag.Current.Instance = value;
           
            if (previousProperty.IsDirective)
            {
                if (previousProperty.Equals(CoreTypes.MarkupExtensionArguments))
                {
                    bag.Current.Instance = new MarkupExtensionArgument(value, true);
                    objectAssembler.AssignCurrentInstanceToParentCollection();
                    bag.PopScope();
                }
            }
        }
    }
}