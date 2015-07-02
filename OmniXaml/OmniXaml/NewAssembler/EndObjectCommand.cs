namespace OmniXaml.NewAssembler
{
    using Assembler;

    public class EndObjectCommand : Command
    {
        public EndObjectCommand(SuperObjectAssembler assembler) : base(assembler)
        {            
        }

        public override void Execute()
        {
            if (!State.CurrentValue.IsCollectionHolderObject)
            {
                if (State.HasCurrentInstance)
                {
                    State.CurrentValue.MaterializeType();
                }

                if (State.Count > 1)
                {
                    if (State.PreviousValue.XamlMember.Type.IsCollection)
                    {
                        TypeOperations.Add(State.PreviousValue.Instance, State.CurrentValue.Instance);
                    }
                    else
                    {
                        State.AssignChildToParentProperty();
                    }                    
                }
            }

            Assembler.Result = Current.Instance;
            State.Pop();
        }
    }
}