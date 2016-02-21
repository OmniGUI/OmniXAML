namespace OmniXaml.ObjectAssembler.Commands
{
    using System;
    using System.Collections;

    public class EndObjectCommand : Command
    {
        private readonly Action<StateCommuter> setResult;

        public EndObjectCommand(StateCommuter stateCommuter, Action<StateCommuter> setResult) : base(stateCommuter)
        {
            this.setResult = setResult;
        }

        public override void Execute()
        {
            if (!StateCommuter.Current.IsGetObject)
            {
                StateCommuter.CreateInstanceOfCurrentXamlTypeIfNotCreatedBefore();
                StateCommuter.PutNameToCurrentInstanceIfAny();

                if (StateCommuter.Current.Instance is IMarkupExtension)
                {
                    ProcessCurrentIntanceValueWithMarkupExtension();               
                }

                StateCommuter.AssociateCurrentInstanceToParent();

                StateCommuter.RegisterInstanceNameToNamescope();
                StateCommuter.NotifyEnd();
            }

            setResult(StateCommuter);
            
            StateCommuter.DecreaseLevel();
        }

        private void ProcessCurrentIntanceValueWithMarkupExtension()
        {
            var processedValue = StateCommuter.GetValueProvidedByMarkupExtension((IMarkupExtension) StateCommuter.Current.Instance);
            StateCommuter.Current.Instance = processedValue;

            var collection = processedValue as ICollection;
            if (collection != null)
            {
                StateCommuter.Current.Collection = collection;
            }
        }

        public override string ToString()
        {
            return "End of Object";
        }
    }
}