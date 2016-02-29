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

                var instance = StateCommuter.Current.Instance;

                if (instance is IMarkupExtension)
                {
                    ProcessCurrentInstanceValueWithMarkupExtension();               
                }
                else if (!StateCommuter.WasAssociatedRightAfterCreation)
                {
                    StateCommuter.AssociateCurrentInstanceToParent();
                }

                StateCommuter.RegisterInstanceNameToNamescope();
                lifyCycleListener.OnAfterProperties(instance);
                lifyCycleListener.OnEnd(instance);
            }

            setResult(StateCommuter);
            
            StateCommuter.DecreaseLevel();
        }

        private void ProcessCurrentInstanceValueWithMarkupExtension()
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