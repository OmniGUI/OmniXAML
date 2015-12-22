namespace OmniXaml.ObjectAssembler.Commands
{
    public class NamespaceDeclarationCommand : Command
    {
        private readonly NamespaceDeclaration namespaceDeclaration;

        public NamespaceDeclarationCommand(ObjectAssembler assembler, NamespaceDeclaration namespaceDeclaration) : base(assembler)
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