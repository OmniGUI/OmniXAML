namespace OmniXaml.ObjectAssembler
{
    public abstract class Command
    {
        protected Command(StateCommuter stateCommuter)
        {
            StateCommuter = stateCommuter;
        }

        protected StateCommuter StateCommuter { get; }

        public abstract void Execute();
    }
}