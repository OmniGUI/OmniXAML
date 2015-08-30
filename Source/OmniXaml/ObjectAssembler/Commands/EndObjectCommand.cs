namespace OmniXaml.ObjectAssembler.Commands
{
    public class EndObjectCommand : Command
    {
        public EndObjectCommand(ObjectAssembler assembler) : base(assembler)
        {
        }

        public override void Execute()
        {
            if (!StateCommuter.IsGetObject)
            {
                StateCommuter.CreateInstanceOfCurrentXamlTypeIfNotCreatedBefore();

                if (StateCommuter.Instance is IMarkupExtension)
                {
                    StateCommuter.Instance = StateCommuter.GetValueProvidedByMarkupExtension((IMarkupExtension)StateCommuter.Instance);
                    StateCommuter.AssociateCurrentInstanceToParent();
                }
                else if (!StateCommuter.WasInstanceAssignedRightAfterBeingCreated)
                {
                    StateCommuter.AssociateCurrentInstanceToParent();
                }
            }

            Assembler.Result = StateCommuter.Instance;
            StateCommuter.DecreaseLevel();
        }
    }
}