namespace OmniXaml.NewAssembler.Commands
{
    public class ValueCommand : Command
    {
        private readonly object value;

        public ValueCommand(SuperObjectAssembler superObjectAssembler, object value) : base(superObjectAssembler)
        {
            this.value = value;
        }

        public override void Execute()
        {
            this.StateCommuter.Instance = value;
            this.StateCommuter.AssignChildToParentProperty();
            this.StateCommuter.DecreaseLevel();
        }
    }
}