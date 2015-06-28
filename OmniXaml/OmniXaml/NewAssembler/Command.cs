namespace OmniXaml.NewAssembler
{
    using Glass;

    public abstract class Command
    {
        protected Command(SuperObjectAssembler assembler)
        {
            this.Assembler = assembler;
        }

        protected SuperObjectAssembler Assembler { get; }

        public abstract void Execute();
        public StackingLinkedList<Level> State => Assembler.State;
        public Level Current => State.CurrentValue;
    }
}