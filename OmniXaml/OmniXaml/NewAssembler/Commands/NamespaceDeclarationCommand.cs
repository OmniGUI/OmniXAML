namespace OmniXaml.NewAssembler.Commands
{
    public class NamespaceDeclarationCommand : Command
    {
        private readonly NamespaceDeclaration namespaceDeclaration;

        public NamespaceDeclarationCommand(SuperObjectAssembler assembler, NamespaceDeclaration namespaceDeclaration) : base(assembler)
        {
            this.namespaceDeclaration = namespaceDeclaration;
        }

        public override void Execute()
        {            
        }
    }
}