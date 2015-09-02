namespace OmniXaml.ObjectAssembler.Commands
{
    using System;
    using System.Collections.ObjectModel;
    using Typing;

    public class StartMemberCommand : Command
    {
        private readonly XamlMemberBase member;

        public StartMemberCommand(ObjectAssembler assembler, XamlMemberBase member) : base(assembler)
        {
            this.member = member;
        }

        private static DirectiveKind GetDirectiveKind(XamlMemberBase xamlMember)
        {
            if (xamlMember.Equals(CoreTypes.Initialization))
            {
                return DirectiveKind.Initialization;
            }
            if (xamlMember.Equals(CoreTypes.Items))
            {
                return DirectiveKind.Items;
            }
            if (xamlMember.Equals(CoreTypes.sKey))
            {
                return DirectiveKind.Key;
            }
            if (xamlMember.Equals(CoreTypes.MarkupExtensionArguments))
            {
                return DirectiveKind.MarkupExtensionArguments;
            }
            if (xamlMember.Equals(CoreTypes.sName))
            {
                return DirectiveKind.Name;
            }

            throw new InvalidOperationException($"Unexpected XAML directive. The directive {xamlMember} has been found and we don't know how to handle it.");
        }

        public override void Execute()
        {
            StateCommuter.Current.XamlMember = member;

            if (member.IsDirective)
            {
                SetCommuterStateAccordingToDirective();
            }
            else
            {
                ForceInstanceCreationOfCurrentTypeAndAssociateIfPossible();
            }
        }

        private void ForceInstanceCreationOfCurrentTypeAndAssociateIfPossible()
        {
            StateCommuter.CreateInstanceOfCurrentXamlTypeIfNotCreatedBefore();
            if (!StateCommuter.Current.WasInstanceAssignedRightAfterBeingCreated)
            {
                StateCommuter.AssociateCurrentInstanceToParentRightAfterCreation();
            }
        }

        private void SetCommuterStateAccordingToDirective()
        {
            switch (GetDirectiveKind(member))
            {               
                case DirectiveKind.Initialization:
                    StateCommuter.ValueProcessingMode = ValueProcessingMode.InitializationValue;
                    break;

                case DirectiveKind.Key:
                    StateCommuter.ValueProcessingMode = ValueProcessingMode.Key;
                    break;

                case DirectiveKind.MarkupExtensionArguments:
                    StateCommuter.Current.CtorArguments = new Collection<ConstructionArgument>();
                    StateCommuter.ValueProcessingMode = ValueProcessingMode.ConstructionParameter;
                    break;

                case DirectiveKind.Name:
                    StateCommuter.ValueProcessingMode = ValueProcessingMode.Name;
                    break;
            }
        }

        private enum DirectiveKind
        {
            Key,
            Items,
            Name,
            MarkupExtensionArguments,
            Initialization
        }
    }
}