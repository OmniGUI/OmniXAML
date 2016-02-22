namespace OmniXaml.ObjectAssembler.Commands
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using Typing;

    public class StartMemberCommand : Command
    {
        private readonly MemberBase member;

        public StartMemberCommand(StateCommuter stateCommuter, MemberBase member) : base(stateCommuter)
        {
            this.member = member;
        }

        private static DirectiveKind GetDirectiveKind(MemberBase member)
        {
            if (member.Equals(CoreTypes.Initialization))
            {
                return DirectiveKind.Initialization;
            }
            if (member.Equals(CoreTypes.Items))
            {
                return DirectiveKind.Items;
            }
            if (member.Equals(CoreTypes.Key))
            {
                return DirectiveKind.Key;
            }
            if (member.Equals(CoreTypes.MarkupExtensionArguments))
            {
                return DirectiveKind.MarkupExtensionArguments;
            }
            if (member.Equals(CoreTypes.Name))
            {
                return DirectiveKind.Name;
            }
            if (member.Equals(CoreTypes.UnknownContent))
            {
                return DirectiveKind.UnknownContent;
            }


            throw new InvalidOperationException($"Unexpected XAML directive. The directive {member} has been found and we don't know how to handle it.");
        }

        public override void Execute()
        {
            var realMember = member;

            var mutable = member as MutableMember;

            if (mutable!=null && IsMemberEquivalentToNameDirective(mutable))
            {
                realMember = CoreTypes.Name;
            }

            StateCommuter.Current.Member = realMember;

            if (realMember.IsDirective)
            {
                SetCommuterStateAccordingToDirective(realMember);
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

        private static bool IsMemberEquivalentToNameDirective(MutableMember memberToCheck)
        {
            return Equals(memberToCheck, memberToCheck.DeclaringType.RuntimeNamePropertyMember);
        }

        private void ForceInstanceCreationOfCurrentType()
        {
            StateCommuter.CreateInstanceOfCurrentXamlTypeIfNotCreatedBefore();
        }

        private void SetCommuterStateAccordingToDirective(MemberBase memberBase)
        {
            switch (GetDirectiveKind(memberBase))
            {
                case DirectiveKind.Items:
                    if (StateCommuter.HasParent && !StateCommuter.ParentIsOneToMany)
                    {
                        throw new ParseException("Cannot assign a more than one item to a member that is not a collection.");
                    }

                    if (!StateCommuter.Current.IsGetObject)
                    {
                        AccommodateLevelsForIncomingChildren();
                    }
                    break;

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
                case DirectiveKind.UnknownContent:
                    AccommodateLevelsForIncomingChildren();
                    break;
            }
        }

        private void AccommodateLevelsForIncomingChildren()
        {
            StateCommuter.CreateInstanceOfCurrentXamlTypeIfNotCreatedBefore();
            var instance = StateCommuter.Current.Instance;

            var collection = instance as ICollection;
            if (collection != null)
            {
                StateCommuter.Current.Collection = collection;
            }
        }

        private enum DirectiveKind
        {
            Key,
            Items,
            Name,
            MarkupExtensionArguments,
            Initialization,
            UnknownContent
        }

        public override string ToString()
        {
            return $"Start Of Member: {member}";
        }
    }
}