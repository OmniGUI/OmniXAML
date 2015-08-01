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
            if (StateCommuter.IsWaitingValueAsKey)
            {
                StateCommuter.Key = value;
                StateCommuter.IsWaitingValueAsKey = false;
            }
            else if (!StateCommuter.IsProcessingValuesAsCtorArguments)
            {
                StateCommuter.RaiseLevel();
                StateCommuter.Instance = value;
                StateCommuter.AssignChildToParentProperty();
                StateCommuter.DecreaseLevel();
            }

            else
            {
                StateCommuter.AddCtorArgument(value);
            }
        }
    }
}