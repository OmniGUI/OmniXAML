namespace OmniXaml.NewAssembler.Commands
{
    public class ValueCommand : Command
    {
        private readonly string value;

        public ValueCommand(SuperObjectAssembler superObjectAssembler, string value) : base(superObjectAssembler)
        {
            this.value = value;
        }

        public override void Execute()
        {
            if (!StateCommuter.IsProcessingValuesAsCtorArguments)
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