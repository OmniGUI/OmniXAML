namespace OmniXaml.ObjectAssembler.Commands
{
    public class ValueCommand : Command
    {
        private readonly string value;

        public ValueCommand(ObjectAssembler objectAssembler, string value) : base(objectAssembler)
        {
            this.value = value;
            ValuePipeLine = new ValuePipeline(objectAssembler.WiringContext.TypeContext);
        }

        private ValuePipeline ValuePipeLine { get; set; }

        public override void Execute()
        {
            switch (StateCommuter.ValueProcessingMode)
            {
                case ValueProcessingMode.InitializationValue:
                    StateCommuter.Instance = ValuePipeLine.ConvertValueIfNecessary(value, StateCommuter.XamlType);
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
                    StateCommuter.Instance = value;
                    StateCommuter.AssignChildToParentProperty();
                    StateCommuter.DecreaseLevel();
                    break;

                default:
                    throw new XamlParsingException($"{StateCommuter.ValueProcessingMode} has a value that is not expected.");
            }            
        }
    }
}