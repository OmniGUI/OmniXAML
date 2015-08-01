namespace OmniXaml.NewAssembler.Commands
{
    using Typing;

    public class StartMemberCommand : Command
    {
        private readonly XamlMemberBase member;

        public StartMemberCommand(SuperObjectAssembler assembler, XamlMemberBase member) : base(assembler)
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
                else if (IsKey)
                {
                    StateCommuter.IsWaitingValueAsKey = true;
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

        private bool IsKey => member.Equals(CoreTypes.Key);

        private bool IsMarkupExtensionArguments => member.Equals(CoreTypes.MarkupExtensionArguments);
    }
}