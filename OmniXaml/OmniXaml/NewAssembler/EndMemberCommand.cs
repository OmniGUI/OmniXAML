namespace OmniXaml.NewAssembler
{
    public class EndMemberCommand : Command
    {
        public override void Execute()
        {
        }

        public EndMemberCommand(SuperObjectAssembler assembler) : base(assembler)
        {
            if (IsLeafMember)
            {
                var child = State.CurrentValue.Instance;
                var parent = State.PreviousValue.Instance;
                var parentProperty = State.PreviousValue.XamlMember;
                parentProperty.SetValue(parent, child);

                State.Pop();
            }

            State.CurrentValue.XamlMember = null;
        }

        private bool IsLeafMember => State.CurrentValue.XamlMember == null;
    }
}