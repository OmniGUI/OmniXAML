namespace OmniXaml.ObjectAssembler.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Typing;

    public class EndMemberCommand : Command
    {
        private readonly ITypeRepository typeRepository;

        public EndMemberCommand(ITypeRepository typeRepository, StateCommuter stateCommuter) : base(stateCommuter)
        {
            this.typeRepository = typeRepository;
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

        private bool IsTherePendingInstanceWaitingToBeAssigned => StateCommuter.Current.HasInstance && StateCommuter.Current.Member == null;

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
                i++;
            }
        }

        private IList<XamlType> GetTypesOfBestCtorMatch(XamlType xamlType, int count)
        {
            var constructor = SelectConstructor(xamlType, count);
            return constructor.GetParameters().Select(arg => typeRepository.GetByType(arg.ParameterType)).ToList();
        }

        private static ConstructorInfo SelectConstructor(XamlType xamlType, int count)
        {
            return xamlType.UnderlyingType.GetTypeInfo().DeclaredConstructors.First(info => info.GetParameters().Count() == count);
        }

        public override string ToString()
        {
            return "End of Member";
        }
    }
}