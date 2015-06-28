namespace OmniXaml.NewAssembler
{
    public class EndObjectCommand : Command
    {
        public override void Execute()
        {        
            Assembler.Result = Current.Instance;
        }

        public EndObjectCommand(SuperObjectAssembler assembler) : base(assembler)
        {
        }
    }
}