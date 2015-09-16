namespace OmniXaml.ObjectAssembler.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Typing;

    public class EndMemberCommand : Command
    {
        private readonly ITopDownValueContext topDownValueContext;
        private readonly ITypeContext typeContext;

        public EndMemberCommand(ObjectAssembler assembler, ITopDownValueContext topDownValueContext) : base(assembler)
        {
            this.topDownValueContext = topDownValueContext;
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