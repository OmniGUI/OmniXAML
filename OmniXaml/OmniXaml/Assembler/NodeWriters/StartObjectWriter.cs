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

            bag.Current.Type = type;

            OverrideCurrentInstanceOnFirstLevel();
        }

        private void OverrideCurrentInstanceOnFirstLevel()
        {
            if (bag.LiveDepth == 1 && rootInstance != null)
            {
                bag.Current.Instance = rootInstance;
            }
        }
    }
}