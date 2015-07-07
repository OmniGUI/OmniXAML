namespace OmniXaml.NewAssembler.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Typing;

    public class EndMemberCommand : Command
    {
        private readonly ITypeContext typeContext;

        public EndMemberCommand(SuperObjectAssembler assembler) : base(assembler)
        {
            typeContext = Assembler.WiringContext.TypeContext;
        }

        public override void Execute()
        {
            if (StateCommuter.IsProcessingValuesAsCtorArguments)
            {
                AdaptCurrentCtorArgumentsToCurrentType();
            }

            if (IsTherePendingInstanceWaitingToBeAssigned)
            {
                StateCommuter.AssociateCurrentInstanceToParent();
                StateCommuter.DecreaseLevel();
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
                var compatibleValue = StateCommuter.ConvertValueIfNecessary(ctorArg.StringValue, targetType.UnderlyingType);
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