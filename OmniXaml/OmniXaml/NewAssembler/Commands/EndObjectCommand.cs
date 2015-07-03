namespace OmniXaml.NewAssembler.Commands
{
    public class EndObjectCommand : Command
    {
        public EndObjectCommand(SuperObjectAssembler assembler) : base(assembler)
        {            
        }

        public override void Execute()
        {
            if (!StateCommuter.IsObjectFromMember)
            {
                StateCommuter.CreateInstanceOfCurrentXamlTypeIfNotCreatedBefore();
                Assembler.Result = StateCommuter.Instance;

                if (StateCommuter.Level > 1)
                {
                    if (StateCommuter.IsPreviousHoldingChildrenIntoACollection)
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