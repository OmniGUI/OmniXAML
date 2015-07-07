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
                    ReplaceInstanceByValueProvidedByMarkupExtension((MarkupExtension) StateCommuter.Instance);
                }

                Assembler.Result = StateCommuter.Instance;
                StateCommuter.AssociateCurrentInstanceToParent();
            }

            StateCommuter.DecreaseLevel();
        }

        private void ReplaceInstanceByValueProvidedByMarkupExtension(MarkupExtension instance)
        {
            var markupExtensionContext = GetExtensionContext();
            StateCommuter.Instance = instance.ProvideValue(markupExtensionContext);
        }

        private MarkupExtensionContext GetExtensionContext()
        {
            var inflationContext = new MarkupExtensionContext
            {
                TargetObject = StateCommuter.PreviousInstance,
                TargetProperty = StateCommuter.PreviousInstance.GetType().GetRuntimeProperty(StateCommuter.PreviousMember.Name),
                TypeRepository = Assembler.WiringContext.TypeContext,
            };

            return inflationContext;
        }

    }
}