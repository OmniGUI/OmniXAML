namespace OmniXaml.ObjectAssembler.Commands
{
    public class ValueCommand : Command
    {
        private readonly string value;

        public ValueCommand(ObjectAssembler objectAssembler, ITopDownValueContext topDownValueContext, string value) : base(objectAssembler)
        {
            this.value = value;
            ValuePipeLine = new ValuePipeline(objectAssembler.TypeSource, topDownValueContext);
        }

        private ValuePipeline ValuePipeLine { get; }

        public override void Execute()
        {
            switch (StateCommuter.ValueProcessingMode)
            {
                case ValueProcessingMode.InitializationValue:
                    StateCommuter.Current.Instance = ValuePipeLine.ConvertValueIfNecessary(value, StateCommuter.Current.XamlType);
            
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