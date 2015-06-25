namespace OmniXaml.Assembler.NodeWriters
{
    using System;
    using System.Collections.Generic;
    using Parsers.MarkupExtensions;
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
                if (!IsConstructionDirective(property))
                {
                    objectAssembler.PrepareNewInstanceBecauseWeWantToConfigureIt(bag);
                }
                if (Equals(property, CoreTypes.MarkupExtensionArguments))
                {
                    SetupCurrentCollectionToHoldPositionalArguments();
                }
            }
            
        }

        private bool IsConstructionDirective(XamlMember xamlMember)
        {
            return Equals(xamlMember, CoreTypes.MarkupExtensionArguments);
        }

        private void SetupCurrentCollectionToHoldPositionalArguments()
        {
            bag.Current.Collection = new List<MarkupExtensionArgument>();
        }
    }

    internal class MarkupExtensionArgument
    {
        public bool WasText { get; }
        public object Value { get; }

        public MarkupExtensionArgument(object value, bool wasText)
        {
            this.Value = value;
            this.WasText = wasText;
        }
    }
}