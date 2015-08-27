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
            else if (IsName)
            {
                StateCommuter.ValueProcessingMode = ValueProcessingMode.Name;
            }
            else
            {
                throw new XamlParseException($"Unexpected XAML directive. The directive {member} has been found and we don't know how to handle it.");
            }
        }

        public bool IsName => member.Equals(CoreTypes.Name);

        private bool IsInitialization => member.Equals(CoreTypes.Initialization);

        private bool IsKey => member.Equals(CoreTypes.Key);

        private bool IsMarkupExtensionArguments => member.Equals(CoreTypes.MarkupExtensionArguments);
    }
}