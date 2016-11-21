namespace WpfApp
{
    using OmniXaml;

    public class MyExtension : IMarkupExtension
    {
        public object GetValue(ExtensionValueContext context)
        {
            var type = context.BuildContext.PrefixedTypeResolver.GetTypeByPrefix(context.BuildContext.CurrentNode, "wow:MyExtension");

            return "Hi, brother";
        }
    }
}