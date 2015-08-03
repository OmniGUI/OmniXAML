namespace OmniXaml.NewAssembler.Commands
{
    using System.Runtime.InteropServices.ComTypes;

    public class ValueCommand : Command
    {
        private readonly string value;

        public ValueCommand(SuperObjectAssembler superObjectAssembler, string value) : base(superObjectAssembler)
        {
            this.value = value;
        }

        public override void Execute()
        {
            if (StateCommuter.IsWaitingValueAsInitializationParameter)
            {
                StateCommuter.Instance = value;
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