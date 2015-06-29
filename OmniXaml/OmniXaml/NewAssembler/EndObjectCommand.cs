namespace OmniXaml.NewAssembler
{
    public class EndObjectCommand : Command
    {
        public EndObjectCommand(SuperObjectAssembler assembler) : base(assembler)
        {            
        }

        public override void Execute()
        {
            var assemblerStack = State;

            if (State.HasCurrentInstance)
            {                
                assemblerStack.CurrentValue.MeterializeType();
            }

            if (State.Count > 1)
            {
                State.AssignChildToParent();
            }

            Assembler.Result = Current.Instance;
            assemblerStack.Pop();
        }
    }
}