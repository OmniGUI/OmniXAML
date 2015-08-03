namespace OmniXaml.NewAssembler.Commands
{
    using System;
    using System.Runtime.InteropServices.ComTypes;

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
            if (StateCommuter.IsWaitingValueAsInitializationParameter)
            {
                var underlyingType = StateCommuter.XamlType.UnderlyingType;
                StateCommuter.Instance = ValuePipeLine.ConvertValueIfNecessary(value, underlyingType);
            }
            else if (StateCommuter.IsWaitingValueAsKey)
            {
                StateCommuter.SetKey(value);
                StateCommuter.IsWaitingValueAsKey = false;
            }
            else if (StateCommuter.IsProcessingValuesAsCtorArguments)
            {
                StateCommuter.AddCtorArgument(value);                
            }
            else
            {
                StateCommuter.RaiseLevel();
                StateCommuter.Instance = value;
                StateCommuter.AssignChildToParentProperty();
                StateCommuter.DecreaseLevel();
            }
        }
    }
}