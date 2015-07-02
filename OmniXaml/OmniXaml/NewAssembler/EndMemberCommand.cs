namespace OmniXaml.NewAssembler
{
    public class EndMemberCommand : Command
    {
        public override void Execute()
        {
        }

        public EndMemberCommand(SuperObjectAssembler assembler) : base(assembler)
        {
            if (ShouldAssignCurrentInstanceToParent)
            {
                State.AssignChildToParentProperty();
                State.Pop();
            }

            State.CurrentValue.XamlMember = null;
        }

        private bool ShouldAssignCurrentInstanceToParent => State.CurrentValue.XamlMember == null;
    }
}