namespace OmniXaml.NewAssembler.Commands
{
    using System.Collections;
    using Typing;

    public class GetObjectCommand : Command
    {
        public GetObjectCommand(SuperObjectAssembler superObjectAssembler) : base(superObjectAssembler)
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