namespace OmniXaml.ObjectAssembler.Commands
{
    using System.Collections;

    public class EndObjectCommand : Command
    {
        public EndObjectCommand(ObjectAssembler assembler) : base(assembler)
        {
        }

        public override void Execute()
        {
            if (!StateCommuter.Current.IsGetObject)
            {
                StateCommuter.CreateInstanceOfCurrentXamlTypeIfNotCreatedBefore();
                
                if (StateCommuter.Current.Instance is IMarkupExtension)
                {
                    object val = StateCommuter.GetValueProvidedByMarkupExtension((IMarkupExtension)StateCommuter.Current.Instance);
                    StateCommuter tempQualifier = StateCommuter;
                    tempQualifier.Current.Instance = val;

                    var collection = val as ICollection;
                    if (collection != null)
                    {
                        tempQualifier.Current.Collection = collection;
                    }
                    StateCommuter.AssociateCurrentInstanceToParent();
                }
                else if (!StateCommuter.Current.WasInstanceAssignedRightAfterBeingCreated)
                {
                    StateCommuter.AssociateCurrentInstanceToParent();
                }
            }

            Assembler.Result = StateCommuter.Current.Instance;
            
            StateCommuter.DecreaseLevel();
        }      
    }
}