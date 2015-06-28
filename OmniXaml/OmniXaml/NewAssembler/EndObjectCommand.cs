namespace OmniXaml.NewAssembler
{
    public class EndObjectCommand : Command
    {
        public EndObjectCommand(SuperObjectAssembler assembler) : base(assembler)
        {            
        }

        public override void Execute()
        {
            if (State.CurrentValue.Instance == null)
            {                
                State.CurrentValue.MeterializeCurrentType();
            }

            if (State.Count > 1)
            {
                var child = State.CurrentValue.Instance;
                var parent = State.PreviousValue.Instance;
                var parentProperty = State.PreviousValue.XamlMember;
                parentProperty.SetValue(parent, child);
            }

            Assembler.Result = Current.Instance;
            State.Pop();
        }
    }
}