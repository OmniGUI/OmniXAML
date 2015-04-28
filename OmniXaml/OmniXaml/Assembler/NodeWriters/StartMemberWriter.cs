namespace OmniXaml.Assembler.NodeWriters
{
    using Typing;

    internal class StartMemberWriter
    {
        private readonly ObjectAssembler objectAssembler;
        private readonly StateBag bag;

        public StartMemberWriter(ObjectAssembler objectAssembler)
        {
            this.objectAssembler = objectAssembler;
            bag = objectAssembler.Bag;
        }

        public void WriteStartMember(XamlMember property)
        {
            bag.Current.Property = property;

            if (bag.Current.Instance == null)
            {
                objectAssembler.PrepareNewInstanceBecauseWeWantToConfigureIt(bag);
            }
        }
    }
}