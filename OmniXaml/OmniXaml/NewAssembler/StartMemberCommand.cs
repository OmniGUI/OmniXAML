namespace OmniXaml.NewAssembler
{
    using System;
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
            State.Value.XamlMember = member;
        }
    }
}