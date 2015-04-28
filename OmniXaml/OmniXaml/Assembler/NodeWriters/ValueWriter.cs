namespace OmniXaml.Assembler.NodeWriters
{
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
            objectAssembler.SetUnfinishedResult();
            bag.PushScope();
            bag.Current.Instance = value;            
        }
    }
}