namespace OmniXaml.NewAssembler.Commands
{
    using Typing;

    public class StartMemberCommand : Command
    {
        private readonly XamlMember member;

        public StartMemberCommand(SuperObjectAssembler assembler, XamlMember member) : base(assembler)
        {
            this.member = member;
        }

        public override void Execute()
        {
            StateCommuter.Member = member;

            if (member.IsDirective)
            {
                if (IsMarkupExtensionArguments)
                {
                    StateCommuter.BeginProcessingValuesAsCtorArguments();
                }
            }
            else
            {
                StateCommuter.CreateInstanceOfCurrentXamlTypeIfNotCreatedBefore();
                if (!StateCommuter.WasAssociatedRightAfterCreation)
                {
                    StateCommuter.AssociateCurrentInstanceToParentForCreation();
                }
            }
        }

        private bool IsMarkupExtensionArguments => member.Equals(CoreTypes.MarkupExtensionArguments);
    }
}