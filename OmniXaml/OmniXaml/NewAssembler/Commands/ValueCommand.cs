namespace OmniXaml.NewAssembler.Commands
{
    using System;

    public class ValueCommand : Command
    {
        private readonly string value;

        public ValueCommand(SuperObjectAssembler superObjectAssembler, string value) : base(superObjectAssembler)
        {
            this.value = value;
            ValuePipeLine = new ValuePipeline(superObjectAssembler.WiringContext);
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
                    throw new InvalidOperationException($"{StateCommuter.ValueProcessingMode} has a value that is not expected.");
            }            
        }
    }
}