namespace OmniXaml.NewAssembler.Commands
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
            if (member.IsDirective)
            {
                return;
            }

            StateCommuter.CreateInstanceOfCurrentXamlTypeIfNotCreatedBefore();            
            StateCommuter.Member = member;
            StateCommuter.RaiseLevel();
        }      
    }
}