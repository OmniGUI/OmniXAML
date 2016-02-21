namespace OmniXaml.ObjectAssembler.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using TypeConversion;
    using Typing;

    public class EndMemberCommand : Command
    {
        private readonly ITypeRepository typeRepository;
        private readonly IValueContext valueContext;

        public EndMemberCommand(ITypeRepository typeRepository, StateCommuter stateCommuter, IValueContext valueContext) : base(stateCommuter)
        {
            this.typeRepository = typeRepository;
            this.valueContext = valueContext;
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
                object compatibleValue;
                CommonValueConversion.TryConvert(ctorArg.StringValue, targetType, valueContext, out compatibleValue);
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
            var declaredConstructors = xamlType.UnderlyingType.GetTypeInfo().DeclaredConstructors;
            return declaredConstructors.First(info => info.GetParameters().Length == count);
        }

        public override string ToString()
        {
            return "End of Member";
        }
    }
}