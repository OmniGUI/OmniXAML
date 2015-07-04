namespace OmniXaml.NewAssembler.Commands
{
    public class EndObjectCommand : Command
    {
        public EndObjectCommand(SuperObjectAssembler assembler) : base(assembler)
        {            
        }

        public override void Execute()
        {
            if (!StateCommuter.IsGetObject)
            {
                StateCommuter.CreateInstanceOfCurrentXamlTypeIfNotCreatedBefore();
                Assembler.Result = StateCommuter.Instance;

                if (StateCommuter.Level > 1)
                {
                    if (StateCommuter.PreviousIsHostingChildren)
                    {
                        StateCommuter.AssignChildToCurrentCollection();
                    }
                    else
                    {
                        StateCommuter.AssignChildToParentProperty();
                    }                
                }
            }

            StateCommuter.DecreaseLevel();
        }
    }
}