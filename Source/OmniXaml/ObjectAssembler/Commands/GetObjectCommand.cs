namespace OmniXaml.ObjectAssembler.Commands
{
    public class GetObjectCommand : Command
    {
        public GetObjectCommand(ObjectAssembler objectAssembler) : base(objectAssembler)
        {            
        }

        public override void Execute()
        {            
            StateCommuter.RaiseLevel();
            StateCommuter.IsGetObject = true;
            StateCommuter.Instance = StateCommuter.ValueOfPreviousInstanceAndItsMember();
        }       
    }
}