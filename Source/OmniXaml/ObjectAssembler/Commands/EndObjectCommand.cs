namespace OmniXaml.ObjectAssembler.Commands
{
    using System;
    using System.Collections;

    public class EndObjectCommand : Command
    {
        private readonly Action<StateCommuter> setResult;
        private readonly IInstanceLifeCycleListener lifyCycleListener;

        public EndObjectCommand(StateCommuter stateCommuter, Action<StateCommuter> setResult, IInstanceLifeCycleListener lifyCycleListener) : base(stateCommuter)
        {
            this.setResult = setResult;
            this.lifyCycleListener = lifyCycleListener;
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
                else if (!StateCommuter.WasAssociatedRightAfterCreation)
                {
                    StateCommuter.AssociateCurrentInstanceToParent();
                }

                StateCommuter.RegisterInstanceNameToNamescope();
                lifyCycleListener.OnAfterProperties(StateCommuter.Current.Instance);
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

            StateCommuter.AssociateCurrentInstanceToParent();
        }

        public override string ToString()
        {
            return "End of Object";
        }
    }
}