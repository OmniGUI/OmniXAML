namespace OmniXaml.Assembler.NodeWriters
{
    using Typing;

    public class StartObjectWriter
    {
        private readonly ObjectAssembler objectAssembler;
        private readonly object rootInstance;

        private readonly StateBag bag;
        
        public StartObjectWriter(ObjectAssembler objectAssembler, object rootInstance = null)
        {
            this.objectAssembler = objectAssembler;
            this.rootInstance = rootInstance;
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

            bag.Current.Instance = rootInstance;

            bag.Current.Type = type;
        }
    }
}