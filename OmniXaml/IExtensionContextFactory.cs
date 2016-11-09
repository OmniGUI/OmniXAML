namespace OmniXaml
{
    public interface IExtensionContextFactory
    {
        ExtensionValueContext CreateExtensionContext(Assignment assignment, BuildContext buildContext);
    }
}