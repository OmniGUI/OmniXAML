namespace OmniXaml.NewAssembler.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Typing;

    public class EndMemberCommand : Command
    {
        private readonly ITopDownMemberValueContext topDownMemberValueContext;
        private readonly ITypeContext typeContext;

        public EndMemberCommand(SuperObjectAssembler assembler, ITopDownMemberValueContext topDownMemberValueContext) : base(assembler)
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
            var member = StateCommuter.Member as MutableXamlMember;

            if (member != null && StateCommuter.Member.XamlType.IsCollection)
            {
                var instance = member.GetValue(StateCommuter.Instance);
                topDownMemberValueContext.SetMemberValue(StateCommuter.Member.XamlType, instance);
            }
        }

        public bool IsTherePendingInstanceWaitingToBeAssigned => StateCommuter.HasCurrentInstance && StateCommuter.Member == null;

        private void AdaptCurrentCtorArgumentsToCurrentType()
        {
            var arguments = StateCommuter.CtorArguments;
            var xamlTypes = GetTypesOfBestCtorMatch(StateCommuter.XamlType, arguments.Count);

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
            return constructor.GetParameters().Select(p => typeContext.GetXamlType(p.ParameterType)).ToList();
        }

        private ConstructorInfo SelectConstructor(XamlType xamlType, int count)
        {
            return xamlType.UnderlyingType.GetTypeInfo().DeclaredConstructors.First(info => info.GetParameters().Count() == count);
        }
    }
}