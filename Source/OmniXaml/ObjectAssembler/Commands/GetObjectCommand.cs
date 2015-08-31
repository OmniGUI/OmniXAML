namespace OmniXaml.ObjectAssembler.Commands
{
    using System.Collections;

    public class GetObjectCommand : Command
    {
        public GetObjectCommand(ObjectAssembler objectAssembler) : base(objectAssembler)
        {            
        }

        public override void Execute()
        {            
            StateCommuter.RaiseLevel();
            StateCommuter.Current.IsGetObject = true;
            object val = StateCommuter.ValueOfPreviousInstanceAndItsMember;
            StateCommuter tempQualifier = StateCommuter;
            tempQualifier.Current.Instance = val;

            var collection = val as ICollection;
            if (collection != null)
            {
                tempQualifier.Current.Collection = collection;
            }
        }       
    }
}