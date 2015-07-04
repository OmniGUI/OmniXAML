namespace OmniXaml.NewAssembler.Commands
{
    using Typing;

    public class StartObjectCommand : Command
    {
        private readonly XamlType xamlType;

        public StartObjectCommand(SuperObjectAssembler assembler, XamlType xamlType) : base(assembler)
        {
            this.xamlType = xamlType;
        }

        public override void Execute()
        {
            if (ConflictsWithObjectBeingConfigured)
            {
                StateCommuter.RaiseLevel();
            }

            StateCommuter.XamlType = xamlType;
        }

        private bool ConflictsWithObjectBeingConfigured => StateCommuter.HasCurrentInstance || StateCommuter.IsGetObject;
    }
}