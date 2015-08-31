namespace OmniXaml.ObjectAssembler.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Typing;

    public class EndMemberCommand : Command
    {
        private readonly ITopDownMemberValueContext topDownMemberValueContext;
        private readonly ITypeContext typeContext;

        public EndMemberCommand(ObjectAssembler assembler, ITopDownMemberValueContext topDownMemberValueContext) : base(assembler)
        {
            this.topDownMemberValueContext = topDownMemberValueContext;
            typeContext = Assembler.WiringContext.TypeContext;
        }

        public override void Execute()
        {
            if (StateCommuter.ValueProcessingMode == ValueProcessingMode.ConstructionParameter)
            {
                AdaptCurrentCtorArgumentsToCurrentType();
                StateCommuter.ValueProcessingMode = ValueProcessingMode.AssignToMember;
            }

            if (IsTherePendingInstanceWaitingToBeAssigned)
            {
                StateCommuter.AssociateCurrentInstanceToParent();
                StateCommuter.DecreaseLevel();
            }

            SaveMemberValueToTopDownEnvironment();
        }

        private void SaveMemberValueToTopDownEnvironment()
        {
            var member = StateCommuter.Current.XamlMember as MutableXamlMember;

            if (member != null && StateCommuter.Current.XamlMember.XamlType.IsCollection)
            {
                var instance = member.GetValue(StateCommuter.Current.Instance);
                topDownMemberValueContext.SetMemberValue(StateCommuter.Current.XamlMember.XamlType, instance);
            }
        }

        public bool IsTherePendingInstanceWaitingToBeAssigned => StateCommuter.Current.HasInstance && StateCommuter.Current.XamlMember == null;

        private void AdaptCurrentCtorArgumentsToCurrentType()
        {
            var arguments = (IList<ConstructionArgument>) StateCommuter.Current.CtorArguments;
            var xamlTypes = GetTypesOfBestCtorMatch(StateCommuter.Current.XamlType, arguments.Count);

            var i = 0;
            foreach (var ctorArg in arguments)
            {
                var targetType = xamlTypes[i];
                var compatibleValue = StateCommuter.ValuePipeline.ConvertValueIfNecessary(ctorArg.StringValue, targetType);
                ctorArg.Value = compatibleValue;
            }
        }

        private IList<XamlType> GetTypesOfBestCtorMatch(XamlType xamlType, int count)
        {
            var constructor = SelectConstructor(xamlType, count);
            return constructor.GetParameters().Select(arg => typeContext.GetXamlType(arg.ParameterType)).ToList();
        }

        private ConstructorInfo SelectConstructor(XamlType xamlType, int count)
        {
            return xamlType.UnderlyingType.GetTypeInfo().DeclaredConstructors.First(info => info.GetParameters().Count() == count);
        }
    }
}