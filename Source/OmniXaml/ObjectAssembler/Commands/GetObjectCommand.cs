namespace OmniXaml.ObjectAssembler.Commands
{
    using System.Collections;

    public class GetObjectCommand : Command
    {
        public GetObjectCommand(StateCommuter stateCommuter) : base(stateCommuter)
        {            
        }

        public override void Execute()
        {
            var previousMember = StateCommuter.Current.Member;

            StateCommuter.RaiseLevel();
            StateCommuter.Current.IsGetObject = true;
            var instanceToGet = StateCommuter.ValueOfPreviousInstanceAndItsMember;
            StateCommuter.Current.Instance = instanceToGet;
            StateCommuter.Current.Member = previousMember;

            var collection = instanceToGet as ICollection;
            if (collection != null)
            {
                StateCommuter.Current.Collection = collection;
            }
        }

        public override string ToString()
        {
            return "Get Object";
        }
    }
}