namespace OmniXaml.NewAssembler
{
    public abstract class Command
    {
        protected Command(SuperObjectAssembler assembler)
        {
            Assembler = assembler;
        }

        protected SuperObjectAssembler Assembler { get; }

        public abstract void Execute();
        protected StateCommuter StateCommuter => Assembler.StateCommuter;        
    }
}