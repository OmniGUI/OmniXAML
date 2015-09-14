namespace OmniXaml.ObjectAssembler.Commands
{
    public class ValueCommand : Command
    {
        private readonly string value;

        public ValueCommand(ObjectAssembler objectAssembler, ITopDownValueContext topDownValueContext, string value) : base(objectAssembler)
        {
            this.value = value;
            ValuePipeLine = new ValuePipeline(objectAssembler.WiringContext.TypeContext, topDownValueContext);
        }

        private ValuePipeline ValuePipeLine { get; set; }

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
                    StateCommuter.AssignChildToParentProperty();
                    StateCommuter.DecreaseLevel();
                    break;

                case ValueProcessingMode.Name:
                    StateCommuter.SetNameForCurrentInstance(value);
                    StateCommuter.ValueProcessingMode = ValueProcessingMode.AssignToMember;
                    break;

                default:
                    throw new XamlParseException(
                        "Unexpected mode was set trying to process a Value XAML instruction. " +
                        $"We found \"{StateCommuter.ValueProcessingMode}\") and it cannot be handled.");
            }            
        }
    }
}