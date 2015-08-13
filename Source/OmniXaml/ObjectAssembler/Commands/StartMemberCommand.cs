namespace OmniXaml.ObjectAssembler.Commands
{
    using System.Collections.ObjectModel;
    using Typing;

    public class StartMemberCommand : Command
    {
        private readonly XamlMemberBase member;

        public StartMemberCommand(ObjectAssembler assembler, XamlMemberBase member) : base(assembler)
        {
            this.member = member;
        }

        public override void Execute()
        {
            StateCommuter.Member = member;

            if (member.IsDirective)
            {
                SetCommuterStateAccordingToDirective();
            }
            else
            {
                CreateInstanceOfCurrentTypeAndAssociateIfPossible();
            }
        }

        private void CreateInstanceOfCurrentTypeAndAssociateIfPossible()
        {
            StateCommuter.CreateInstanceOfCurrentXamlTypeIfNotCreatedBefore();
            if (!StateCommuter.WasAssociatedRightAfterCreation)
            {
                StateCommuter.AssociateCurrentInstanceToParentForCreation();
            }
        }

        private void SetCommuterStateAccordingToDirective()
        {
            if (IsMarkupExtensionArguments)
            {
                StateCommuter.CurrentCtorParameters = new Collection<ConstructionArgument>();
                StateCommuter.ValueProcessingMode = ValueProcessingMode.ConstructionParameter;
            }
            else if (IsKey)
            {
                StateCommuter.ValueProcessingMode = ValueProcessingMode.Key;
            }
            else if (IsInitialization)
            {
                StateCommuter.ValueProcessingMode = ValueProcessingMode.InitializationValue;
            }
        }

        private bool IsInitialization => member.Equals(CoreTypes.Initialization);

        private bool IsKey => member.Equals(CoreTypes.Key);

        private bool IsMarkupExtensionArguments => member.Equals(CoreTypes.MarkupExtensionArguments);
    }
}