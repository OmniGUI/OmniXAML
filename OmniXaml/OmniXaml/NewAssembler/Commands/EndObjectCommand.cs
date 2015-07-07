namespace OmniXaml.NewAssembler.Commands
{
    using System.Reflection;

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

                if (StateCommuter.Instance is MarkupExtension)
                {
                    StateCommuter.Instance = StateCommuter.ReplaceInstanceByValueProvidedByMarkupExtension((MarkupExtension)StateCommuter.Instance);
                    StateCommuter.AssociateCurrentInstanceToParent();
                }
                else if (!StateCommuter.WasAssociatedRightAfterCreation)
                {
                    StateCommuter.AssociateCurrentInstanceToParent();
                }
            }

            Assembler.Result = StateCommuter.Instance;
            StateCommuter.DecreaseLevel();
        }
    }
}