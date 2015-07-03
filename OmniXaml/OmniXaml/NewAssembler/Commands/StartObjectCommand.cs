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
            if (StateCommuter.HasCurrentInstance)
            {
                StateCommuter.RaiseLevel();
            }

            StateCommuter.XamlType = xamlType;
        }
    }
}