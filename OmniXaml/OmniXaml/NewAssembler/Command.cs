namespace OmniXaml.NewAssembler
{
    public abstract class Command
    {
        protected Command(SuperObjectAssembler assembler)
        {
            this.Assembler = assembler;
        }

        protected SuperObjectAssembler Assembler { get; }

        public abstract void Execute();
        public AssemblerState State => Assembler.State;
        public Level Current => State.CurrentValue;
    }
}