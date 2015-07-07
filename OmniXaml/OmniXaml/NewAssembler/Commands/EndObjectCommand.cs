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
                }

                Assembler.Result = StateCommuter.Instance;
                StateCommuter.AssociateCurrentInstanceToParent();
            }

            StateCommuter.DecreaseLevel();
        } 
    }
}