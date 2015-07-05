namespace OmniXaml.NewAssembler.Commands
{
    using Typing;

    public class StartObjectCommand : Command
    {
        private readonly XamlType xamlType;
        private readonly object rootInstance;

        public StartObjectCommand(SuperObjectAssembler assembler, XamlType xamlType, object rootInstance) : base(assembler)
        {
            this.xamlType = xamlType;
            this.rootInstance = rootInstance;
        }

        public override void Execute()
        {            
            if (ConflictsWithObjectBeingConfigured)
            {
                StateCommuter.RaiseLevel();
            }

            StateCommuter.XamlType = xamlType;
            OverrideCurrentInstanceOnFirstLevel();
        }

        private bool ConflictsWithObjectBeingConfigured => StateCommuter.HasCurrentInstance || StateCommuter.IsGetObject;

        private void OverrideCurrentInstanceOnFirstLevel()
        {
            if (StateCommuter.Level == 1 && rootInstance != null)
            {
                StateCommuter.Instance = rootInstance;
            }
        }
    }
}