namespace OmniXaml.ObjectAssembler.Commands
{
    public class NamespaceDeclarationCommand : Command
    {
        private readonly NamespaceDeclaration namespaceDeclaration;

        public NamespaceDeclarationCommand(NamespaceDeclaration namespaceDeclaration, StateCommuter stateCommuter) : base(stateCommuter)
        {
            this.namespaceDeclaration = namespaceDeclaration;
        }

        public override void Execute()
        {            
        }

        public override string ToString()
        {
            return $"Namespace Declaration {namespaceDeclaration}";
        }
    }
}