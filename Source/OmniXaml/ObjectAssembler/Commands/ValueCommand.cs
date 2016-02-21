namespace OmniXaml.ObjectAssembler.Commands
{
    using TypeConversion;
    using Typing;

    public class ValueCommand : Command
    {
        private readonly IValueContext valueContext;
        private readonly string value;

        public ValueCommand(StateCommuter stateCommuter, IValueContext valueContext, string value) : base(stateCommuter)
        {
            this.valueContext = valueContext;
            this.value = value;
        }

        public override void Execute()
        {
            switch (StateCommuter.ValueProcessingMode)
            {
                case ValueProcessingMode.InitializationValue:
                    object compatibleValue;
                    CommonValueConversion.TryConvert(value, StateCommuter.Current.XamlType, valueContext, out compatibleValue);
                    StateCommuter.Current.Instance = compatibleValue;
                    StateCommuter.ValueProcessingMode = ValueProcessingMode.AssignToMember;
                    break;

                case ValueProcessingMode.Key:
                    StateCommuter.SetKey(value);
                    StateCommuter.ValueProcessingMode = ValueProcessingMode.AssignToMember;
                    break;

                case ValueProcessingMode.ConstructionParameter:
                    StateCommuter.AddCtorArgument(value);
                    break;

                case ValueProcessingMode.AssignToMember:
                    StateCommuter.RaiseLevel();
                    StateCommuter.Current.Instance = value;
                    StateCommuter.AssociateCurrentInstanceToParent();
                    StateCommuter.DecreaseLevel();
                    break;

                case ValueProcessingMode.Name:
                    StateCommuter.SetNameForCurrentInstance(value);
                    StateCommuter.ValueProcessingMode = ValueProcessingMode.AssignToMember;
                    break;

                default:
                    throw new ParseException(
                        "Unexpected mode was set trying to process a Value XAML instruction. " +
                        $"We found \"{StateCommuter.ValueProcessingMode}\") and it cannot be handled.");
            }                                    
        }

        public override string ToString()
        {
            return $"Value: {value}";
        }
    }
}