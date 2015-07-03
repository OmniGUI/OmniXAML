namespace OmniXaml.NewAssembler.Commands
{
    public class GetObjectCommand : Command
    {
        public GetObjectCommand(SuperObjectAssembler superObjectAssembler) : base(superObjectAssembler)
        {            
        }

        public override void Execute()
        {
            StateCommuter.ConfigureForGetObject();            
        }       
    }
}