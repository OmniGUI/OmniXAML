namespace OmniXaml.NewAssembler
{
    using Typing;

    public class StartMemberCommand : Command
    {
        private readonly XamlMember member;

        public StartMemberCommand(SuperObjectAssembler assembler, XamlMember member) : base(assembler)
        {
            this.member = member;
        }

        public override void Execute()
        {
            State.CurrentValue.XamlMember = member;

            if (State.CurrentValue.Instance == null)
            {
                State.CurrentValue.MeterializeType();
            }
        }      
    }
}