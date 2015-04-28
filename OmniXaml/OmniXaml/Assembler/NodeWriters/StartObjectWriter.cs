namespace OmniXaml.Assembler.NodeWriters
{
    using Typing;

    public class StartObjectWriter
    {
        private readonly ObjectAssembler objectAssembler;

        private readonly StateBag bag;

        public StartObjectWriter(ObjectAssembler objectAssembler)
        {
            this.objectAssembler = objectAssembler;
            bag = objectAssembler.Bag;
        }

        private bool IsThereAnotherObjectBeingConfigured => bag.Current.Type != null;

        public void WriteStartObject(XamlType type)
        {
            objectAssembler.SetUnfinishedResult();

            if (IsThereAnotherObjectBeingConfigured)
            {
                bag.PushScope();
            }

            bag.Current.Type = type;
        }
    }
}