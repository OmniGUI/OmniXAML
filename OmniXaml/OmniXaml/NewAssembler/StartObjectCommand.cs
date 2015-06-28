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
            if (State.CurrentValue.XamlType != null)
            {
                State.Push(new Level());
            }

            State.CurrentValue.XamlType = xamlType;          
        }
    }
}