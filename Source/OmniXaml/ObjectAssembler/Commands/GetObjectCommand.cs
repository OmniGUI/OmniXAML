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
            var previousMember = StateCommuter.Current.XamlMember;

            StateCommuter.RaiseLevel();
            StateCommuter.Current.IsGetObject = true;
            var instanceToGet = StateCommuter.ValueOfPreviousInstanceAndItsMember;
            StateCommuter.Current.Instance = instanceToGet;
            StateCommuter.Current.XamlMember = previousMember;

            var collection = instanceToGet as ICollection;
            if (collection != null)
            {
                StateCommuter.Current.Collection = collection;
            }
        }       
    }
}