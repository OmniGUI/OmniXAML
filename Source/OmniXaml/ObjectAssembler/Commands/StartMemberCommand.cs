namespace OmniXaml.ObjectAssembler.Commands
{
    using System;
    using System.Collections;
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
            if (xamlMember.Equals(CoreTypes.Key))
            {
                return DirectiveKind.Key;
            }
            if (xamlMember.Equals(CoreTypes.MarkupExtensionArguments))
            {
                return DirectiveKind.MarkupExtensionArguments;
            }
            if (xamlMember.Equals(CoreTypes.Name))
            {
                return DirectiveKind.Name;
            }
            if (xamlMember.Equals(CoreTypes.UnknownContent))
            {
                return DirectiveKind.UnknownContent;
            }


            throw new InvalidOperationException($"Unexpected XAML directive. The directive {xamlMember} has been found and we don't know how to handle it.");
        }

        public override void Execute()
        {
            var realMember = member;

            var mutable = member as MutableXamlMember;

            if (mutable!=null && IsMemberEquivalentToNameDirective(mutable))
            {
                realMember = CoreTypes.Name;
            }

            StateCommuter.Current.XamlMember = realMember;

            if (realMember.IsDirective)
            {
                SetCommuterStateAccordingToDirective(realMember);
            }
            else
            {
                ForceInstanceCreationOfCurrentType();
            }
        }

        private static bool IsMemberEquivalentToNameDirective(MutableXamlMember memberToCheck)
        {
            return Equals(memberToCheck, memberToCheck.DeclaringType.RuntimeNamePropertyMember);
        }

        private void ForceInstanceCreationOfCurrentType()
        {
            StateCommuter.CreateInstanceOfCurrentXamlTypeIfNotCreatedBefore();
        }

        private void SetCommuterStateAccordingToDirective(XamlMemberBase xamlMemberBase)
        {
            switch (GetDirectiveKind(xamlMemberBase))
            {
                case DirectiveKind.Items:
                    if (StateCommuter.HasParent && !StateCommuter.ParentIsOneToMany)
                    {
                        throw new XamlParseException("Cannot assign a more than one item to a member that is not a collection.");
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