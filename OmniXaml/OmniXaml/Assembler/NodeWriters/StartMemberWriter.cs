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

        public void WriteStartMember(XamlMemberBase property)
        {
            bag.Current.Property = property;

            if (bag.Current.Instance == null)
            {
                if (property.IsDirective && !IsConstructionDirective((XamlDirective) property))
                {
                    objectAssembler.PrepareNewInstanceBecauseWeWantToConfigureIt(bag);
                }
                if (Equals(property, CoreTypes.MarkupExtensionArguments))
                {
                    SetupCurrentCollectionToHoldPositionalArguments();
                }
            }
            
        }

        private bool IsConstructionDirective(XamlDirective xamlMember)
        {
            return Equals(xamlMember, CoreTypes.MarkupExtensionArguments);
        }

        private void SetupCurrentCollectionToHoldPositionalArguments()
        {
            bag.Current.Collection = new List<MarkupExtensionArgument>();
        }
    }
}