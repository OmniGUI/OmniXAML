namespace OmniXaml.NewAssembler
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
            State.Value.XamlType = xamlType;
            var instance = State.Current.Value.XamlType.CreateInstance(null);
            State.Value.Instance = instance;
        }
    }
}